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
        //게임 시작(게임 유)
        if (mMainClient.mQuizManager.isCompetitionState_Starting())
        {
            if(mMainClient.mQuizManager.getQuizType() == "quiz") //ox퀴즈
            {
                mQuizText.text = "OX퀴즈 문제를 선택하셨습니다." + System.Environment.NewLine + "5초 후 퀴즈가 시작됩니다" + System.Environment.NewLine +
                "문제 종료시 중앙에 있어도 탈락 처리 됩니다" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mRemainCompetitionTime);
            }else if (mMainClient.mQuizManager.getQuizType() == "quiz2") //4지선다형
            {
                mQuizText.text = "4지선다형 문제를 선택하셨습니다." + System.Environment.NewLine + "5초 후 퀴즈가 시작됩니다" + System.Environment.NewLine +
                "문제 종료시 중앙에 있어도 탈락 처리 됩니다" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mRemainCompetitionTime);
                if (fail == false)
                {
                    StartCoroutine(EndQuiz4AfterDelay(10f)); //CenterWall 생성 시간 (텍스트 출력 후 10초) 수정가능
                }
            }
            else if (mMainClient.mQuizManager.getQuizType() == "quiz3") //it퀴즈
            {
                mQuizText.text = "IT퀴즈를 선택하셨습니다." + System.Environment.NewLine + "5초 후 퀴즈가 시작됩니다" + System.Environment.NewLine +
                "문제 종료시 중앙에 있어도 탈락 처리 됩니다" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mRemainCompetitionTime);
                if (fail == false)
                {
                    StartCoroutine(EndQuiz4AfterDelay(10f)); //CenterWall 생성 시간 (텍스트 출력 후 10초) 수정가능
                }
            }
            IEnumerator EndQuiz4AfterDelay(float delay)
            {
                yield return new WaitForSeconds(delay);
                mFailWall.SetActive(true);
                fail = true;
            }
        }
        //게임 Play
        if (mMainClient.mQuizManager.isCompetitionState_QuizPlay())
        {
            if (mMainClient.mQuizManager.getQuizType() == "quiz") //ox퀴즈
            {
                mMainClient.mQuizManager.cleanFloor = true;
                // 문제출력
                mQuizText.text = mMainClient.mQuizManager.getQuizContentOX() + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.getAnswerTimeOut());
                //시간
                mMainClient.mQuizManager.mAnswerTimeOut -= Time.deltaTime;
            }
            else if (mMainClient.mQuizManager.getQuizType() == "quiz2") //4지선다형
            {
                mCube.MoveCubes();
                // 문제출력
                mQuizText.text = mMainClient.mQuizManager.getQuizContentFour() + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.getAnswerTimeOut());
                fEx1Text.text = mMainClient.mQuizManager.getQuizEx1Four();
                fEx2Text.text = mMainClient.mQuizManager.getQuizEx2Four();
                fEx3Text.text = mMainClient.mQuizManager.getQuizEx3Four();
                fEx4Text.text = mMainClient.mQuizManager.getQuizEx4Four();
                //시간
                mMainClient.mQuizManager.mAnswerTimeOut -= Time.deltaTime;
            }
            else if(mMainClient.mQuizManager.getQuizType() == "quiz3") //it퀴즈
            {
                mCube.MoveCubes();
                // 문제출력
                mQuizText.text = mMainClient.mQuizManager.getQuizContentIT() + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.getAnswerTimeOut());
                itEx1Text.text = mMainClient.mQuizManager.getQuizEx1IT();
                itEx2Text.text = mMainClient.mQuizManager.getQuizEx2IT();
                itEx3Text.text = mMainClient.mQuizManager.getQuizEx3IT();
                itEx4Text.text = mMainClient.mQuizManager.getQuizEx4IT();
                //시간
                mMainClient.mQuizManager.mAnswerTimeOut -= Time.deltaTime;
            }
        }
        //답 비교 / 결과 출력
        if (mMainClient.mQuizManager.isCompetitionState_QuizAnswer())
        {
            if (mMainClient.mQuizManager.getQuizType() == "quiz") //ox퀴즈
            {
                //정답 비교
                string result = (mMainClient.mQuizManager.getQuizAnswerOX() == "O" ? "O" : "X");
                string explain = mMainClient.mQuizManager.getQuizExplainOX();
                //시간
                mMainClient.mQuizManager.mNextAnswerDelayTimeOut -= Time.deltaTime;
                if (result == "X")
                {
                    mQuizText.text = "결과는 : " + result + "입니다" + System.Environment.NewLine + "설명 : " + explain + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mNextAnswerDelayTimeOut);
                    CheckFloor();
                }
                else
                {
                    //결과 출력
                    mQuizText.text = "결과는 : " + result + "입니다" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mNextAnswerDelayTimeOut);
                    CheckFloor();
                }
            }
            else if (mMainClient.mQuizManager.getQuizType() == "quiz2") //4지선다형
            {
                //시간
                mMainClient.mQuizManager.mNextAnswerDelayTimeOut -= Time.deltaTime;
                //결과 출력
                mQuizText.text = "결과는 : " + mMainClient.mQuizManager.getQuizAnswerFour() + "입니다" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mNextAnswerDelayTimeOut);
                CheckAnswer();
            }
            else if (mMainClient.mQuizManager.getQuizType() == "quiz3") //it퀴즈
            {
                //시간
                mMainClient.mQuizManager.mNextAnswerDelayTimeOut -= Time.deltaTime;
                //결과 출력
                mQuizText.text = "결과는 : " + mMainClient.mQuizManager.getQuizAnswerIT() + "입니다" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mNextAnswerDelayTimeOut);
                CheckAnswer2();
            }
        }
        //다음 퀴즈 넘어감
        if (mMainClient.mQuizManager.isCompetitionState_QuizNext())
        {
            if(mMainClient.mQuizManager.getQuizType() == "quiz") //ox퀴즈
            {
                if (mMainClient.mQuizManager.nextAnswerOX()) //퀴즈가 남아있을때
                {
                    mMainClient.mQuizManager.mNextAnswerDelayTimeOut = 5.0f;
                }

                if (mMainClient.mQuizManager.getRemainQuizCountOX() == 0) //퀴즈가 남아있지 않을때 종료
                {
                    mQuizText.text = "퀴즈가 종료되었습니다";

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
            else if (mMainClient.mQuizManager.getQuizType() == "quiz2") //4지선다형
            {
                if (mMainClient.mQuizManager.nextAnswerFour()) //퀴즈가 남아있을때
                {
                    mMainClient.mQuizManager.mNextAnswerDelayTimeOut = 5.0f;
                }

                if (mMainClient.mQuizManager.getRemainQuizCountFour() == 0) //퀴즈가 남아있지 않을때 종료
                {
                    mPMotion.ClearPlayer();
                    fail = false;
                    mQuizText.text = "퀴즈가 종료되었습니다";

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
            else if (mMainClient.mQuizManager.getQuizType() == "quiz3") //it퀴즈
            {
                if (mMainClient.mQuizManager.nextAnswerIT()) //퀴즈가 남아있을때
                {
                    mMainClient.mQuizManager.mNextAnswerDelayTimeOut = 5.0f;
                }

                if (mMainClient.mQuizManager.getRemainQuizCountIT() == 0) //퀴즈가 남아있지 않을때 종료
                {
                    mPMotion.ClearPlayer();
                    fail = false;
                    mQuizText.text = "퀴즈가 종료되었습니다";

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

    //발판(O/X)과 정답 확인 OX퀴즈
    public void CheckFloor()
    {
        //cleanFloor가 true이고 게임이 시작되면
        if (mMainClient.mQuizManager.cleanFloor && !mMainClient.mQuizManager.isCompetitionPlay())
        {
            //문제 정답이 O일 때 X 발판을 Die로 변경하고, X일 때 O 발판을 Die로 변경
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

    //4지선다형
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

    //it퀴즈
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