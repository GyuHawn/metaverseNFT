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
    public GameObject mWinner;
    public GameObject mPlayer;

    public GameObject mOobj;
    public GameObject mXobj;

    void Awake()
    {
        mMainClient = GameObject.FindObjectOfType<MainClient>();
        mTerrain = GameObject.Find("Terrain");
        mWinner = GameObject.Find("Trophy");
        mPlayer = GameObject.Find("Player");
    }

    void Update()
    {
        //���� ����(���� ��)
        if (mMainClient.mQuizManager.isCompetitionState_Starting())
        {
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
                    mQuizCanvas.gameObject.SetActive(false);
                }

                ClearFloor();

                mPlayer.GetComponent<PlayerMotion>().save = false;
                mWinner.GetComponent<Winner>().isFollowing = false;
                mWinner.GetComponent<Winner>().winner.transform.position = new Vector3(8, 36f, 9);
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
                mOobj.gameObject.tag = "Quiz";
                mXobj.gameObject.tag = "Die";
            }
            else if (mMainClient.mQuizManager.checkQuizAnswer("X"))
            {
                mTerrain.GetComponent<TerrainGenerator>().mOFloor.gameObject.tag = "Die";
                mTerrain.GetComponent<TerrainGenerator>().mCenterFloor.gameObject.tag = "Die";
                mTerrain.GetComponent<TerrainGenerator>().mXFloor.gameObject.tag = "Quiz";
                mOobj.gameObject.tag = "Die";
                mXobj.gameObject.tag = "Quiz";
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
        mOobj.gameObject.tag = "Quiz";
        mXobj.gameObject.tag = "Quiz";
    }
}