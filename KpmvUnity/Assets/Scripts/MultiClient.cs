using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MultiClient : MonoBehaviour
{
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

    public class Client : JcCtUnity1.JcCtUnity1
    {
        public int cti;
        static public void qv(string s1) { Debug.Log(s1); }

        public Client() : base(System.Text.Encoding.Unicode) { }

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
                case 2:
                    {
                        int pIndex = pkrd.rInt32s();
                        cti = pkrd.rInt32s();
                        qv("server 수신 Index,cti : " + pIndex +" , "+ cti);
                        if (pIndex <0)
                        {
                            qv("인원수 초과");
                            this.disconnect(); return false;
                        }

                        LcIPT.Instance.SetIndex(pIndex);

                        using (JcCtUnity1.PkWriter1Nm pkw = new JcCtUnity1.PkWriter1Nm(20))
                        {
                           
                            this.send(pkw);
                        }
                    }
                    break;

                //request movesend all
                case 20:
                    {
                         qv("수신 20: will currentSend");
                         LcIPT.Instance.currentSend(this);

                    }
                    break;

                case 3:
                    {
                        var pidx = pkrd.rInt32s();
                        int code = pkrd.rInt32s();
                        var xx = pkrd.rReal32();
                        var yy = pkrd.rReal32();
                        var zz = pkrd.rReal32();

                        string name="", color="";
                        if (code==0)
                        {
                            name = pkrd.rStr1def();
                            color = pkrd.rStr1def();
                        }
                        if (pidx >= 0 )
                        {
                            Debug.Log("server 수신 pidx, code: " + pidx + " , "+code);
                            if (LcIPT.Instance.mPlayers[pidx] == null)
                            {
                                LcIPT.Instance.InstantiatePlayer(pidx,color);
                                GameObject t = new GameObject("myname");
                                t.transform.parent = LcIPT.Instance.mPlayers[pidx].transform;
                                t.transform.localPosition = new Vector3(0f, 8f, 0f);

                                var t1 = t.AddComponent<TextMeshPro>();
                                t1.GetComponent<TMP_Text>().font = LcIPT.Instance.m_Font;
                                t1.GetComponent<TMP_Text>().color = Color.black;
                                if (MainClient.currentUser != null) { t1.text = name; }
                                t1.alignment = TextAlignmentOptions.Center;
                                t1.fontSize = 5;

                            }
                            LcIPT.Instance.mPlayers[pidx].GetComponent<PlayerMotion>().SetKey((KeyCode)code);
                            LcIPT.Instance.mPlayers[pidx].GetComponent<PlayerMotion>().ThisUpdate();
                            LcIPT.Instance.mPlayers[pidx].transform.position = new Vector3(xx, yy, zz);                          
                        }
                    }
                    break;
                case 30:
                    {
                        var pidx = pkrd.rInt32s();
                        int code = pkrd.rInt32s();
                        var xx = pkrd.rReal32();
                        var yy = pkrd.rReal32();
                        var zz = pkrd.rReal32();

                        if (pidx >= 0 && LcIPT.Instance.mPlayers[pidx] != null)
                        {
                            LcIPT.Instance.mPlayers[pidx].transform.position += new Vector3(xx, yy, zz);
                        }
                    }
                    break;
                case 31:
                    {
                        var pidx = pkrd.rInt32s();
                        var jumpForce = pkrd.rReal32();

                        if (pidx >= 0 && LcIPT.Instance.mPlayers[pidx] != null)
                        {
                            Rigidbody rb = LcIPT.Instance.mPlayers[pidx].GetComponent<Rigidbody>();
                            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
                        }
                    }
                    break;
                case 4:
                    {
                        int pidx = pkrd.rInt32s();
                        Destroy(LcIPT.Instance.mPlayers[pidx]);

                    }
                    break;
                case 112:
                    {
                        const int maxLines = 5;
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
    static public List<string> mLines = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        mCt = new Client();

    }

    // Update is called once per frame
    void Update()
    {
        mCt.framemove();
    }
}
