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

    public bool ox; // ���� ������ �¾Ҵ��� ����
    public float mRemainCompetitionTime = 600.0f; // ���� ���۱��� �ð�
    public float mAnswerTimeOut = 10.0f; // ���� �ð�
    public float mNextAnswerDelayTimeOut = 5.0f; // ���� ���� �ð�
    public bool gameStarted = false; // ���� ���� ����
    public int mCurrentQuizIndex = 0; // ���� ���� �ε���
    public bool cleanFloor = true;

    public List<QuizData> mQuizList = new List<QuizData>();


    // - �ӽ�1 -
    public List<byte> mRdBytes = new List<byte>();
    public uint mRdCur = 0;
    public int rdNext() { return mRdBytes[(int)(mRdCur++ % mRdBytes.Count)]; }
    // - �ӽ�1 -


    public string leftTime() //���� ������ �����ð� ���Ҷ����
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
            return "OX����";
        }
        else if (getQuizType() == "four")
        {
            return "4�������� �⺻���";
        }
        else if (getQuizType() == "it")
        {
            return "IT����";
        }

        return quizType();
    }

    public void quizAdd(QuizData quiz)
    {
        mQuizList.Add(quiz);
    }

    public string getQuizType() //���� ���� : ox
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

    public bool isCompetitionPlay() //��ȸ����(��/��)
    {
        return gameStarted;
    }

    public void setCompetitionPlay() //��ȸ����(��/��) 
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
        //�ð��� �׻� �帥��.
        mRemainCompetitionTime -= Time.deltaTime;
    }

    //���� ����
    public bool isCompetitionState_Starting()
    {
        //���� �������� :
        //���� ���� ���� > 0 
        if (getQuizType() == "ox" || getQuizType() == "four" || getQuizType() == "it") //OX����
        {
            if (getRemainQuizCount() > 0)
            {
                //���������� true�̰� ������۽ð� > 0
                if (!isCompetitionPlay() && mRemainCompetitionTime > 0)
                {
                    return true;
                }
            }
        }
        //���� ���� ���� ���� 0 ���� �̰ų� ���������� false �̰� ������۽ð��� 0 ���� �϶� false��
        return false;
    }

    //���� Play
    public bool isCompetitionState_QuizPlay()
    {
        //���� Play���� :
        //���� ���� ���� > 0 
        if (getQuizType() == "ox" || getQuizType() == "four" || getQuizType() == "it") //OX����
        {
            if (getRemainQuizCount() > 0)
            {
                //���������� true�̰� ������۽ð� > 0
                if (!isCompetitionPlay() && mRemainCompetitionTime > 0)
                {

                }
                //���� �ð�(���ݼ���5��) > 0
                else if (getAnswerTimeOut() > 0)
                {
                    return true;
                }
            }
        }
        //���� �������� ������ 0 ���� �̰ų� ���� ������ false�̰� ������۽ð��� 0 ���� �̰ų� ������ ���� ���ð��� 0���� ������ false
        return false;
    }

    //���� Ǯ��
    public bool isCompetitionState_QuizAnswer()
    {
        //���� Ǫ�� ���� :
        //���� ���� ���� > 0 
        if (getQuizType() == "ox" || getQuizType() == "four" || getQuizType() == "it") //OX����
        {
            if (getRemainQuizCount() > 0)
            {
                //���������� true�̰� ������۽ð� > 0
                if (!isCompetitionPlay() && mRemainCompetitionTime > 0)
                {

                }
                //���� Ǫ�½ð�(���ݼ��� 5��) > 0
                else if (getAnswerTimeOut() > 0)
                {

                }
                //���� �����ð�(���ݼ��� 5��) > 0
                else if (mNextAnswerDelayTimeOut > 0)
                {
                    return true;
                }
            }
        }
        //���� �������� ������ 0 ���� �̰ų� ���� ������false�̰� ������۽ð��� 0���� �̰ų� ����Ǫ�½ð��� 0���� �۰ų�
        //���� �����γѾ�� �ð���(���ݼ����� �ð�) 0�� ���ϰ� �Ǹ� false
        return false;
    }

    //���� �������� �Ѿ��
    public bool isCompetitionState_QuizNext()
    {
        //���� �������� �Ѿ�� ���� :
        //���� ���� ���� > 0 
        if (getQuizType() == "ox" || getQuizType() == "four" || getQuizType() == "it") //OX����
        {
            if (getRemainQuizCount() > 0)
            {
                //���������� true�̰� ������۽ð� > 0
                if (!isCompetitionPlay() && mRemainCompetitionTime > 0)
                {

                }
                //���� Ǫ�½ð�(���ݼ��� 5��) > 0
                else if (getAnswerTimeOut() > 0)
                {

                }
                //���� �����ð�(���ݼ��� 5��) > 0
                else if (mNextAnswerDelayTimeOut > 0)
                {

                }
                else
                {
                    //���������� false�̰� ������۽ð��� 0���� �̰ų� ����Ǫ�½ð��� 0���� �۰ų�
                    //���� �����γѾ�� �ð���(���ݼ����� �ð�) 0�� �����̸� true
                    return true;
                }
            }
        }
        //���ӳ��� ��� 0 ���� �̰ų� ���������� false�̰� ������۽ð��� 0���� �̰ų� ����Ǫ�½ð��� 0���� �۰ų�
        //���� �����γѾ�� �ð���(���ݼ����� �ð�) 0�� �����̰ų� ������� 0�����ϰ� �Ǹ� false
        return false;
    }
}