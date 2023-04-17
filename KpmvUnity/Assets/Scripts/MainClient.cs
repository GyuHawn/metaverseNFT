using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MainClient/* : MonoBehaviour*/
{
    public QuizManager mQuizManager;
    public static PlayerObj currentUser;

    public class PlayerObj
    {
        public string mGame;
        public string mUserName;
        public string mEOA;
        public string mUserid;
        public string mPassword;
        public string mColor;
        
        public void posiSend(Client ct, bool saveDB = false)
        {
            using (JcCtUnity1.PkWriter1Nm pkw = new JcCtUnity1.PkWriter1Nm(111))
            {
                pkw.wStr1(mGame);
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
            const int maxLines = 5;
            var t1 = GameObject.Find("Text1");
            var tmp = t1.GetComponent<TextMeshProUGUI>();

            mLines.Add(message);

            if (mLines.Count > maxLines)
            {
                mLines.RemoveAt(0);
            }
            tmp.SetText(string.Join("\n", mLines));
        }
    }
    public static Quizinfo quizKind;
    public class Quizinfo
    {
        public string mGame, mTime, mWinner, mQuiz;
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

        public bool mbQuizStartrecv;
        protected override bool onRecvTake(Jc1Dn2_0.PkReader1 pkrd)
        {
            switch (pkrd.getPkt())
            {
                case 99:
                    {
                        if (!mbQuizStartrecv) //주의! 임시로 한번만 !!
                        {
                            mbQuizStartrecv = true;
                            var s1 = pkrd.rStr1def();
                            qv("게임시작 : " + s1);

                            mQuizManager.mRdBytes.Clear(); mQuizManager.mRdCur = 0;
                            int rdlen = pkrd.rInt32s();
                            for (int i = 0; i < rdlen; i++)
                            { mQuizManager.mRdBytes.Add(pkrd.rInt8u()); }

                            var qs = GameObject.Find("quizstarttext").GetComponent<QuizStart>();
                            qs.canvas2.gameObject.SetActive(false);
                            qs.canvas.gameObject.SetActive(true);
                            qs.mMainClient.mQuizManager.mRemainCompetitionTime = 5.0f;
                            qs.textOn = true;
                        }
                    }
                    break;
                case 100: 
                    {
                        var tCnt = pkrd.rInt32s(); //전체퀴즈 개수

                        for (int j = 0; j < tCnt; j++)
                        {
                            var s1 = pkrd.rStr1def();
                            var s2 = pkrd.rStr1def();
                            var s3 = pkrd.rStr1def();

                            if (s1 == "ox")
                            {
                                QuizManager.QuizData quizdata = new QuizManager.QuizData();
                                string s4 = pkrd.rStr1def();

                                qv("ServerEnter 수신 Kind: " + s1 + ", Content : " + s2 + ", correct : " + s3 + ", Explain :" + s4);

                                quizdata.mKind = s1;
                                quizdata.mContent = s2;
                                quizdata.mCorrect = s3;
                                quizdata.mExplain = s4;
                                mQuizManager.mQuizList.Add(quizdata);
                                qv("OXQuiz dbList : " + mQuizManager.mQuizList.Count);
                            }
                            else if (s1 == "four" || s1 == "it")
                            {
                                QuizManager.QuizData quizdata = new QuizManager.QuizData();
                                string s4 = pkrd.rStr1def();
                                string s5 = pkrd.rStr1def();
                                string s6 = pkrd.rStr1def();
                                string s7 = pkrd.rStr1def();

                                qv("ServerEnter 수신 Kind: " + s1 + ", Content : " + s2 + ", Correct : " + s3 + ", ex1 : " + s4 + ", ex2 : " + s5 + ", ex3 : " + s6 + ", ex4 :" + s7);

                                quizdata.mKind = s1;
                                quizdata.mContent = s2;
                                quizdata.mCorrect = s3;
                                quizdata.mExp1 = s4;
                                quizdata.mExp2 = s5;
                                quizdata.mExp3 = s6;
                                quizdata.mExp4 = s7;

                                mQuizManager.mQuizList.Add(quizdata);
                                qv("FourQuiz dbList : " + mQuizManager.mQuizList.Count);
                            }
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
                        var s5 = "";

                        for (int i = 0; i < count; i++)
                        {
                            PlayerObj mObjP = new PlayerObj();
                            s1 = pkrd.rStr1def();
                            s2 = pkrd.rStr1def();
                            s3 = pkrd.rStr1def();
                            s4 = pkrd.rStr1def();
                            s5 = pkrd.rStr1def();

                            qv("ServerEnter 수신 s1: " + s1 + " s2 : " + s2 + " s5 : " + s5);
                            mObjP.mUserName = s1;
                            mObjP.mEOA = s2;
                            mObjP.mUserid = s3;
                            mObjP.mPassword = s4;
                            mObjP.mColor = s5;
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
                        var s4 = "";
                        for (int i = 0; i < count; i++)
                        {
                            Quizinfo mQuizinfo = new Quizinfo();
                            PlayerObj mObjP = new PlayerObj();
                            s1 = pkrd.rStr1def();
                            s2 = pkrd.rStr1def();
                            s3 = pkrd.rStr1def();
                            s4 = pkrd.rStr1def();
                            qv("ServerEnter 수신 s1: " + s1 + " s2 : " + s2 + " s3: " + s3 + " s4: " + s4);
                            mQuizinfo.mGame = s1;
                            mObjP.mGame = s1;
                            mQuizinfo.mTime = s2;
                            mQuizinfo.mWinner = s3;
                            mQuizinfo.mQuiz = s4;
                            qv("Quiz Start Time : " + s2);
                            qv("선택한 Quiz : " + s4);
                            quizinfo.Add(mQuizinfo);
                        }
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
    static public string serverAddress;

   
    public void Start()
    {
        mQuizManager = new QuizManager();

        mCt = new Client(mQuizManager);
        
    }

    public void Update()
    {
        mQuizManager.framemove();
        mCt.framemove();
    }
}
