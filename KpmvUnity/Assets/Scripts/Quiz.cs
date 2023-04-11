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

    public TextMeshProUGUI fEx1Text;
    public TextMeshProUGUI fEx2Text;
    public TextMeshProUGUI fEx3Text;
    public TextMeshProUGUI fEx4Text;

    public TextMeshProUGUI itEx1Text;
    public TextMeshProUGUI itEx2Text;
    public TextMeshProUGUI itEx3Text;
    public TextMeshProUGUI itEx4Text;

    public Cube mCube;
    public PlayerMotion mPMotion;
    public GameObject mFailWall;
    public bool fail = false;

    public GameObject mOobj;
    public GameObject mXobj;
    public LcIPT lcipt;

    void Awake()
    {
        mMainClient = GameObject.FindObjectOfType<MainClient>();
        mCube = GameObject.FindObjectOfType<Cube>();
        mTerrain = GameObject.Find("Terrain");
        mWinner = GameObject.Find("Trophy");
        lcipt = GameObject.FindObjectOfType<LcIPT>();
    }

    public void Update()
    {
        //���� ����(���� ��)
        if (mMainClient.mQuizManager.isCompetitionState_Starting())
        {
            if(mMainClient.mQuizManager.getQuizType() == "quiz") //ox����
            {
                mQuizText.text = "OX���� ������ �����ϼ̽��ϴ�." + System.Environment.NewLine + "5�� �� ��� ���۵˴ϴ�" + System.Environment.NewLine +
                "���� ����� �߾ӿ� �־ Ż�� ó�� �˴ϴ�" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mRemainCompetitionTime);
            }else if (mMainClient.mQuizManager.getQuizType() == "quiz2") //4��������
            {
                mQuizText.text = "4�������� ������ �����ϼ̽��ϴ�." + System.Environment.NewLine + "5�� �� ��� ���۵˴ϴ�" + System.Environment.NewLine +
                "���� ����� �߾ӿ� �־ Ż�� ó�� �˴ϴ�" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mRemainCompetitionTime);
                if (fail == false)
                {
                    StartCoroutine(EndQuiz4AfterDelay(10f)); //CenterWall ���� �ð� (�ؽ�Ʈ ��� �� 10��) ��������
                }
            }
            else if (mMainClient.mQuizManager.getQuizType() == "quiz3") //it����
            {
                mQuizText.text = "IT��� �����ϼ̽��ϴ�." + System.Environment.NewLine + "5�� �� ��� ���۵˴ϴ�" + System.Environment.NewLine +
                "���� ����� �߾ӿ� �־ Ż�� ó�� �˴ϴ�" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mRemainCompetitionTime);
                if (fail == false)
                {
                    StartCoroutine(EndQuiz4AfterDelay(10f)); //CenterWall ���� �ð� (�ؽ�Ʈ ��� �� 10��) ��������
                }
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
            if (mMainClient.mQuizManager.getQuizType() == "quiz") //ox����
            {
                mMainClient.mQuizManager.cleanFloor = true;
                // �������
                mQuizText.text = mMainClient.mQuizManager.getQuizContentOX() + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.getAnswerTimeOut());
                //�ð�
                mMainClient.mQuizManager.mAnswerTimeOut -= Time.deltaTime;
            }
            else if (mMainClient.mQuizManager.getQuizType() == "quiz2") //4��������
            {
                mCube.MoveCubes();
                // �������
                mQuizText.text = mMainClient.mQuizManager.getQuizContentFour() + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.getAnswerTimeOut());
                fEx1Text.text = mMainClient.mQuizManager.getQuizEx1Four();
                fEx2Text.text = mMainClient.mQuizManager.getQuizEx2Four();
                fEx3Text.text = mMainClient.mQuizManager.getQuizEx3Four();
                fEx4Text.text = mMainClient.mQuizManager.getQuizEx4Four();
                //�ð�
                mMainClient.mQuizManager.mAnswerTimeOut -= Time.deltaTime;
            }
            else if(mMainClient.mQuizManager.getQuizType() == "quiz3") //it����
            {
                mCube.MoveCubes();
                // �������
                mQuizText.text = mMainClient.mQuizManager.getQuizContentIT() + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.getAnswerTimeOut());
                itEx1Text.text = mMainClient.mQuizManager.getQuizEx1IT();
                itEx2Text.text = mMainClient.mQuizManager.getQuizEx2IT();
                itEx3Text.text = mMainClient.mQuizManager.getQuizEx3IT();
                itEx4Text.text = mMainClient.mQuizManager.getQuizEx4IT();
                //�ð�
                mMainClient.mQuizManager.mAnswerTimeOut -= Time.deltaTime;
            }
        }
        //�� �� / ��� ���
        if (mMainClient.mQuizManager.isCompetitionState_QuizAnswer())
        {
            if (mMainClient.mQuizManager.getQuizType() == "quiz") //ox����
            {
                //���� ��
                string result = (mMainClient.mQuizManager.getQuizAnswerOX() == "O" ? "O" : "X");
                string explain = mMainClient.mQuizManager.getQuizExplainOX();
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
            else if (mMainClient.mQuizManager.getQuizType() == "quiz2") //4��������
            {
                //�ð�
                mMainClient.mQuizManager.mNextAnswerDelayTimeOut -= Time.deltaTime;
                //��� ���
                mQuizText.text = "����� : " + mMainClient.mQuizManager.getQuizAnswerFour() + "�Դϴ�" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mNextAnswerDelayTimeOut);
                CheckAnswer();
            }
            else if (mMainClient.mQuizManager.getQuizType() == "quiz3") //it����
            {
                //�ð�
                mMainClient.mQuizManager.mNextAnswerDelayTimeOut -= Time.deltaTime;
                //��� ���
                mQuizText.text = "����� : " + mMainClient.mQuizManager.getQuizAnswerIT() + "�Դϴ�" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mNextAnswerDelayTimeOut);
                CheckAnswer2();
            }
        }
        //���� ���� �Ѿ
        if (mMainClient.mQuizManager.isCompetitionState_QuizNext())
        {
            if(mMainClient.mQuizManager.getQuizType() == "quiz") //ox����
            {
                if (mMainClient.mQuizManager.nextAnswerOX()) //��� ����������
                {
                    mMainClient.mQuizManager.mNextAnswerDelayTimeOut = 5.0f;
                }

                if (mMainClient.mQuizManager.getRemainQuizCountOX() == 0) //��� �������� ������ ����
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

                        lcipt.go.GetComponent<PlayerMotion>().save = false;
                        mWinner.GetComponent<Winner>().isFollowing = false;
                        mWinner.GetComponent<Winner>().winner.transform.position = new Vector3(8, 36f, 9);
                    }
                }
            }
            else if (mMainClient.mQuizManager.getQuizType() == "quiz2") //4��������
            {
                if (mMainClient.mQuizManager.nextAnswerFour()) //��� ����������
                {
                    mMainClient.mQuizManager.mNextAnswerDelayTimeOut = 5.0f;
                }

                if (mMainClient.mQuizManager.getRemainQuizCountFour() == 0) //��� �������� ������ ����
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

                        lcipt.go.GetComponent<PlayerMotion>().save = false;
                        mWinner.GetComponent<Winner>().isFollowing = false;
                        mWinner.GetComponent<Winner>().winner.transform.position = new Vector3(-23, 1f, -7);
                        yield return new WaitForSeconds(5f);
                        mFailWall.SetActive(false);
                    }
                }
            }
            else if (mMainClient.mQuizManager.getQuizType() == "quiz3") //it����
            {
                if (mMainClient.mQuizManager.nextAnswerIT()) //��� ����������
                {
                    mMainClient.mQuizManager.mNextAnswerDelayTimeOut = 5.0f;
                }

                if (mMainClient.mQuizManager.getRemainQuizCountIT() == 0) //��� �������� ������ ����
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

                        lcipt.go.GetComponent<PlayerMotion>().save = false;
                        mWinner.GetComponent<Winner>().isFollowing = false;
                        mWinner.GetComponent<Winner>().winner.transform.position = new Vector3(-23, 1f, -7);
                        yield return new WaitForSeconds(5f);
                        mFailWall.SetActive(false);
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
            if (mMainClient.mQuizManager.checkQuizAnswerOX("O"))
            {
                mTerrain.GetComponent<TerrainGenerator>().mOFloor.gameObject.tag = "Quiz";
                mTerrain.GetComponent<TerrainGenerator>().mCenterFloor.gameObject.tag = "Die";
                mTerrain.GetComponent<TerrainGenerator>().mXFloor.gameObject.tag = "Die";
                mOobj.gameObject.tag = "Quiz";
                mXobj.gameObject.tag = "Die";
            }
            else if (mMainClient.mQuizManager.checkQuizAnswerOX("X"))
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
        if (!mMainClient.mQuizManager.isCompetitionPlay() || lcipt.go.gameObject.tag != "Fail")
        {
            if (mMainClient.mQuizManager.checkQuizAnswerFour("1"))
            {
                if (lcipt.go.tag != "Red")
                {
                    lcipt.go.transform.position = new Vector3(-22, -1.1f, 3);
                }
                mCube.RemoveCube1();
            }
            else if (mMainClient.mQuizManager.checkQuizAnswerFour("2"))
            {
                if (lcipt.go.tag != "Blue")
                {
                    lcipt.go.transform.position = new Vector3(-22, -1.1f, 3);
                }
                mCube.RemoveCube2();
            }
            else if (mMainClient.mQuizManager.checkQuizAnswerFour("3"))
            {
                if (lcipt.go.tag != "Yellow")
                {
                    lcipt.go.transform.position = new Vector3(-22, -1.1f, 3);
                }
                mCube.RemoveCube3();
            }
            else if (mMainClient.mQuizManager.checkQuizAnswerFour("4"))
            {
                if (lcipt.go.tag != "Green")
                {
                    lcipt.go.transform.position = new Vector3(-22, -1.1f, 3);
                }
                mCube.RemoveCube4();
            }
        }
    }

    //it����
    void CheckAnswer2()
    {
        if (!mMainClient.mQuizManager.isCompetitionPlay() || lcipt.go.gameObject.tag != "Fail")
        {
            if (mMainClient.mQuizManager.checkQuizAnswerIT("1"))
            {
                if (lcipt.go.tag != "Red")
                {
                    lcipt.go.transform.position = new Vector3(-22, -1.1f, 3);
                }
                mCube.RemoveCube1();
            }
            else if (mMainClient.mQuizManager.checkQuizAnswerIT("2"))
            {
                if (lcipt.go.tag != "Blue")
                {
                    lcipt.go.transform.position = new Vector3(-22, -1.1f, 3);
                }
                mCube.RemoveCube2();
            }
            else if (mMainClient.mQuizManager.checkQuizAnswerIT("3"))
            {
                if (lcipt.go.tag != "Yellow")
                {
                    lcipt.go.transform.position = new Vector3(-22, -1.1f, 3);
                }
                mCube.RemoveCube3();
            }
            else if (mMainClient.mQuizManager.checkQuizAnswerIT("4"))
            {
                if (lcipt.go.tag != "Green")
                {
                    lcipt.go.transform.position = new Vector3(-22, -1.1f, 3);
                }
                mCube.RemoveCube4();
            }
        }
    }
}