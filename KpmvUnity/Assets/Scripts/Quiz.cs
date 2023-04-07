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
        //게임 시작(게임 유)
        if (scene.name == "Quiz1")
        {
            //게임 시작(게임 유)
            
            if (mMainClient.mQuizManager.isCompetitionState_Starting())
            {
                mQuizText.text = "OX퀴즈 문제를 선택하셨습니다." + System.Environment.NewLine + "10초 후 퀴즈가 시작됩니다" + System.Environment.NewLine +
                  "문제 종료시 중앙에 있어도 탈락 처리 됩니다" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mRemainCompetitionTime);
            }
            //게임 Play
            if (mMainClient.mQuizManager.isCompetitionState_QuizPlay())
            {
                mMainClient.mQuizManager.cleanFloor = true;
                // 문제출력
                mQuizText.text = mMainClient.mQuizManager.getQuizContent() + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.getAnswerTimeOut());
                //시간
                mMainClient.mQuizManager.mAnswerTimeOut -= Time.deltaTime;
            }
            //답 비교 / 결과 출력
            if (mMainClient.mQuizManager.isCompetitionState_QuizAnswer())
            {
                //정답 비교
                string result = (mMainClient.mQuizManager.getQuizAnswer() == "O" ? "O" : "X");
                string explain = mMainClient.mQuizManager.getQuizExplain();
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

                //CheckFloor();
            }
            //다음 퀴즈 넘어감
            if (mMainClient.mQuizManager.isCompetitionState_QuizNext())
            {
                if (mMainClient.mQuizManager.nextAnswer()) //퀴즈가 남아있을때
                {
                    mMainClient.mQuizManager.mNextAnswerDelayTimeOut = 5.0f;
                }

                if (mMainClient.mQuizManager.getRemainQuizCount() == 0) //퀴즈가 남아있지 않을때 종료
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

                        mPlayer.GetComponent<PlayerMotion>().save = false;
                        mWinner.GetComponent<Winner>().isFollowing = false;
                        mWinner.GetComponent<Winner>().winner.transform.position = new Vector3(8, 36f, 9);
                    }
                }
            }
        }
        else if (scene.name == "Quiz2")
        {
            //게임 시작(게임 유)
            if (mMainClient.mQuizManager.isCompetitionState_Starting())
            {
                //mFailWall.SetActive(false);
                mQuizText.text = "4지선다형 문제를 선택하셨습니다." + System.Environment.NewLine + "10초 후 퀴즈가 시작됩니다" + System.Environment.NewLine +
                  "문제 종료시 중앙에 있어도 탈락 처리 됩니다" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mRemainCompetitionTime);
                if (fail == false)
                {
                    StartCoroutine(EndQuiz4AfterDelay(5f)); //CenterWall 생성 시간 (텍스트 출력 후 5초) 수정가능
                }
            }
            IEnumerator EndQuiz4AfterDelay(float delay)
            {
                yield return new WaitForSeconds(delay);
                mFailWall.SetActive(true);
                fail = true;
            }
            //게임 Play
            if (mMainClient.mQuizManager.isCompetitionState_QuizPlay())
            {
                mCube.MoveCubes();
                // 문제출력
                mQuizText.text = mMainClient.mQuizManager.getQuizContent2() + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.getAnswerTimeOut());
                mEx1Text.text = mMainClient.mQuizManager.getQuizEx1();
                mEx2Text.text = mMainClient.mQuizManager.getQuizEx2();
                mEx3Text.text = mMainClient.mQuizManager.getQuizEx3();
                mEx4Text.text = mMainClient.mQuizManager.getQuizEx4();
                //시간
                mMainClient.mQuizManager.mAnswerTimeOut -= Time.deltaTime;
            }
            //답 비교 / 결과 출력
            if (mMainClient.mQuizManager.isCompetitionState_QuizAnswer())
            {
                //시간
                mMainClient.mQuizManager.mNextAnswerDelayTimeOut -= Time.deltaTime;
                //결과 출력
                mQuizText.text = "결과는 : " + mMainClient.mQuizManager.getQuizAnswer2Correct() + "입니다" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mNextAnswerDelayTimeOut);
                CheckAnswer();
            }

            //다음 퀴즈 넘어감
            if (mMainClient.mQuizManager.isCompetitionState_QuizNext())
            {
                if (mMainClient.mQuizManager.nextAnswer2()) //퀴즈가 남아있을때
                {
                    mMainClient.mQuizManager.mNextAnswerDelayTimeOut = 5.0f;
                }

                if (mMainClient.mQuizManager.getRemainQuizCount2() == 0) //퀴즈가 남아있지 않을때 종료
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


    //발판(O/X)과 정답 확인
    public void CheckFloor()
    {
        //cleanFloor가 true이고 게임이 시작되면
        if (mMainClient.mQuizManager.cleanFloor && !mMainClient.mQuizManager.isCompetitionPlay())
        {
            //문제 정답이 O일 때 X 발판을 Die로 변경하고, X일 때 O 발판을 Die로 변경
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

    //4지선다형
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