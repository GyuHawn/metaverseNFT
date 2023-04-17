using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quiz : MonoBehaviour
{
    Scene scene;
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

    public GameObject WinParticle;
    public GameObject OAnswerParticle;
    public GameObject XAnswerParticle;

    void Awake()
    {
        scene = SceneManager.GetActiveScene();
        mMainClient = LcIPT.GetThis().mMc;
        mCube = GameObject.FindObjectOfType<Cube>();
        mTerrain = GameObject.Find("Terrain");
        mWinner = GameObject.Find("Trophy");
    }

    public void Update()
    {
        //게임 시작(게임 유)
        if (mMainClient.mQuizManager.isCompetitionState_Starting())
        {
            string qzmsg1 = "err ";
            switch (mMainClient.mQuizManager.getQuizType())
            {
                case "ox":
                    qzmsg1 = "OX퀴즈 문제를 선택하셨습니다.";
                    break;
                case "four":
                    qzmsg1 = "4지선다형 문제를 선택하셨습니다.";
                    break;
                case "it":
                    qzmsg1 = "IT퀴즈를 선택하셨습니다.";
                    break;
            }

            foreach (var player in LcIPT.GetThis().mPlayers)
            {
                if (player != null)
                {
                    player.GetComponent<PlayerMotion>().Alive();
                }
            }

            mQuizText.text = qzmsg1 + System.Environment.NewLine + "5초 후 퀴즈가 시작됩니다" + System.Environment.NewLine +
            "문제 종료시 중앙에 있어도 탈락 처리 됩니다" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mRemainCompetitionTime);

            if (mMainClient.mQuizManager.getQuizType() != "ox" && fail == false)
            {
                StartCoroutine(EndQuiz4AfterDelay(10f)); //CenterWall 생성 시간 (텍스트 출력 후 10초) 수정가능
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
            if (mMainClient.mQuizManager.getQuizType() == "ox") //ox퀴즈
            {
                mMainClient.mQuizManager.getQuizContent();
                mMainClient.mQuizManager.cleanFloor = true;
                // 문제출력
                mQuizText.text = mMainClient.mQuizManager.getQuizContent() + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.getAnswerTimeOut());
                //시간
                mMainClient.mQuizManager.mAnswerTimeOut -= Time.deltaTime;
            }
            else if (mMainClient.mQuizManager.getQuizType() == "four" || mMainClient.mQuizManager.getQuizType() == "it") //4지선다형
            {
                mCube.MoveCubes();
                // 문제출력
                mQuizText.text = mMainClient.mQuizManager.getQuizContent() + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.getAnswerTimeOut());
                Ex1Text.text = mMainClient.mQuizManager.getQuizEx1();
                Ex2Text.text = mMainClient.mQuizManager.getQuizEx2();
                Ex3Text.text = mMainClient.mQuizManager.getQuizEx3();
                Ex4Text.text = mMainClient.mQuizManager.getQuizEx4();
                //시간
                mMainClient.mQuizManager.mAnswerTimeOut -= Time.deltaTime;
            }
        }
        //답 비교 / 결과 출력
        if (mMainClient.mQuizManager.isCompetitionState_QuizAnswer())
        {
            if (mMainClient.mQuizManager.getQuizType() == "ox") //ox퀴즈
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
            }
            else if (mMainClient.mQuizManager.getQuizType() == "four" || mMainClient.mQuizManager.getQuizType() == "it") //4지선다형
            {
                //시간
                mMainClient.mQuizManager.mNextAnswerDelayTimeOut -= Time.deltaTime;
                //결과 출력
                mQuizText.text = "결과는 : " + mMainClient.mQuizManager.getQuizAnswer() + "입니다" + System.Environment.NewLine + Mathf.Round(mMainClient.mQuizManager.mNextAnswerDelayTimeOut);
                CheckAnswer();
            }
        }
        //다음 퀴즈 넘어감
        if (mMainClient.mQuizManager.isCompetitionState_QuizNext())
        {
            if (mMainClient.mQuizManager.getQuizType() == "ox" || mMainClient.mQuizManager.getQuizType() == "four" || mMainClient.mQuizManager.getQuizType() == "it") //ox퀴즈
            {
                if (mMainClient.mQuizManager.nextAnswer()) //퀴즈가 남아있을때
                {
                    mMainClient.mQuizManager.mNextAnswerDelayTimeOut = 5.0f;
                }

                if (mMainClient.mQuizManager.getRemainQuizCount() == 0) //퀴즈가 남아있지 않을때 종료
                {
                    if(mMainClient.mQuizManager.getQuizType() == "ox")
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

                            LcIPT.GetThis().go.GetComponent<PlayerMotion>().save = false;
                            mWinner.GetComponent<Winner>().mWinner = null;
                            mWinner.GetComponent<Winner>().mTrophy.transform.position = new Vector3(8, 36f, 9);
                            if (scene.name == "Quiz1")
                            {
                                WinParticle.gameObject.SetActive(true);
                                yield return new WaitForSeconds(10f);
                                WinParticle.gameObject.SetActive(false);
                            }
                        }
                    }else if(mMainClient.mQuizManager.getQuizType() == "four" || mMainClient.mQuizManager.getQuizType() == "it")
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

                            LcIPT.GetThis().go.GetComponent<PlayerMotion>().save = false;
                            mWinner.GetComponent<Winner>().mWinner = null;
                            mWinner.GetComponent<Winner>().mTrophy.transform.position = new Vector3(-23, 1f, -7);
                            yield return new WaitForSeconds(5f);
                            mFailWall.SetActive(false);
                            if (WinParticle.gameObject.activeSelf == true)
                            {
                                WinParticle.gameObject.SetActive(false);
                            }
                        }
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
            if (mMainClient.mQuizManager.checkQuizAnswer("O"))
            {
                StartCoroutine(OParticle(6f));
                IEnumerator OParticle(float delay)
                {
                    OAnswerParticle.gameObject.SetActive(true);
                    yield return new WaitForSeconds(delay);
                    OAnswerParticle.gameObject.SetActive(false);
                }
                mTerrain.GetComponent<TerrainGenerator>().mOFloor.gameObject.tag = "Quiz";
                mTerrain.GetComponent<TerrainGenerator>().mCenterFloor.gameObject.tag = "Die";
                mTerrain.GetComponent<TerrainGenerator>().mXFloor.gameObject.tag = "Die";
                mOobj.gameObject.tag = "Quiz";
                mXobj.gameObject.tag = "Die";
            }
            else if (mMainClient.mQuizManager.checkQuizAnswer("X"))
            {
                StartCoroutine(XParticle(6f));
                IEnumerator XParticle(float delay)
                {
                    XAnswerParticle.gameObject.SetActive(true);
                    yield return new WaitForSeconds(delay);
                    XAnswerParticle.gameObject.SetActive(false);
                }
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