using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizManager //: MonoBehaviour
{
    enum CompetitionState
    {
        None,
        QuizLoad,
        Starting,
        QuizPlay, //Qestion,
        QuizAnswer, //Answer,
        QuizWait,
        Result,
        End,
    }

    CompetitionState mState;

    public List<int> userQuiz = new List<int>();
    public List<int> userQuiz2 = new List<int>();

    public class QuizData
    {
        public string mContent, mAnswer, mExplain;
    }

    public class QuizData2
    {
        public string mContent2, mEx1, mEx2, mEx3, mEx4, mCorrect;
    }

    public bool ox; // 문제 정답이 맞았는지 여부
    public string sname;
    public float mRemainCompetitionTime = 600.0f; // 퀴즈 시작까지 시간
    public float mAnswerTimeOut = 3.0f; // 문제 시간
    public float mNextAnswerDelayTimeOut = 3.0f; // 다음 문제 시간
    public bool gameStarted = false; // 게임 시작 여부
    public int mCurrentQuizIndex = 0; // 지금 퀴즈 인덱스
    public bool cleanFloor = true;

    public List<QuizData> mQuizList = new List<QuizData>();
    public List<QuizData2> mQuizList2 = new List<QuizData2>();

    public void quizAdd(QuizData quiz)
    {
        mQuizList.Add(quiz);
    }

    public void quizAdd2(QuizData2 quiz2)
    {
        mQuizList2.Add(quiz2);
    }

    public string getQuizAnswer()
    {
        return mQuizList[mCurrentQuizIndex].mAnswer;
    }

    public string getQuizAnswer2Correct()
    {
        return mQuizList2[mCurrentQuizIndex].mCorrect;
    }

    public string getQuizExplain()
    {
        return mQuizList[mCurrentQuizIndex].mExplain;
    }

    public float getAnswerTimeOut()
    {
        return mAnswerTimeOut;
    }

    public bool checkQuizAnswer(string answer)
    {
        return getQuizAnswer() == answer;
    }

    public bool checkQuizAnswer2(string answer)
    {
        return getQuizAnswer2Correct() == answer;
    }

    public string getQuizContent()
    {
        return mQuizList[mCurrentQuizIndex].mContent;
    }

    public string getQuizContent2()
    {
        return mQuizList2[mCurrentQuizIndex].mContent2;
    }

    public string getQuizEx1() //4지선다형 보기1번
    {
        return mQuizList2[mCurrentQuizIndex].mEx1;
    }
    public string getQuizEx2() //4지선다형 보기2번
    {
        return mQuizList2[mCurrentQuizIndex].mEx2;
    }
    public string getQuizEx3() //4지선다형 보기3번
    {
        return mQuizList2[mCurrentQuizIndex].mEx3;
    }
    public string getQuizEx4() //4지선다형 보기4번
    {
        return mQuizList2[mCurrentQuizIndex].mEx4;
    }

    public bool isCompetitionPlay() //대회여부(유/무)
    {
        return gameStarted;
    }

    public void setCompetitionPlay() //대회여부(유/무)
    {
        gameStarted = true;
        nextAnswer();
    }

    public void setCompetitionPlay2() //대회여부(유/무)
    {
        gameStarted = true;
        nextAnswer2();
    }

    public int getQuizMax()
    {
        return mQuizList.Count;
    }

    public int getQuizMax2()
    {
        return mQuizList2.Count;
    }

    public int getUsedQuizCount()
    {
        return userQuiz.Count;
    }

    public int getUsedQuizCount2()
    {
        return userQuiz2.Count;
    }

    public int getRemainQuizCount()
    {
        return getQuizMax() - getUsedQuizCount();
    }

    public int getRemainQuizCount2()
    {
        return getQuizMax2() - getUsedQuizCount2();
    }

    public void setCompetitionResult(string ss)
    {

    }

    public float getCompetitionStartTime()
    {
        return mRemainCompetitionTime;
    }
    //다음문제로
    public bool nextAnswer()
    {
        userQuiz.Add(mCurrentQuizIndex);

        if (mQuizList.Count != 0)
        {
            mCurrentQuizIndex++;
            mAnswerTimeOut = 3.0f;
        }
        return true;
    }

    public bool nextAnswer2()
    {
        userQuiz2.Add(mCurrentQuizIndex);

        if (mQuizList2.Count != 0)
        {
            mCurrentQuizIndex++;
            mAnswerTimeOut = 5.0f;
        }
        return true;
    }

    public void framemove()
    {
        //시간은 항상 흐른다.
        mRemainCompetitionTime -= Time.deltaTime;
    }

    //게임 시작
    public bool isCompetitionState_Starting()
    {
        Debug.Log("scene.name : " + sname);
        //게임 시작조건 :
        //남은 퀴즈 개수 > 0 
        if (sname == "Quiz1") //OX퀴즈
        {
            if (getRemainQuizCount() > 0)
            {
                //게임유무가 true이고 퀴즈시작시간 > 0
                if (!isCompetitionPlay() && mRemainCompetitionTime > 0)
                {
                    return true;
                }
            }
        }
        else if (sname == "Quiz2") //4지선다형
        {
            if (getRemainQuizCount2() > 0)
            {
                if (!isCompetitionPlay() && mRemainCompetitionTime > 0)
                {
                    return true;
                }
            }
        }
        //게임 남은 퀴즈 개수 0 이하 이거나 게임유무가 false 이고 퀴즈시작시간이 0 이하 일때 false로
        return false;
    }

    //게임 Play
    public bool isCompetitionState_QuizPlay()
    {
        //게임 Play조건 :
        //남은 퀴즈 개수 > 0 
        if (sname == "Quiz1") //OX퀴즈
        {
            if (getRemainQuizCount() > 0)
            {
                //게임유무가 true이고 퀴즈시작시간 > 0
                if (!isCompetitionPlay() && mRemainCompetitionTime > 0)
                {

                }
                //문제 시간(지금설정5초) > 0
                else if (getAnswerTimeOut() > 0)
                {
                    return true;
                }
            }
        }
        else if (sname == "Quiz2") //4지선다형
        {
            if (getRemainQuizCount2() > 0)
            {
                if (!isCompetitionPlay() && mRemainCompetitionTime > 0)
                {

                }
                else if (getAnswerTimeOut() > 0)
                {
                    return true;
                }
            }
        }
        //게임 남은퀴즈 개수가 0 이하 이거나 게임 유무가 false이고 퀴즈시작시간이 0 이하 이거나 문제에 대한 대답시간이 0보다 작으면 false
        return false;
    }

    //퀴즈 풀기
    public bool isCompetitionState_QuizAnswer()
    {
        //퀴즈 푸는 조건 :
        //남은 퀴즈 개수 > 0 
        if (sname == "Quiz1") //OX퀴즈
        {
            if (getRemainQuizCount() > 0)
            {
                //게임유무가 true이고 퀴즈시작시간 > 0
                if (!isCompetitionPlay() && mRemainCompetitionTime > 0)
                {

                }
                //문제 푸는시간(지금설정 5초) > 0
                else if (getAnswerTimeOut() > 0)
                {

                }
                //다음 문제시간(지금설정 5초) > 0
                else if (mNextAnswerDelayTimeOut > 0)
                {
                    return true;
                }
            }
        }
        else if (sname == "Quiz2") //4지선다형
        {
            if (getRemainQuizCount2() > 0)
            {
                if (!isCompetitionPlay() && mRemainCompetitionTime > 0)
                {

                }
                else if (getAnswerTimeOut() > 0)
                {

                }
                else if (mNextAnswerDelayTimeOut > 0)
                {
                    return true;
                }
            }
        }
        //게임 남은퀴즈 개수가 0 이하 이거나 게임 유무가false이고 퀴즈시작시간이 0이하 이거나 문제푸는시간이 0보다 작거나
        //다음 문제로넘어가는 시간이(지금설정한 시간) 0초 이하가 되면 false
        return false;
    }

    //퀴즈 다음으로 넘어가기
    public bool isCompetitionState_QuizNext()
    {
        //퀴즈 다음으로 넘어가는 조건 :
        //남은 퀴즈 개수 > 0 
        if (sname == "Quiz1") //OX퀴즈
        {
            if (getRemainQuizCount() > 0)
            {
                //게임유무가 true이고 퀴즈시작시간 > 0
                if (!isCompetitionPlay() && mRemainCompetitionTime > 0)
                {

                }
                //문제 푸는시간(지금설정 5초) > 0
                else if (getAnswerTimeOut() > 0)
                {

                }
                //다음 문제시간(지금설정 5초) > 0
                else if (mNextAnswerDelayTimeOut > 0)
                {

                }
                else
                {
                    //게임유무가 false이고 퀴즈시작시간이 0이하 이거나 문제푸는시간이 0보다 작거나
                    //다음 문제로넘어가는 시간이(지금설정한 시간) 0초 이하이면 true
                    return true;
                }
            }
        }
        else if (sname == "Quiz2") //4지선다형
        {
            if (getRemainQuizCount2() > 0)
            {
                if (!isCompetitionPlay() && mRemainCompetitionTime > 0)
                {

                }
                else if (getAnswerTimeOut() > 0)
                {

                }
                else if (mNextAnswerDelayTimeOut > 0)
                {

                }
                else
                {
                    return true;
                }
            }
        }
        //게임남은 퀴즈가 0 이하 이거나 게임유무가 false이고 퀴즈시작시간이 0이하 이거나 문제푸는시간이 0보다 작거나
        //다음 문제로넘어가는 시간이(지금설정한 시간) 0초 이하이거나 남은퀴즈가 0개이하가 되면 false
        return false;
    }
}