using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        public string mContent, mAnswer;
    }

    public bool ox; // ���� ������ �¾Ҵ��� ����

    public float mRemainCompetitionTime = 600.0f; // ���� ���۱��� �ð�
    public float mAnswerTimeOut = 3.0f; // ���� �ð�
    public float mNextAnswerDelayTimeOut = 3.0f; // ���� ���� �ð�
    public bool gameStarted = false; // ���� ���� ����
    public int mCurrentQuizIndex = 0; // ���� ���� �ε���
    public bool cleanFloor = true;

    public List<QuizData> mQuizList = new List<QuizData>();

    public void quizAdd(QuizData quiz)
    {
        mQuizList.Add(quiz);
    }

    public string getQuizAnswer()
    {
        return mQuizList[mCurrentQuizIndex].mAnswer;
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
        return mQuizList[mCurrentQuizIndex].mContent;
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
    //����������
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

    public void framemove()
    {
        //�ð��� �׻� �帥��.
        mRemainCompetitionTime -= Time.deltaTime;
    }

    //���� ����(���⼭�� fŰ ������ ����)
    public bool isCompetitionState_Starting()
    {
        //���� �������� :
        //���� ���� ���� > 0 
        if (getRemainQuizCount() > 0)
        {
            //���������� true�̰� ������۽ð� > 0
            if (!isCompetitionPlay() && mRemainCompetitionTime > 0)
            {
                return true;
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
        //���� �������� ������ 0 ���� �̰ų� ���� ������ false�̰� ������۽ð��� 0 ���� �̰ų� ������ ���� ���ð��� 0���� ������ false
        return false;
    }

    //���� Ǯ��
    public bool isCompetitionState_QuizAnswer()
    {
        //���� Ǫ�� ���� :
        //���� ���� ���� > 0 
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
        //���� �������� ������ 0 ���� �̰ų� ���� ������false�̰� ������۽ð��� 0���� �̰ų� ����Ǫ�½ð��� 0���� �۰ų�
        //���� �����γѾ�� �ð���(���ݼ����� �ð�) 0�� ���ϰ� �Ǹ� false
        return false;
    }

    //���� �������� �Ѿ��
    public bool isCompetitionState_QuizNext()
    {
        //���� �������� �Ѿ�� ���� :
        //���� ���� ���� > 0 
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
        //���ӳ��� ��� 0 ���� �̰ų� ���������� false�̰� ������۽ð��� 0���� �̰ų� ����Ǫ�½ð��� 0���� �۰ų�
        //���� �����γѾ�� �ð���(���ݼ����� �ð�) 0�� �����̰ų� ������� 0�����ϰ� �Ǹ� false
        return false;
    }
}