﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MainClient : MonoBehaviour
{

    public QuizManager mQuizManager;
    public static PlayerObj currentUser;

    public class PlayerObj
    {
        public string mUserName;
        public string mEOA;
        public string mUserid;
        public string mPassword;
        
        public void posiSend(Client ct, bool saveDB = false)
        {
            using (JcCtUnity1.PkWriter1Nm pkw = new JcCtUnity1.PkWriter1Nm(111))
            {
                pkw.wStr1(mUserName);
                pkw.wStr1(mEOA);
                ct.send(pkw);
            }
        }

        public void loginSend(Client ct, string mUserid, string mPassword)
        {
            using (JcCtUnity1.PkWriter1Nm pkw = new JcCtUnity1.PkWriter1Nm(113))
            {
                pkw.wStr1(mUserid);
                pkw.wStr1(mPassword);
                ct.send(pkw);
            }
        }
    }

    public class TexstObj
    {
        public string mText;

        public void textSend(Client ct, string message)
        {
            using (JcCtUnity1.PkWriter1Nm pkw = new JcCtUnity1.PkWriter1Nm(112))
            {
                pkw.wStr1(message);
                ct.send(pkw);
            }
        }
    }

    public class Quizinfo
    {
        public string mGame, mTime, mWinner;
    }


    public class Client : JcCtUnity1.JcCtUnity1
    {
        static public void qv(string s1) { Debug.Log(s1); }

        public QuizManager mQuizManager;
        
        public Client(QuizManager qm) : base(System.Text.Encoding.Unicode)
        {
            mQuizManager = qm;
            qv("client qm: " + (mQuizManager == null));
        }

        protected override void innLogOutput(string s1) { Debug.Log(s1); }
        protected override void onConnect(JcCtUnity1.NwRst1 rst1, System.Exception ex = null)
        {
            qv("Dbg on connect: " + rst1);
            int pkt = 1111;
            using (JcCtUnity1.PkWriter1Nm pkw = new JcCtUnity1.PkWriter1Nm(pkt))
            {
                pkw.wInt32u(2222);
            }
            qv("Dbg send packet Type:" + pkt);
        }
        protected override void onDisconnect()
        { qv("Dbg on disconnect"); }

        protected override bool onRecvTake(Jc1Dn2_0.PkReader1 pkrd)
        {
            switch (pkrd.getPkt())
            {
                case 100:
                    {
                        
                        var count = pkrd.rInt32s();

                        var s1 = "";
                        var s2 = "";

                        for (int i = 0; i < count; i++)
                        {
                            QuizManager.QuizData quizdata = new QuizManager.QuizData();
                            s1 = pkrd.rStr1def();
                            s2 = pkrd.rStr1def();

                            qv("ServerEnter 수신 s1: " + s1 + " s2 : " + s2);
                            quizdata.mContent = s1;
                            quizdata.mAnswer = s2;
                            qv("recv 100 qm: " + (mQuizManager == null));
                            qv("ql: " + mQuizManager.mQuizList);
                            qv("qd: " + quizdata);
                            mQuizManager.mQuizList.Add(quizdata);
                            qv("dbList : " + mQuizManager.mQuizList.Count);
                        }
                    }
                    break;
                case 101:
                    {
                        pdbList = new List<PlayerObj>();
                        var count = pkrd.rInt32s();

                        var s1 = "";
                        var s2 = "";
                        var s3 = "";
                        var s4 = "";

                        for (int i = 0; i < count; i++)
                        {
                            PlayerObj mObjP = new PlayerObj();
                            s1 = pkrd.rStr1def();
                            s2 = pkrd.rStr1def();
                            s3 = pkrd.rStr1def();
                            s4 = pkrd.rStr1def();

                            qv("ServerEnter 수신 s1: " + s1 + " s2 : " + s2 + " s3 : " + s3);
                            mObjP.mUserName = s1;
                            mObjP.mEOA = s2;
                            mObjP.mUserid = s3;
                            mObjP.mPassword = s4;
                            pdbList.Add(mObjP);
                            qv("Player dbList : " + pdbList.Count);
                        }
                    }
                    break;
                case 102:
                    {
                        quizinfo = new List<Quizinfo>();
                        var count = pkrd.rInt32s();
                        var s1 = "";
                        var s2 = "";
                        var s3 = "";
                        for (int i = 0; i < count; i++)
                        {
                            Quizinfo mQuizinfo = new Quizinfo();
                            s1 = pkrd.rStr1def();
                            s2 = pkrd.rStr1def();
                            s3 = pkrd.rStr1def();
                            qv("ServerEnter 수신 s1: " + s1 + " s2 : " + s2 + " s3: " + s3);
                            mQuizinfo.mGame = s1;
                            mQuizinfo.mTime = s2;
                            mQuizinfo.mWinner = s3;
                            qv("Quiz Start Time : " + s2);
                            quizinfo.Add(mQuizinfo);
                        }
                    }
                    break;
                case 112:
                    {
                        int maxLines = 5;
                        var message = pkrd.rStr1def();
                        Debug.Log(message);

                        var t1 = GameObject.Find("Text1");
                        var tmp = t1.GetComponent<TextMeshProUGUI>();

                        mLines.Add(message);

                        if (mLines.Count > maxLines)
                        {
                            mLines.RemoveAt(0);
                        }
                        tmp.SetText(string.Join("\n", mLines));
                    }
                    break;
            }
            return true;
        }
    }

    public Client mCt;

    static public List<Quizinfo> quizinfo;
    static public List<PlayerObj> pdbList;
    static public List<string> mLines = new List<string>();

    void Start()
    {
        mQuizManager = new QuizManager();

        mCt = new Client(mQuizManager);
        mCt.connect("127.0.0.3", 7777);
        Debug.Log("Client Start 1111");
    }

    void Update()
    {
        mQuizManager.framemove();
        mCt.framemove();
    }
}
