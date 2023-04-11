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
    public List<int> userQuiz2 = new List<int>();
    public List<int> userQuiz3 = new List<int>();

    public class QuizData //OX퀴즈
    {
        public string mContent, mAnswer, mExplain, mKind;
    }

    public class QuizData2 //4지선다
    {
        public string fContent, fEx1, fEx2, fEx3, fEx4, fCorrect, fKind;
    }

    public class QuizData3 //IT문제
    {
        public string itContent, itEx1, itEx2, itEx3, itEx4, itCorrect, itKind;
    }

    public bool ox; // 문제 정답이 맞았는지 여부
    public float mRemainCompetitionTime = 600.0f; // 퀴즈 시작까지 시간
    public float mAnswerTimeOut = 5.0f; // 문제 시간
    public float mNextAnswerDelayTimeOut = 3.0f; // 다음 문제 시간
    public bool gameStarted = false; // 게임 시작 여부
    public int mCurrentQuizIndex = 0; // 지금 퀴즈 인덱스
    public bool cleanFloor = true;

    public List<QuizData> mQuizList = new List<QuizData>();
    public List<QuizData2> mQuizList2 = new List<QuizData2>();
    public List<QuizData3> mQuizList3 = new List<QuizData3>();

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
        if (getQuizType() == "quiz")
        {
            return "OX퀴즈";
        }
        else if (getQuizType() == "quiz2")
        {
            return "4지선다형 기본상식";
        }
        else if (getQuizType() == "quiz3")
        {
            return "IT퀴즈";
        }

        return quizType();
    }

    public void quizAddOX(QuizData quiz)
    {
        mQuizList.Add(quiz);
    }

    public void quizAdd2Four(QuizData2 quiz2)
    {
        mQuizList2.Add(quiz2);
    }

    public void quizAddIT(QuizData3 quiz3)
    {
        mQuizList3.Add(quiz3);
    }

    public string getQuizType() //퀴즈 종류 : ox
    {
        string quizName = "";
        for (int i = 0; i<MainClient.quizinfo.Count; i++)
        {
            if (MainClient.quizinfo[i].mWinner == "")
            {
                quizName = MainClient.quizinfo[i].mQuiz;
            }
        }
        return quizName;
    }

    public string getQuizAnswerOX() //ox 정답
    {
        return mQuizList[mCurrentQuizIndex].mAnswer;
    }

    public string getQuizAnswerFour() //4지선다형 정답
    {
        return mQuizList2[mCurrentQuizIndex].fCorrect;
    }

    public string getQuizAnswerIT() //it 정답
    {
        return mQuizList3[mCurrentQuizIndex].itCorrect;
    }

    public string getQuizExplainOX() //ox 설명
    {
        return mQuizList[mCurrentQuizIndex].mExplain;
    }

    public float getAnswerTimeOut()
    {
        return mAnswerTimeOut;
    }

    public bool checkQuizAnswerOX(string answer) //ox퀴즈 정답 확인
    {
        return getQuizAnswerOX() == answer;
    }

    public bool checkQuizAnswerFour(string answer) //4지선다형 정답 확인
    {
        return getQuizAnswerFour() == answer;
    }

    public bool checkQuizAnswerIT(string answer) //it퀴즈 정답 확인
    {
        return getQuizAnswerIT() == answer;
    }

    public string getQuizContentOX() //ox퀴즈 문제
    {
        return mQuizList[mCurrentQuizIndex].mContent;
    }

    public string getQuizContentFour() //4지선다형 문제
    {
        return mQuizList2[mCurrentQuizIndex].fContent;
    }

    public string getQuizContentIT()
    {
        return mQuizList3[mCurrentQuizIndex].itContent;
    }

    public string getQuizEx1Four() //4지선다형 보기1번
    {
        return mQuizList2[mCurrentQuizIndex].fEx1;
    }
    public string getQuizEx2Four() //4지선다형 보기2번
    {
        return mQuizList2[mCurrentQuizIndex].fEx2;
    }
    public string getQuizEx3Four() //4지선다형 보기3번
    {
        return mQuizList2[mCurrentQuizIndex].fEx3;
    }
    public string getQuizEx4Four() //4지선다형 보기4번
    {
        return mQuizList2[mCurrentQuizIndex].fEx4;
    }

    public string getQuizEx1IT() //it문제 보기1번
    {
        return mQuizList3[mCurrentQuizIndex].itEx1;
    }

    public string getQuizEx2IT() //it문제 보기2번
    {
        return mQuizList3[mCurrentQuizIndex].itEx2;
    }

    public string getQuizEx3IT() //it문제 보기3번
    {
        return mQuizList3[mCurrentQuizIndex].itEx3;
    }

    public string getQuizEx4IT() //it문제 보기4번
    {
        return mQuizList3[mCurrentQuizIndex].itEx4;
    }

    public bool isCompetitionPlay() //대회여부(유/무)
    {
        return gameStarted;
    }

    public void setCompetitionPlayOX() //ox 대회여부(유/무) 
    {
        gameStarted = true;
        nextAnswerOX();
    }

    public void setCompetitionPlayFour() //4지선다형 대회여부(유/무)
    {
        gameStarted = true;
        nextAnswerFour();
    }

    public void setCompetititonPlayIT() //it퀴즈 대회여부(유/무)
    {
        gameStarted = true;
        nextAnswerIT();
    }

    public int getQuizMaxOX()
    {
        return mQuizList.Count;
    }

    public int getQuizMaxFour()
    {
        return mQuizList2.Count;
    }

    public int getQuizMaxIT()
    {
        return mQuizList3.Count;
    }

    public int getUsedQuizCountOX()
    {
        return userQuiz.Count;
    }

    public int getUsedQuizCountFour()
    {
        return userQuiz2.Count;
    }

    public int getUsedQuizCountIT()
    {
        return userQuiz3.Count;
    }

    public int getRemainQuizCountOX()
    {
        return getQuizMaxOX() - getUsedQuizCountOX();
    }

    public int getRemainQuizCountFour()
    {
        return getQuizMaxFour() - getUsedQuizCountFour();
    }

    public int getRemainQuizCountIT()
    {
        return getQuizMaxIT() - getUsedQuizCountIT();
    }

    public void setCompetitionResult(string ss)
    {

    }

    public float getCompetitionStartTime()
    {
        return mRemainCompetitionTime;
    }
    //다음문제로
    public bool nextAnswerOX()
    {
        userQuiz.Add(mCurrentQuizIndex);

        if (mQuizList.Count != 0)
        {
            mCurrentQuizIndex++;
            mAnswerTimeOut = 3.0f;
        }
        return true;
    }

    public bool nextAnswerFour()
    {
        userQuiz2.Add(mCurrentQuizIndex);

        if (mQuizList2.Count != 0)
        {
            mCurrentQuizIndex++;
            mAnswerTimeOut = 5.0f;
        }
        return true;
    }

    public bool nextAnswerIT()
    {
        userQuiz3.Add(mCurrentQuizIndex);

        if (mQuizList3.Count != 0)
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
        //게임 시작조건 :
        //남은 퀴즈 개수 > 0 
        if (getQuizType() == "quiz") //OX퀴즈
        {
            if (getRemainQuizCountOX() > 0)
            {
                //게임유무가 true이고 퀴즈시작시간 > 0
                if (!isCompetitionPlay() && mRemainCompetitionTime > 0)
                {
                    return true;
                }
            }
        }
        else if (getQuizType() == "quiz2") //4지선다형
        {
            if (getRemainQuizCountFour() > 0)
            {
                if (!isCompetitionPlay() && mRemainCompetitionTime > 0)
                {
                    return true;
                }
            }
        }
        else if (getQuizType() == "quiz3") //IT퀴즈
        {
            if (getRemainQuizCountIT() > 0)
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
        if (getQuizType() == "quiz") //OX퀴즈
        {
            if (getRemainQuizCountOX() > 0)
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
        else if (getQuizType() == "quiz2") //4지선다형
        {
            if (getRemainQuizCountFour() > 0)
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
        else if (getQuizType() == "quiz3") //it
        {
            if (getRemainQuizCountIT() > 0)
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
        if (getQuizType() == "quiz") //OX퀴즈
        {
            if (getRemainQuizCountOX() > 0)
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
        else if (getQuizType() == "quiz2") //4지선다형
        {
            if (getRemainQuizCountFour() > 0)
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
        else if (getQuizType() == "quiz3") //IT퀴즈
        {
            if (getRemainQuizCountIT() > 0)
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
        if (getQuizType() == "quiz") //OX퀴즈
        {
            if (getRemainQuizCountOX() > 0)
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
        else if (getQuizType() == "quiz2") //4지선다형
        {
            if (getRemainQuizCountFour() > 0)
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
        else if (getQuizType() == "quiz3") //IT퀴즈
        {
            if (getRemainQuizCountIT() > 0)
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