using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quiz : MonoBehaviour
{
    public TextMeshProUGUI mQuizText;
    public Canvas mQuizCanvas;
    public MainClient mMainClient;
    public GameObject mTerrain;
    public GameObject mWinner;
    public GameObject mPlayer;

    public TextMeshProUGUI Ex1Text;
    public TextMeshProUGUI Ex2Text;
    public TextMeshProUGUI Ex3Text;
    public TextMeshProUGUI Ex4Text;

    public Cube mCube;
    public PlayerMotion mPMotion;
    public GameObject mFailWall;
    public bool fail = false;

    public GameObject mOobj;
    public GameObject mXobj;

    void Awake()
    {
        mMainClient = LcIPT.GetThis().mMc;
        mCube = GameObject.FindObjectOfType<Cube>();
        mTerrain = GameObject.Find("Terrain");
        mWinner = GameObject.Find("Trophy");
    }

    public void Update()
    {
        //���� ����(���� ��)
        if (mMainClient.mQuizManager.isCompetitionState_Starting())
        {
            string qzmsg1 = "err ";
            switch (mMainClient.mQuizManager.getQuizType())
            {
                case "ox":
                    qzmsg1 = "OX���� ������ �����ϼ̽��ϴ�.";
                    break;
                case "four":
                    qzmsg1 = "4�������� ������ �����ϼ̽��ϴ�.";
                    break;
                case "it":
                    qzmsg1 = "IT��� �����ϼ̽��ϴ�.";
                    break;
            }

            foreach (var player in LcIPT.GetThis().mPlayers)
            {
                if (player != null)
                {
                    player.Alive();
                }
            }

            mQuizText.text = qzmsg1 + System.Environment.NewLine + "5�� �� ��� ���۵˴ϴ�" + System.Environment.NewLine +
            "���� ����� �߾ӿ� �־ Ż�� ó�� �˴ϴ�" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mRemainCompetitionTime);

            if (mMainClient.mQuizManager.getQuizType() != "ox" && fail == false)
            {
                StartCoroutine(EndQuiz4AfterDelay(10f)); //CenterWall ���� �ð� (�ؽ�Ʈ ��� �� 10��) ��������
            }
            IEnumerator EndQuiz4AfterDelay(float delay)
            {
                yield return new WaitForSeconds(delay);
                mFailWall.SetActive(true);
                fail = true;
            }
        }
        //���� Play
        if (mMainClient.mQuizManager.isCompetitionState_QuizPlay())
        {
            if (mMainClient.mQuizManager.getQuizType() == "ox") //ox����
            {
                mMainClient.mQuizManager.getQuizContent();
                mMainClient.mQuizManager.cleanFloor = true;
                // �������
                mQuizText.text = mMainClient.mQuizManager.getQuizContent() + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.getAnswerTimeOut());
                //�ð�
                mMainClient.mQuizManager.mAnswerTimeOut -= Time.deltaTime;
            }
            else if (mMainClient.mQuizManager.getQuizType() == "four" || mMainClient.mQuizManager.getQuizType() == "it") //4��������
            {
                mCube.MoveCubes();
                // �������
                mQuizText.text = mMainClient.mQuizManager.getQuizContent() + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.getAnswerTimeOut());
                Ex1Text.text = mMainClient.mQuizManager.getQuizEx1();
                Ex2Text.text = mMainClient.mQuizManager.getQuizEx2();
                Ex3Text.text = mMainClient.mQuizManager.getQuizEx3();
                Ex4Text.text = mMainClient.mQuizManager.getQuizEx4();
                //�ð�
                mMainClient.mQuizManager.mAnswerTimeOut -= Time.deltaTime;
            }
        }
        //�� �� / ��� ���
        if (mMainClient.mQuizManager.isCompetitionState_QuizAnswer())
        {
            if (mMainClient.mQuizManager.getQuizType() == "ox") //ox����
            {
                //���� ��
                string result = (mMainClient.mQuizManager.getQuizAnswer() == "O" ? "O" : "X");
                string explain = mMainClient.mQuizManager.getQuizExplain();
                //�ð�
                mMainClient.mQuizManager.mNextAnswerDelayTimeOut -= Time.deltaTime;
                if (result == "X")
                {
                    mQuizText.text = "����� : " + result + "�Դϴ�" + System.Environment.NewLine + "���� : " + explain + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mNextAnswerDelayTimeOut);
                    CheckFloor();
                }
                else
                {
                    //��� ���
                    mQuizText.text = "����� : " + result + "�Դϴ�" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mNextAnswerDelayTimeOut);
                    CheckFloor();
                }
            }
            else if (mMainClient.mQuizManager.getQuizType() == "four" || mMainClient.mQuizManager.getQuizType() == "it") //4��������
            {
                //�ð�
                mMainClient.mQuizManager.mNextAnswerDelayTimeOut -= Time.deltaTime;
                //��� ���
                mQuizText.text = "����� : " + mMainClient.mQuizManager.getQuizAnswer() + "�Դϴ�" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mNextAnswerDelayTimeOut);
                CheckAnswer();
            }
        }
        //���� ���� �Ѿ
        if (mMainClient.mQuizManager.isCompetitionState_QuizNext())
        {
            if (mMainClient.mQuizManager.getQuizType() == "ox" || mMainClient.mQuizManager.getQuizType() == "four" || mMainClient.mQuizManager.getQuizType() == "it") //ox����
            {
                if (mMainClient.mQuizManager.nextAnswer()) //��� ����������
                {
                    mMainClient.mQuizManager.mNextAnswerDelayTimeOut = 5.0f;
                }

                if (mMainClient.mQuizManager.getRemainQuizCount() == 0) //��� �������� ������ ����
                {
                    if(mMainClient.mQuizManager.getQuizType() == "ox")
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

                            ClearFloor();

                            LcIPT.GetThis().go.GetComponent<PlayerMotion>().save = false;
                            mWinner.GetComponent<Winner>().mWinner = null;
                            mWinner.GetComponent<Winner>().mTrophy.transform.position = new Vector3(8, 36f, 9);
                        }
                    }else if(mMainClient.mQuizManager.getQuizType() == "four" || mMainClient.mQuizManager.getQuizType() == "it")
                    {
                        mPMotion.ClearPlayer();
                        fail = false;
                        mQuizText.text = "��� ����Ǿ����ϴ�";

                        if (mQuizCanvas.gameObject.activeSelf)
                        {
                            StartCoroutine(EndQuizAfterDelay(3f));
                        }
                        IEnumerator EndQuizAfterDelay(float delay)
                        {
                            yield return new WaitForSeconds(delay);
                            mQuizCanvas.gameObject.SetActive(false);

                            LcIPT.GetThis().go.GetComponent<PlayerMotion>().save = false;
                            mWinner.GetComponent<Winner>().mWinner = null;
                            mWinner.GetComponent<Winner>().mTrophy.transform.position = new Vector3(-23, 1f, -7);
                            yield return new WaitForSeconds(5f);
                            mFailWall.SetActive(false);
                        }
                    }
                }
            }
        }
    }

    //����(O/X)�� ���� Ȯ�� OX����
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

    void CheckAnswer()
    {
        if (!mMainClient.mQuizManager.isCompetitionPlay() || LcIPT.GetThis().go.gameObject.tag != "Fail")
        {
            if (mMainClient.mQuizManager.checkQuizAnswer("1"))
            {
                if (LcIPT.GetThis().go.tag != "Red")
                {
                    LcIPT.GetThis().go.transform.position = new Vector3(-22, -1.1f, 3);
                }
                mCube.RemoveCube1();
            }
            else if (mMainClient.mQuizManager.checkQuizAnswer("2"))
            {
                if (LcIPT.GetThis().go.tag != "Blue")
                {
                    LcIPT.GetThis().go.transform.position = new Vector3(-22, -1.1f, 3);
                }
                mCube.RemoveCube2();
            }
            else if (mMainClient.mQuizManager.checkQuizAnswer("3"))
            {
                if (LcIPT.GetThis().go.tag != "Yellow")
                {
                    LcIPT.GetThis().go.transform.position = new Vector3(-22, -1.1f, 3);
                }
                mCube.RemoveCube3();
            }
            else if (mMainClient.mQuizManager.checkQuizAnswer("4"))
            {
                if (LcIPT.GetThis().go.tag != "Green")
                {
                    LcIPT.GetThis().go.transform.position = new Vector3(-22, -1.1f, 3);
                }
                mCube.RemoveCube4();
            }
        }
    }
}