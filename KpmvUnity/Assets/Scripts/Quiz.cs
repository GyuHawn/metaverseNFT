using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Quiz : MonoBehaviour
{
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

                        for (int i = 0; i < 3; i++)
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

    Client mCt;

    public TextMeshProUGUI quizText;

    private GameObject quizStart;
    private GameObject terrain; // Generator;
    private GameObject winner;

    public GameObject mOobj;
    public GameObject mXobj;
    public GameObject win;

    static public List<Obj1> dbList;
    // ���� ������ �����ϴ� ����Ʈ
    private List<int> userQuiz = new List<int>();

    private int curQuiz; // ���� ���� ����(�ʱ�ȭ)
    private bool ox; // ���� ������ �¾Ҵ��� ����

    private float nextTime = 3.0f; // ���� ���۱��� �ð�
    private float countdown = 5.0f; // ���� �ð�
    private float nextQuiz = 5.0f; // ���� ���� �ð�

    private bool gameStarted = false; // ���� ���� ����
    public int quizCount = 3; // ���� ���� ��
    private int quizIndex = 0; // �������� ������ ���� �ε���

    bool cleanFloor = true;

    void Awake()
    {
        terrain = GameObject.Find("Terrain");
        quizStart = GameObject.Find("Quiz Start");
        winner = GameObject.Find("Trophy");
    }

    void Start()
    {
        mCt = new Client();
        mCt.connect("127.0.0.3", 7777);
        Debug.Log("Start 1111");

        //ShuffleQuiz();
        CloseWall();
    }

    /*/// ������ �������� ���� �Լ�
    private void ShuffleQuiz()
    {
        for (int i = 0; i < dbList.Count; i++)
        {
            Debug.Log("Before shuffling: " + i + " " + dbList[i].mContent);
            int randomIndex = Random.Range(i, dbList.Count);
            Obj1 tempQuiz = dbList[i];
            dbList[i] = dbList[randomIndex];
            dbList[randomIndex] = tempQuiz;
            Debug.Log("After shuffling: " + i + " " + dbList[i].mContent);
        }
    }*/

    void Update()
    {
        mCt.framemove();

        if (userQuiz.Count < quizCount)
        {
            if (nextTime > 0 && !gameStarted)
            {
                nextTime -= Time.deltaTime;
                quizText.text = "10�� �� ��� ���۵˴ϴ�" + System.Environment.NewLine + 
                    "���� ����� �߾ӿ� �־ Ż�� ó�� �˴ϴ�" + System.Environment.NewLine + Mathf.Round(nextTime); // ����
            }
            else if (countdown > 0)
            {
                cleanFloor = true;
                quizText.text = dbList[curQuiz].mContent + System.Environment.NewLine + Mathf.Round(countdown); // ����
                countdown -= Time.deltaTime;
            }
            else if (nextQuiz > 0)
            {
                string result = (dbList[curQuiz].mAnswer=="O" ? "O" : "X");
                nextQuiz -= Time.deltaTime;
                quizText.text = "����� : " + result + "�Դϴ�" + System.Environment.NewLine + Mathf.Round(nextQuiz);
                CheckFloor();
                if (userQuiz.Count == quizCount)
                {
                    CheckAnswer((dbList[curQuiz].mAnswer=="X") == ox);
                }
            }
            else
            {
                if (userQuiz.Count < quizCount)
                {
                    do
                    {
                        quizIndex = Random.Range(0, dbList.Count);
                    } while (userQuiz.Contains(quizIndex));
                    userQuiz.Add(quizIndex);

                    curQuiz = quizIndex;
                    nextQuiz = 5.0f;
                    countdown = 5.0f;
                }
                ox = false;
            }
        }
        else
        {
            quizText.text = "��� ����Ǿ����ϴ�";

            StartCoroutine(EndQuizAfterDelay(3f));
            IEnumerator EndQuizAfterDelay(float delay)
            {
                yield return new WaitForSeconds(delay);
                quizStart.GetComponent<QuizStart>().EndQuiz();
            }

            terrain.GetComponent<TerrainGenerator>().mOFloor.gameObject.tag = "Quiz";
            terrain.GetComponent<TerrainGenerator>().mCenterFloor.gameObject.tag = "Quiz";
            terrain.GetComponent<TerrainGenerator>().mXFloor.gameObject.tag = "Quiz";
            mOobj.gameObject.tag = "Quiz";
            mXobj.gameObject.tag = "Quiz";

            winner.GetComponent<Winner>().winner.transform.position = new Vector3(8, 33.5f, 9);

            gameStarted = false; // ���� ����
            Invoke("ResetQuiz", 5);
        }
    }

    public void CheckFloor()
    {
        if (cleanFloor)
        {
            //���� ������ O�� �� X ������ Die�� �����ϰ�, X�� �� O ������ Die�� ����
            if (dbList[curQuiz].mAnswer == "O")
            {
                terrain.GetComponent<TerrainGenerator>().mOFloor.gameObject.tag = "Quiz";
                terrain.GetComponent<TerrainGenerator>().mCenterFloor.gameObject.tag = "Die";
                terrain.GetComponent<TerrainGenerator>().mXFloor.gameObject.tag = "Die";
                mOobj.gameObject.tag = "Quiz";
                mXobj.gameObject.tag = "Die";
            }
            else if (dbList[curQuiz].mAnswer == "X")
            {
                terrain.GetComponent<TerrainGenerator>().mOFloor.gameObject.tag = "Die";
                terrain.GetComponent<TerrainGenerator>().mCenterFloor.gameObject.tag = "Die";
                terrain.GetComponent<TerrainGenerator>().mXFloor.gameObject.tag = "Quiz";
                mOobj.gameObject.tag = "Die";
                mXobj.gameObject.tag = "Quiz";
            }
            Invoke("ClearFloor", 1f);
            cleanFloor = false;
        }
  
    } 

    public void ClearFloor()
    {
        terrain.GetComponent<TerrainGenerator>().mOFloor.gameObject.tag = "Quiz";
        terrain.GetComponent<TerrainGenerator>().mCenterFloor.gameObject.tag = "Quiz";
        terrain.GetComponent<TerrainGenerator>().mXFloor.gameObject.tag = "Quiz";
        mOobj.gameObject.tag = "Quiz";
        mXobj.gameObject.tag = "Quiz";
    }

    public void CheckAnswer(bool isCorrect)
    {
        if (isCorrect)
        {
            // ���� ó�� �� ������ ���� ����Ʈ���� ����
            dbList.RemoveAt(curQuiz);

            // ���� ���� �ε��� ����Ʈ�� �߰�
            userQuiz.Add(quizIndex);
        }
        else
        {
            // ���� ���� ó�� �� ���� ������ �Ѿ
            curQuiz++;
            nextQuiz = 5.0f;
            countdown = 5.0f;

            // ���� ���� �ε��� ����Ʈ�� �߰�
            userQuiz.Add(quizIndex);
        }
    }

    void CloseWall()
    {
        terrain.GetComponent<TerrainGenerator>().cWallz0.GetComponent<BoxCollider>().enabled = true;
        terrain.GetComponent<TerrainGenerator>().cWallz1.GetComponent<BoxCollider>().enabled = true;
        terrain.GetComponent<TerrainGenerator>().cWallx0.GetComponent<BoxCollider>().enabled = true;
        terrain.GetComponent<TerrainGenerator>().cWallx1.GetComponent<BoxCollider>().enabled = true;

    }

    void OpenWall()
    {
        terrain.GetComponent<TerrainGenerator>().cWallz0.GetComponent<BoxCollider>().enabled = false;
        terrain.GetComponent<TerrainGenerator>().cWallz1.GetComponent<BoxCollider>().enabled = false;
        terrain.GetComponent<TerrainGenerator>().cWallx0.GetComponent<BoxCollider>().enabled = false;
        terrain.GetComponent<TerrainGenerator>().cWallx1.GetComponent<BoxCollider>().enabled = false;
    }

    private void ResetQuiz()
    {
        userQuiz.Clear(); // ���� ���� �ε��� ����Ʈ �ʱ�ȭ
        quizIndex = 0; // ���� ���� ������ ���� �ε��� �ʱ�ȭ
        curQuiz = 0; // ���� ���� �ʱ�ȭ
        ox = false; // ���� ���� �ʱ�ȭ
        nextTime = 3.0f; // ���� ���۱��� �ð� �ʱ�ȭ
        countdown = 5.0f; // ���� �ð� �ʱ�ȭ
        nextQuiz = 5.0f; // ���� ���� �ð� �ʱ�ȭ
        gameStarted = false; // ���� ���� ���� �ʱ�ȭ

        winner.GetComponent<Winner>().winner.transform.position = new Vector3(8, 0f, 9);
        OpenWall();
    }
}
