using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Quiz : MonoBehaviour
{
    public QuizManager mQuizManager;

    public List<Obj1> mlist()
    {
        return dbList;
    }

    static public void qv(string s1) { Debug.Log(s1); }
    public class Obj1
    {
        public string mContent, mAnswer;
    }

    public class Client : JcCtUnity1.JcCtUnity1
    {
        //public int mX = 0;
        public Client() : base(System.Text.Encoding.Unicode) { }
        //public void qv(string s1) { innLogOutput(s1); }

        // JcCtUnity1.JcCtUnity1
        protected override void innLogOutput(string s1) { Debug.Log(s1); }
        protected override void onConnect(JcCtUnity1.NwRst1 rst1, System.Exception ex = null)
        {
            qv("Dbg on connect: " + rst1);
            int pkt = 1111;
            using (JcCtUnity1.PkWriter1Nm pkw = new JcCtUnity1.PkWriter1Nm(pkt))
            {
                pkw.wInt32u(2222);
                //this.send(pkw);
            }
            qv("Dbg send packet Type:" + pkt);
        }
        protected override void onDisconnect()
        { qv("Dbg on disconnect"); }
        //protected override bool onRecvTake(Jc1Dn2_0.PkReader1 pkrd)
        //{ qv("Dbg on recv: " + pkrd.getPkt()/* + pkrd.ReadString()*/ ); return true; }
        protected override bool onRecvTake(Jc1Dn2_0.PkReader1 pkrd)
        {
            switch (pkrd.getPkt())
            {
                case 100:
                    {
                        
                        dbList = new List<Obj1>();
                        var count = pkrd.rInt32s();

                        var s1 = "";
                        var s2 = "";

                        for (int i = 0; i < count; i++)
                        {
                            Obj1 mObj1 = new Obj1();
                             s1 = pkrd.rStr1def();
                             s2 = pkrd.rStr1def();

                            qv("ServerEnter ���� s1: " + s1 + " s2 : " + s2);
                            mObj1.mContent = s1;
                            mObj1.mAnswer = s2;
                            dbList.Add(mObj1);
                            qv("dbList : " + dbList.Count);
                        }
                    }
                    break;
            }
            return true;
        }
    }

    public Client mCt;
    
    static public List<Obj1> dbList;

    void Awake()
    {
        mQuizManager.terrain = GameObject.Find("Terrain");
        mQuizManager.quizStart = GameObject.Find("QuizStart");
        mQuizManager.winner = GameObject.Find("Trophy");
    }

    void Start()
    {
        mCt = new Client();
        mCt.connect("127.0.0.1", 7777);
        Debug.Log("Start 1111");
    }
    

    void Update()
    {
        mCt.framemove();

        //���� ����(���� ��)
        if (mQuizManager.isCompetitionState_Starting())
        {
            mQuizManager.quizText.text = "10�� �� ��� ���۵˴ϴ�" + System.Environment.NewLine +
              "���� ����� �߾ӿ� �־ Ż�� ó�� �˴ϴ�" + System.Environment.NewLine + Mathf.Round(mQuizManager.mRemainCompetitionTime);
        }
        //���� Play
        if (mQuizManager.isCompetitionState_QuizPlay())
        {
            mQuizManager.cleanFloor = true;
            mQuizManager.quizText.text = dbList[mQuizManager.curQuiz].mContent + System.Environment.NewLine + Mathf.Round(mQuizManager.getAnswerTimeOut()); // ����
            mQuizManager.mAnswerTimeOut -= Time.deltaTime;
        }
        //�� �� / ��� ���
        if (mQuizManager.isCompetitionState_QuizAnswer())
        {
            string result = (dbList[mQuizManager.curQuiz].mAnswer == "O" ? "O" : "X");
            mQuizManager.NextAnswerDelayTimeOut -= Time.deltaTime;
            //Debug.Log("NextAnswerDelayTimeOut " + NextAnswerDelayTimeOut); //�����ð�
            mQuizManager.quizText.text = "����� : " + result + "�Դϴ�" + System.Environment.NewLine + Mathf.Round(mQuizManager.NextAnswerDelayTimeOut);
            CheckFloor();
        }
        //���� ���� �Ѿ
        if(mQuizManager.isCompetitionState_QuizNext())
        {
            if (mQuizManager.nextAnswer()) //��� ����������
            {
                mQuizManager.NextAnswerDelayTimeOut = 5.0f;
            }

            if(mQuizManager.getRemainQuizCount() == 0) //��� �������� ������ ����
            {
                mQuizManager.quizText.text = "��� ����Ǿ����ϴ�";

                if (mQuizManager.canvas.gameObject.activeSelf)
                {
                    StartCoroutine(EndQuizAfterDelay(5f));
                }
                IEnumerator EndQuizAfterDelay(float delay)
                {
                    yield return new WaitForSeconds(delay);
                    mQuizManager.quizStart.GetComponent<QuizStart>().EndQuiz();
                }

                mQuizManager.terrain.GetComponent<TerrainGenerator>().mOFloor.gameObject.tag = "Quiz";
                mQuizManager.terrain.GetComponent<TerrainGenerator>().mCenterFloor.gameObject.tag = "Quiz";
                mQuizManager.terrain.GetComponent<TerrainGenerator>().mXFloor.gameObject.tag = "Quiz";
                mQuizManager.mOobj.gameObject.tag = "Quiz";
                mQuizManager.mXobj.gameObject.tag = "Quiz";

                mQuizManager.winner.GetComponent<Winner>().winner.transform.position = new Vector3(8, 33.5f, 9);

                Invoke("ResetQuiz", 5);
            }
        }
    }
    //����(O/X)�� ���� Ȯ��
    public void CheckFloor()
    {
        //cleanFloor�� true�̰� ������ ���۵Ǹ�
        if (mQuizManager.cleanFloor && !mQuizManager.isCompetitionPlay())
        {
            //���� ������ O�� �� X ������ Die�� �����ϰ�, X�� �� O ������ Die�� ����
            if (dbList[mQuizManager.curQuiz].mAnswer == "O")
            {
                mQuizManager.terrain.GetComponent<TerrainGenerator>().mOFloor.gameObject.tag = "Quiz";
                mQuizManager.terrain.GetComponent<TerrainGenerator>().mCenterFloor.gameObject.tag = "Die";
                mQuizManager.terrain.GetComponent<TerrainGenerator>().mXFloor.gameObject.tag = "Die";
                mQuizManager.mOobj.gameObject.tag = "Quiz";
                mQuizManager.mXobj.gameObject.tag = "Die";
            }
            else if (dbList[mQuizManager.curQuiz].mAnswer == "X")
            {
                mQuizManager.terrain.GetComponent<TerrainGenerator>().mOFloor.gameObject.tag = "Die";
                mQuizManager.terrain.GetComponent<TerrainGenerator>().mCenterFloor.gameObject.tag = "Die";
                mQuizManager.terrain.GetComponent<TerrainGenerator>().mXFloor.gameObject.tag = "Quiz";
                mQuizManager.mOobj.gameObject.tag = "Die";
                mQuizManager.mXobj.gameObject.tag = "Quiz";
            }
            Invoke("ClearFloor", 1f);
            mQuizManager.cleanFloor = false;
        }
    }

    public void ClearFloor()
    {
        mQuizManager.terrain.GetComponent<TerrainGenerator>().mOFloor.gameObject.tag = "Quiz";
        mQuizManager.terrain.GetComponent<TerrainGenerator>().mCenterFloor.gameObject.tag = "Quiz";
        mQuizManager.terrain.GetComponent<TerrainGenerator>().mXFloor.gameObject.tag = "Quiz";
        mQuizManager.mOobj.gameObject.tag = "Quiz";
        mQuizManager.mXobj.gameObject.tag = "Quiz";
    }

    public void CloseWall()
    {
        mQuizManager.terrain.GetComponent<TerrainGenerator>().cWallz0.GetComponent<BoxCollider>().enabled = true;
        mQuizManager.terrain.GetComponent<TerrainGenerator>().cWallz1.GetComponent<BoxCollider>().enabled = true;
        mQuizManager.terrain.GetComponent<TerrainGenerator>().cWallx0.GetComponent<BoxCollider>().enabled = true;
        mQuizManager.terrain.GetComponent<TerrainGenerator>().cWallx1.GetComponent<BoxCollider>().enabled = true;

    }

    public void OpenWall()
    {
        mQuizManager.terrain.GetComponent<TerrainGenerator>().cWallz0.GetComponent<BoxCollider>().enabled = false;
        mQuizManager.terrain.GetComponent<TerrainGenerator>().cWallz1.GetComponent<BoxCollider>().enabled = false;
        mQuizManager.terrain.GetComponent<TerrainGenerator>().cWallx0.GetComponent<BoxCollider>().enabled = false;
        mQuizManager.terrain.GetComponent<TerrainGenerator>().cWallx1.GetComponent<BoxCollider>().enabled = false;
    }
    //�ʱ�ȭ
    private void ResetQuiz() 
    {
        mQuizManager.userQuiz.Clear(); // ���� ���� �ε��� ����Ʈ �ʱ�ȭ
        mQuizManager.mCurrentQuizIndex = 0; // ���� ���� ������ ���� �ε��� �ʱ�ȭ
        mQuizManager.curQuiz = 0; // ���� ���� �ʱ�ȭ
        mQuizManager.ox = false; // ���� ���� �ʱ�ȭ
        mQuizManager.mRemainCompetitionTime = 3.0f; // ���� ���۱��� �ð� �ʱ�ȭ
        mQuizManager.mAnswerTimeOut = 5.0f; // ���� �ð� �ʱ�ȭ
        mQuizManager.NextAnswerDelayTimeOut = 5.0f; // ���� ���� �ð� �ʱ�ȭ
        mQuizManager.gameStarted = false; // ���� ���� ���� �ʱ�ȭ

        mQuizManager.winner.GetComponent<Winner>().winner.transform.position = new Vector3(8, 0f, 9);
        OpenWall();
    }
}