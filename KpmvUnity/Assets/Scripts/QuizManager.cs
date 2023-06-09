using System;
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

    public class QuizData
    {
        public string mContent, mCorrect, mExp1, mExp2, mExp3, mExp4, mExplain, mKind;
    }

    public bool ox; // 문제 정답이 맞았는지 여부
    public float mRemainCompetitionTime = 600.0f; // 퀴즈 시작까지 시간
    public float mAnswerTimeOut = 10.0f; // 문제 시간
    public float mNextAnswerDelayTimeOut = 5.0f; // 다음 문제 시간
    public bool gameStarted = false; // 게임 시작 여부
    public int mCurrentQuizIndex = 0; // 지금 퀴즈 인덱스
    public bool cleanFloor = true;

    public List<QuizData> mQuizList = new List<QuizData>();


    // - 임시1 -
    public List<byte> mRdBytes = new List<byte>();
    public uint mRdCur = 0;
    public int rdNext() { return mRdBytes[(int)(mRdCur++ % mRdBytes.Count)]; }
    // - 임시1 -


    public string leftTime() //게임 시작전 남은시간 구할때사용
    {
        string quizStart = "";
        for (int i = 0; i < MainClient.quizinfo.Count; i++)
        {
            if (MainClient.quizinfo[i].mWinner == "")
            {
                quizStart = MainClient.quizinfo[i].mTime;
            }
        }
        DateTime quizDate = DateTime.Parse(quizStart);
        string now = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
        DateTime quiznow = DateTime.Parse(now);
        TimeSpan differ = quizDate - quiznow;

        return differ.ToString();
    }

    public string quizType()
    {
        if (getQuizType() == "ox")
        {
            return "OX퀴즈";
        }
        else if (getQuizType() == "four")
        {
            return "4지선다형 기본상식";
        }
        else if (getQuizType() == "it")
        {
            return "IT퀴즈";
        }

        return quizType();
    }

    public void quizAdd(QuizData quiz)
    {
        mQuizList.Add(quiz);
    }

    public string getQuizType() //퀴즈 종류 : ox
    {
        string quizName = "";
        for (int i = 0; i < MainClient.quizinfo.Count; i++)
        {
            if (MainClient.quizinfo[i].mWinner == "")
            {
                quizName = MainClient.quizinfo[i].mQuiz;
            }
        }
        return quizName;
    }

    public string getQuizAnswer()
    {
        return mQuizList[mCurrentQuizIndex].mCorrect;
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

    public string getQuizContent()
    {
        if (mQuizList[mCurrentQuizIndex].mKind == "ox" || mQuizList[mCurrentQuizIndex].mKind == "four" || mQuizList[mCurrentQuizIndex].mKind == "it")
        {
            return mQuizList[mCurrentQuizIndex].mContent;
        }
        return mQuizList[mCurrentQuizIndex].mContent;
    }

    public string getQuizEx1()
    {
        return mQuizList[mCurrentQuizIndex].mExp1;
    }
    public string getQuizEx2()
    {
        return mQuizList[mCurrentQuizIndex].mExp2;
    }
    public string getQuizEx3()
    {
        return mQuizList[mCurrentQuizIndex].mExp3;
    }
    public string getQuizEx4()
    {
        return mQuizList[mCurrentQuizIndex].mExp4;
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

    public int getQuizMax()
    {
        return mQuizList.Count;
    }

    public int getUsedQuizCount()
    {
        return userQuiz.Count;
    }

    public int getRemainQuizCount()
    {
        return getQuizMax() - getUsedQuizCount();
    }

    public void setCompetitionResult(string ss)
    {

    }

    public float getCompetitionStartTime()
    {
        return mRemainCompetitionTime;
    }

    public bool nextAnswer()
    {
        userQuiz.Add(mCurrentQuizIndex);

        if (mQuizList[mCurrentQuizIndex].mKind == "ox" || mQuizList[mCurrentQuizIndex].mKind == "four" || mQuizList[mCurrentQuizIndex].mKind == "it")
        {
            if (mQuizList.Count != 0)
            {
                mCurrentQuizIndex++;
                mAnswerTimeOut = 10.0f;
            }
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
        //게임 시작조건 :
        //남은 퀴즈 개수 > 0 
        if (getQuizType() == "ox" || getQuizType() == "four" || getQuizType() == "it") //OX퀴즈
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
        //게임 남은 퀴즈 개수 0 이하 이거나 게임유무가 false 이고 퀴즈시작시간이 0 이하 일때 false로
        return false;
    }

    //게임 Play
    public bool isCompetitionState_QuizPlay()
    {
        //게임 Play조건 :
        //남은 퀴즈 개수 > 0 
        if (getQuizType() == "ox" || getQuizType() == "four" || getQuizType() == "it") //OX퀴즈
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
        //게임 남은퀴즈 개수가 0 이하 이거나 게임 유무가 false이고 퀴즈시작시간이 0 이하 이거나 문제에 대한 대답시간이 0보다 작으면 false
        return false;
    }

    //퀴즈 풀기
    public bool isCompetitionState_QuizAnswer()
    {
        //퀴즈 푸는 조건 :
        //남은 퀴즈 개수 > 0 
        if (getQuizType() == "ox" || getQuizType() == "four" || getQuizType() == "it") //OX퀴즈
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
        //게임 남은퀴즈 개수가 0 이하 이거나 게임 유무가false이고 퀴즈시작시간이 0이하 이거나 문제푸는시간이 0보다 작거나
        //다음 문제로넘어가는 시간이(지금설정한 시간) 0초 이하가 되면 false
        return false;
    }

    //퀴즈 다음으로 넘어가기
    public bool isCompetitionState_QuizNext()
    {
        //퀴즈 다음으로 넘어가는 조건 :
        //남은 퀴즈 개수 > 0 
        if (getQuizType() == "ox" || getQuizType() == "four" || getQuizType() == "it") //OX퀴즈
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
        //게임남은 퀴즈가 0 이하 이거나 게임유무가 false이고 퀴즈시작시간이 0이하 이거나 문제푸는시간이 0보다 작거나
        //다음 문제로넘어가는 시간이(지금설정한 시간) 0초 이하이거나 남은퀴즈가 0개이하가 되면 false
        return false;
    }
}