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

    public bool ox; // ���� ������ �¾Ҵ��� ����
    public string sname;
    public float mRemainCompetitionTime = 600.0f; // ���� ���۱��� �ð�
    public float mAnswerTimeOut = 3.0f; // ���� �ð�
    public float mNextAnswerDelayTimeOut = 3.0f; // ���� ���� �ð�
    public bool gameStarted = false; // ���� ���� ����
    public int mCurrentQuizIndex = 0; // ���� ���� �ε���
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

    public string getQuizEx1() //4�������� ����1��
    {
        return mQuizList2[mCurrentQuizIndex].mEx1;
    }
    public string getQuizEx2() //4�������� ����2��
    {
        return mQuizList2[mCurrentQuizIndex].mEx2;
    }
    public string getQuizEx3() //4�������� ����3��
    {
        return mQuizList2[mCurrentQuizIndex].mEx3;
    }
    public string getQuizEx4() //4�������� ����4��
    {
        return mQuizList2[mCurrentQuizIndex].mEx4;
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

    public void setCompetitionPlay2() //��ȸ����(��/��)
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
        //�ð��� �׻� �帥��.
        mRemainCompetitionTime -= Time.deltaTime;
    }

    //���� ����
    public bool isCompetitionState_Starting()
    {
        Debug.Log("scene.name : " + sname);
        //���� �������� :
        //���� ���� ���� > 0 
        if (sname == "Quiz1") //OX����
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
        else if (sname == "Quiz2") //4��������
        {
            if (getRemainQuizCount2() > 0)
            {
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
        if (sname == "Quiz1") //OX����
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
        else if (sname == "Quiz2") //4��������
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
        //���� �������� ������ 0 ���� �̰ų� ���� ������ false�̰� ������۽ð��� 0 ���� �̰ų� ������ ���� ���ð��� 0���� ������ false
        return false;
    }

    //���� Ǯ��
    public bool isCompetitionState_QuizAnswer()
    {
        //���� Ǫ�� ���� :
        //���� ���� ���� > 0 
        if (sname == "Quiz1") //OX����
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
        else if (sname == "Quiz2") //4��������
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
        //���� �������� ������ 0 ���� �̰ų� ���� ������false�̰� ������۽ð��� 0���� �̰ų� ����Ǫ�½ð��� 0���� �۰ų�
        //���� �����γѾ�� �ð���(���ݼ����� �ð�) 0�� ���ϰ� �Ǹ� false
        return false;
    }

    //���� �������� �Ѿ��
    public bool isCompetitionState_QuizNext()
    {
        //���� �������� �Ѿ�� ���� :
        //���� ���� ���� > 0 
        if (sname == "Quiz1") //OX����
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
        else if (sname == "Quiz2") //4��������
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
        //���ӳ��� ��� 0 ���� �̰ų� ���������� false�̰� ������۽ð��� 0���� �̰ų� ����Ǫ�½ð��� 0���� �۰ų�
        //���� �����γѾ�� �ð���(���ݼ����� �ð�) 0�� �����̰ų� ������� 0�����ϰ� �Ǹ� false
        return false;
    }
}