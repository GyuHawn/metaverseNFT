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

        //게임 시작(게임 유)
        if (mMainClient.mQuizManager.isCompetitionState_Starting())
        {
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!"+(mQuizText==null));
            mQuizText.text = "10초 후 퀴즈가 시작됩니다" + System.Environment.NewLine +
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
            //시간
            mMainClient.mQuizManager.mNextAnswerDelayTimeOut -= Time.deltaTime;
            //결과 출력
            mQuizText.text = "결과는 : " + result + "입니다" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mNextAnswerDelayTimeOut);
            CheckFloor();
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
    //게임이 시작되면 더이상 참가자가 O/X판으로 들어오지 못하게 벽을 만드는것
    //탈락자도 다시 못들어옴
    public void CloseWall()
    {
       mTerrain.GetComponent<TerrainGenerator>().cWallz0.GetComponent<BoxCollider>().enabled = true;
        mTerrain.GetComponent<TerrainGenerator>().cWallz1.GetComponent<BoxCollider>().enabled = true;
        mTerrain.GetComponent<TerrainGenerator>().cWallx0.GetComponent<BoxCollider>().enabled = true;
        mTerrain.GetComponent<TerrainGenerator>().cWallx1.GetComponent<BoxCollider>().enabled = true;

    }
    //CloserWall되어있는것을 다시 Open시키는것(ResetQuiz실행시 활성화됨)
    public void OpenWall()
    {
       mTerrain.GetComponent<TerrainGenerator>().cWallz0.GetComponent<BoxCollider>().enabled = false;
        mTerrain.GetComponent<TerrainGenerator>().cWallz1.GetComponent<BoxCollider>().enabled = false;
        mTerrain.GetComponent<TerrainGenerator>().cWallx0.GetComponent<BoxCollider>().enabled = false;
        mTerrain.GetComponent<TerrainGenerator>().cWallx1.GetComponent<BoxCollider>().enabled = false;
    }
    //초기화
    private void ResetQuiz()
    {
        mMainClient.mQuizManager.userQuiz.Clear(); // 사용된 문제 인덱스 리스트 초기화
        mMainClient.mQuizManager.mCurrentQuizIndex = 0; // 다음 문제 선택을 위한 인덱스 초기화
        mMainClient.mQuizManager.ox = false; // 정답 여부 초기화
        mMainClient.mQuizManager.mRemainCompetitionTime = 3.0f; // 퀴즈 시작까지 시간 초기화
        mMainClient.mQuizManager.mAnswerTimeOut = 5.0f; // 문제 시간 초기화
        mMainClient.mQuizManager.mNextAnswerDelayTimeOut = 5.0f; // 다음 문제 시간 초기화
        mMainClient.mQuizManager.gameStarted = false; // 게임 시작 여부 초기화

        OpenWall();
    }
}