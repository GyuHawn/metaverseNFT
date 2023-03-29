using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Quiz : MonoBehaviour
{
    public TextMeshProUGUI mQuizText;
    public Canvas mQuizCanvas;
    public MainClient mMainClient;
    public GameObject mTerrain;
    public GameObject mQuizStart;
    public GameObject mWinner;
    public GameObject mPlayer;

    // public static List<MainClient> dbList;
    void Awake()
    {
        mMainClient = GameObject.FindObjectOfType<MainClient>();
        mTerrain = GameObject.Find("Terrain");
        mQuizStart = GameObject.Find("QuizStart");
        mWinner = GameObject.Find("Trophy");
        mPlayer = GameObject.Find("Player");
    }


    void Update()
    {

        //���� ����(���� ��)
        if (mMainClient.mQuizManager.isCompetitionState_Starting())
        {
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!"+(mQuizText==null));
            mQuizText.text = "10�� �� ��� ���۵˴ϴ�" + System.Environment.NewLine +
              "���� ����� �߾ӿ� �־ Ż�� ó�� �˴ϴ�" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mRemainCompetitionTime);
        }
        //���� Play
        if (mMainClient.mQuizManager.isCompetitionState_QuizPlay())
        {
            mMainClient.mQuizManager.cleanFloor = true;
            // �������
            mQuizText.text = mMainClient.mQuizManager.getQuizContent() + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.getAnswerTimeOut());
            //�ð�
            mMainClient.mQuizManager.mAnswerTimeOut -= Time.deltaTime;
        }
        //�� �� / ��� ���
        if (mMainClient.mQuizManager.isCompetitionState_QuizAnswer())
        {
            //���� ��
            string result = (mMainClient.mQuizManager.getQuizAnswer() == "O" ? "O" : "X");
            //�ð�
            mMainClient.mQuizManager.mNextAnswerDelayTimeOut -= Time.deltaTime;
            //��� ���
            mQuizText.text = "����� : " + result + "�Դϴ�" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mNextAnswerDelayTimeOut);
            CheckFloor();
        }
        //���� ���� �Ѿ
        if (mMainClient.mQuizManager.isCompetitionState_QuizNext())
        {
            if (mMainClient.mQuizManager.nextAnswer()) //��� ����������
            {
                mMainClient.mQuizManager.mNextAnswerDelayTimeOut = 5.0f;
            }

            if (mMainClient.mQuizManager.getRemainQuizCount() == 0) //��� �������� ������ ����
            {
                mQuizText.text = "��� ����Ǿ����ϴ�";

                if (mQuizCanvas.gameObject.activeSelf)
                {
                    StartCoroutine(EndQuizAfterDelay(5f));
                }
                IEnumerator EndQuizAfterDelay(float delay)
                {
                    yield return new WaitForSeconds(delay);
                    mQuizStart.GetComponent<QuizStart>().EndQuiz();
                }

                ClearFloor();

                mPlayer.GetComponent<PlayerMotion>().save = false;
                mWinner.GetComponent<Winner>().isFollowing = false;
                mWinner.GetComponent<Winner>().winner.transform.position = new Vector3(8, 36f, 9);
                Invoke("ResetQuiz", 5);
            }
        }
    }
    //����(O/X)�� ���� Ȯ��
    public void CheckFloor()
    {
        //cleanFloor�� true�̰� ������ ���۵Ǹ�
        if (mMainClient.mQuizManager.cleanFloor && !mMainClient.mQuizManager.isCompetitionPlay())
        {
            //���� ������ O�� �� X ������ Die�� �����ϰ�, X�� �� O ������ Die�� ����
            if (mMainClient.mQuizManager.checkQuizAnswer("O"))
            {
                mTerrain.GetComponent<TerrainGenerator>().mOFloor.gameObject.tag = "Quiz";
                mTerrain.GetComponent<TerrainGenerator>().mCenterFloor.gameObject.tag = "Die";
                mTerrain.GetComponent<TerrainGenerator>().mXFloor.gameObject.tag = "Die";
                mMainClient.mQuizManager.mOobj.gameObject.tag = "Quiz";
                mMainClient.mQuizManager.mXobj.gameObject.tag = "Die";
            }
            else if (mMainClient.mQuizManager.checkQuizAnswer("X"))
            {
                mTerrain.GetComponent<TerrainGenerator>().mOFloor.gameObject.tag = "Die";
                mTerrain.GetComponent<TerrainGenerator>().mCenterFloor.gameObject.tag = "Die";
                mTerrain.GetComponent<TerrainGenerator>().mXFloor.gameObject.tag = "Quiz";
                mMainClient.mQuizManager.mOobj.gameObject.tag = "Die";
                mMainClient.mQuizManager.mXobj.gameObject.tag = "Quiz";
            }
            Invoke("ClearFloor", 1f);
            mMainClient.mQuizManager.cleanFloor = false;
        }
    }

    public void ClearFloor()
    {
        mTerrain.GetComponent<TerrainGenerator>().mOFloor.gameObject.tag = "Quiz";
        mTerrain.GetComponent<TerrainGenerator>().mCenterFloor.gameObject.tag = "Quiz";
        mTerrain.GetComponent<TerrainGenerator>().mXFloor.gameObject.tag = "Quiz";
        mMainClient.mQuizManager.mOobj.gameObject.tag = "Quiz";
        mMainClient.mQuizManager.mXobj.gameObject.tag = "Quiz";
    }
    //������ ���۵Ǹ� ���̻� �����ڰ� O/X������ ������ ���ϰ� ���� ����°�
    //Ż���ڵ� �ٽ� ������
    public void CloseWall()
    {
       mTerrain.GetComponent<TerrainGenerator>().cWallz0.GetComponent<BoxCollider>().enabled = true;
        mTerrain.GetComponent<TerrainGenerator>().cWallz1.GetComponent<BoxCollider>().enabled = true;
        mTerrain.GetComponent<TerrainGenerator>().cWallx0.GetComponent<BoxCollider>().enabled = true;
        mTerrain.GetComponent<TerrainGenerator>().cWallx1.GetComponent<BoxCollider>().enabled = true;

    }
    //CloserWall�Ǿ��ִ°��� �ٽ� Open��Ű�°�(ResetQuiz����� Ȱ��ȭ��)
    public void OpenWall()
    {
       mTerrain.GetComponent<TerrainGenerator>().cWallz0.GetComponent<BoxCollider>().enabled = false;
        mTerrain.GetComponent<TerrainGenerator>().cWallz1.GetComponent<BoxCollider>().enabled = false;
        mTerrain.GetComponent<TerrainGenerator>().cWallx0.GetComponent<BoxCollider>().enabled = false;
        mTerrain.GetComponent<TerrainGenerator>().cWallx1.GetComponent<BoxCollider>().enabled = false;
    }
    //�ʱ�ȭ
    private void ResetQuiz()
    {
        mMainClient.mQuizManager.userQuiz.Clear(); // ���� ���� �ε��� ����Ʈ �ʱ�ȭ
        mMainClient.mQuizManager.mCurrentQuizIndex = 0; // ���� ���� ������ ���� �ε��� �ʱ�ȭ
        mMainClient.mQuizManager.ox = false; // ���� ���� �ʱ�ȭ
        mMainClient.mQuizManager.mRemainCompetitionTime = 3.0f; // ���� ���۱��� �ð� �ʱ�ȭ
        mMainClient.mQuizManager.mAnswerTimeOut = 5.0f; // ���� �ð� �ʱ�ȭ
        mMainClient.mQuizManager.mNextAnswerDelayTimeOut = 5.0f; // ���� ���� �ð� �ʱ�ȭ
        mMainClient.mQuizManager.gameStarted = false; // ���� ���� ���� �ʱ�ȭ

        OpenWall();
    }
}