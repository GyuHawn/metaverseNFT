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
        //게임 시작(게임 유)
        if (mMainClient.mQuizManager.isCompetitionState_Starting())
        {
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
                    mQuizCanvas.gameObject.SetActive(false);
                }

                ClearFloor();

                mPlayer.GetComponent<PlayerMotion>().save = false;
                mWinner.GetComponent<Winner>().isFollowing = false;
                mWinner.GetComponent<Winner>().winner.transform.position = new Vector3(8, 36f, 9);
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
}