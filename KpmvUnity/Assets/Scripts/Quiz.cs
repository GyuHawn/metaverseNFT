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

    public TextMeshProUGUI mEx1Text;
    public TextMeshProUGUI mEx2Text;
    public TextMeshProUGUI mEx3Text;
    public TextMeshProUGUI mEx4Text;
    public Cube mCube;
    public PlayerMotion mPMotion;
    public GameObject mFailWall;
    public bool fail = false;

    public GameObject mOobj;
    public GameObject mXobj;

    void Awake()
    {
        mMainClient = GameObject.FindObjectOfType<MainClient>();
        mCube = GameObject.FindObjectOfType<Cube>();
        mTerrain = GameObject.Find("Terrain");
        mWinner = GameObject.Find("Trophy");
        mPlayer = GameObject.Find("Player");
    }

    public void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        mMainClient.mQuizManager.sname = scene.name;
        //���� ����(���� ��)
        if (scene.name == "Quiz1")
        {
            //���� ����(���� ��)
            
            if (mMainClient.mQuizManager.isCompetitionState_Starting())
            {
                mQuizText.text = "OX���� ������ �����ϼ̽��ϴ�." + System.Environment.NewLine + "10�� �� ��� ���۵˴ϴ�" + System.Environment.NewLine +
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

                //CheckFloor();
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

                        ClearFloor();

                        mPlayer.GetComponent<PlayerMotion>().save = false;
                        mWinner.GetComponent<Winner>().isFollowing = false;
                        mWinner.GetComponent<Winner>().winner.transform.position = new Vector3(8, 36f, 9);
                    }
                }
            }
        }
        else if (scene.name == "Quiz2")
        {
            //���� ����(���� ��)
            if (mMainClient.mQuizManager.isCompetitionState_Starting())
            {
                //mFailWall.SetActive(false);
                mQuizText.text = "4�������� ������ �����ϼ̽��ϴ�." + System.Environment.NewLine + "10�� �� ��� ���۵˴ϴ�" + System.Environment.NewLine +
                  "���� ����� �߾ӿ� �־ Ż�� ó�� �˴ϴ�" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mRemainCompetitionTime);
                if (fail == false)
                {
                    StartCoroutine(EndQuiz4AfterDelay(5f)); //CenterWall ���� �ð� (�ؽ�Ʈ ��� �� 5��) ��������
                }
            }
            IEnumerator EndQuiz4AfterDelay(float delay)
            {
                yield return new WaitForSeconds(delay);
                mFailWall.SetActive(true);
                fail = true;
            }
            //���� Play
            if (mMainClient.mQuizManager.isCompetitionState_QuizPlay())
            {
                mCube.MoveCubes();
                // �������
                mQuizText.text = mMainClient.mQuizManager.getQuizContent2() + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.getAnswerTimeOut());
                mEx1Text.text = mMainClient.mQuizManager.getQuizEx1();
                mEx2Text.text = mMainClient.mQuizManager.getQuizEx2();
                mEx3Text.text = mMainClient.mQuizManager.getQuizEx3();
                mEx4Text.text = mMainClient.mQuizManager.getQuizEx4();
                //�ð�
                mMainClient.mQuizManager.mAnswerTimeOut -= Time.deltaTime;
            }
            //�� �� / ��� ���
            if (mMainClient.mQuizManager.isCompetitionState_QuizAnswer())
            {
                //�ð�
                mMainClient.mQuizManager.mNextAnswerDelayTimeOut -= Time.deltaTime;
                //��� ���
                mQuizText.text = "����� : " + mMainClient.mQuizManager.getQuizAnswer2Correct() + "�Դϴ�" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mNextAnswerDelayTimeOut);
                CheckAnswer();
            }

            //���� ���� �Ѿ
            if (mMainClient.mQuizManager.isCompetitionState_QuizNext())
            {
                if (mMainClient.mQuizManager.nextAnswer2()) //��� ����������
                {
                    mMainClient.mQuizManager.mNextAnswerDelayTimeOut = 5.0f;
                }

                if (mMainClient.mQuizManager.getRemainQuizCount2() == 0) //��� �������� ������ ����
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

                        mPlayer.GetComponent<PlayerMotion>().save = false;
                        mWinner.GetComponent<Winner>().isFollowing = false;
                        mWinner.GetComponent<Winner>().winner.transform.position = new Vector3(-23, 1f, -7);
                        yield return new WaitForSeconds(5f);
                        mFailWall.SetActive(false);
                    }
                }
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

    //4��������
    void CheckAnswer()
    {
        if (!mMainClient.mQuizManager.isCompetitionPlay() || mPlayer.gameObject.tag != "Fail")
        {
            if (mMainClient.mQuizManager.checkQuizAnswer2("1"))
            {
                if (mPlayer.tag != "Red")
                {
                    mPlayer.transform.position = new Vector3(-22, -1.1f, 3);
                }
                mCube.RemoveCube1();
            }
            else if (mMainClient.mQuizManager.checkQuizAnswer2("2"))
            {
                if (mPlayer.tag != "Blue")
                {
                    mPlayer.transform.position = new Vector3(-22, -1.1f, 3);
                }
                mCube.RemoveCube2();
            }
            else if (mMainClient.mQuizManager.checkQuizAnswer2("3"))
            {
                if (mPlayer.tag != "Yellow")
                {
                    mPlayer.transform.position = new Vector3(-22, -1.1f, 3);
                }
                mCube.RemoveCube3();
            }
            else if (mMainClient.mQuizManager.checkQuizAnswer2("4"))
            {
                if (mPlayer.tag != "Green")
                {
                    mPlayer.transform.position = new Vector3(-22, -1.1f, 3);
                }
                mCube.RemoveCube4();
            }
        }
    }
}