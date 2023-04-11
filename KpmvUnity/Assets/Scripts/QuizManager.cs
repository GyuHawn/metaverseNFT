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

    public class QuizData //OX����
    {
        public string mContent, mAnswer, mExplain, mKind;
    }

    public class QuizData2 //4������
    {
        public string fContent, fEx1, fEx2, fEx3, fEx4, fCorrect, fKind;
    }

    public class QuizData3 //IT����
    {
        public string itContent, itEx1, itEx2, itEx3, itEx4, itCorrect, itKind;
    }

    public bool ox; // ���� ������ �¾Ҵ��� ����
    public float mRemainCompetitionTime = 600.0f; // ���� ���۱��� �ð�
    public float mAnswerTimeOut = 5.0f; // ���� �ð�
    public float mNextAnswerDelayTimeOut = 3.0f; // ���� ���� �ð�
    public bool gameStarted = false; // ���� ���� ����
    public int mCurrentQuizIndex = 0; // ���� ���� �ε���
    public bool cleanFloor = true;

    public List<QuizData> mQuizList = new List<QuizData>();
    public List<QuizData2> mQuizList2 = new List<QuizData2>();
    public List<QuizData3> mQuizList3 = new List<QuizData3>();

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
        if (getQuizType() == "quiz")
        {
            return "OX����";
        }
        else if (getQuizType() == "quiz2")
        {
            return "4�������� �⺻���";
        }
        else if (getQuizType() == "quiz3")
        {
            return "IT����";
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

    public string getQuizType() //���� ���� : ox
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

    public string getQuizAnswerOX() //ox ����
    {
        return mQuizList[mCurrentQuizIndex].mAnswer;
    }

    public string getQuizAnswerFour() //4�������� ����
    {
        return mQuizList2[mCurrentQuizIndex].fCorrect;
    }

    public string getQuizAnswerIT() //it ����
    {
        return mQuizList3[mCurrentQuizIndex].itCorrect;
    }

    public string getQuizExplainOX() //ox ����
    {
        return mQuizList[mCurrentQuizIndex].mExplain;
    }

    public float getAnswerTimeOut()
    {
        return mAnswerTimeOut;
    }

    public bool checkQuizAnswerOX(string answer) //ox���� ���� Ȯ��
    {
        return getQuizAnswerOX() == answer;
    }

    public bool checkQuizAnswerFour(string answer) //4�������� ���� Ȯ��
    {
        return getQuizAnswerFour() == answer;
    }

    public bool checkQuizAnswerIT(string answer) //it���� ���� Ȯ��
    {
        return getQuizAnswerIT() == answer;
    }

    public string getQuizContentOX() //ox���� ����
    {
        return mQuizList[mCurrentQuizIndex].mContent;
    }

    public string getQuizContentFour() //4�������� ����
    {
        return mQuizList2[mCurrentQuizIndex].fContent;
    }

    public string getQuizContentIT()
    {
        return mQuizList3[mCurrentQuizIndex].itContent;
    }

    public string getQuizEx1Four() //4�������� ����1��
    {
        return mQuizList2[mCurrentQuizIndex].fEx1;
    }
    public string getQuizEx2Four() //4�������� ����2��
    {
        return mQuizList2[mCurrentQuizIndex].fEx2;
    }
    public string getQuizEx3Four() //4�������� ����3��
    {
        return mQuizList2[mCurrentQuizIndex].fEx3;
    }
    public string getQuizEx4Four() //4�������� ����4��
    {
        return mQuizList2[mCurrentQuizIndex].fEx4;
    }

    public string getQuizEx1IT() //it���� ����1��
    {
        return mQuizList3[mCurrentQuizIndex].itEx1;
    }

    public string getQuizEx2IT() //it���� ����2��
    {
        return mQuizList3[mCurrentQuizIndex].itEx2;
    }

    public string getQuizEx3IT() //it���� ����3��
    {
        return mQuizList3[mCurrentQuizIndex].itEx3;
    }

    public string getQuizEx4IT() //it���� ����4��
    {
        return mQuizList3[mCurrentQuizIndex].itEx4;
    }

    public bool isCompetitionPlay() //��ȸ����(��/��)
    {
        return gameStarted;
    }

    public void setCompetitionPlayOX() //ox ��ȸ����(��/��) 
    {
        gameStarted = true;
        nextAnswerOX();
    }

    public void setCompetitionPlayFour() //4�������� ��ȸ����(��/��)
    {
        gameStarted = true;
        nextAnswerFour();
    }

    public void setCompetititonPlayIT() //it���� ��ȸ����(��/��)
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
    //����������
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
        //�ð��� �׻� �帥��.
        mRemainCompetitionTime -= Time.deltaTime;
    }

    //���� ����
    public bool isCompetitionState_Starting()
    {
        //���� �������� :
        //���� ���� ���� > 0 
        if (getQuizType() == "quiz") //OX����
        {
            if (getRemainQuizCountOX() > 0)
            {
                //���������� true�̰� ������۽ð� > 0
                if (!isCompetitionPlay() && mRemainCompetitionTime > 0)
                {
                    return true;
                }
            }
        }
        else if (getQuizType() == "quiz2") //4��������
        {
            if (getRemainQuizCountFour() > 0)
            {
                if (!isCompetitionPlay() && mRemainCompetitionTime > 0)
                {
                    return true;
                }
            }
        }
        else if (getQuizType() == "quiz3") //IT����
        {
            if (getRemainQuizCountIT() > 0)
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
        if (getQuizType() == "quiz") //OX����
        {
            if (getRemainQuizCountOX() > 0)
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
        else if (getQuizType() == "quiz2") //4��������
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
        //���� �������� ������ 0 ���� �̰ų� ���� ������ false�̰� ������۽ð��� 0 ���� �̰ų� ������ ���� ���ð��� 0���� ������ false
        return false;
    }

    //���� Ǯ��
    public bool isCompetitionState_QuizAnswer()
    {
        //���� Ǫ�� ���� :
        //���� ���� ���� > 0 
        if (getQuizType() == "quiz") //OX����
        {
            if (getRemainQuizCountOX() > 0)
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
        else if (getQuizType() == "quiz2") //4��������
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
        else if (getQuizType() == "quiz3") //IT����
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
        //���� �������� ������ 0 ���� �̰ų� ���� ������false�̰� ������۽ð��� 0���� �̰ų� ����Ǫ�½ð��� 0���� �۰ų�
        //���� �����γѾ�� �ð���(���ݼ����� �ð�) 0�� ���ϰ� �Ǹ� false
        return false;
    }

    //���� �������� �Ѿ��
    public bool isCompetitionState_QuizNext()
    {
        //���� �������� �Ѿ�� ���� :
        //���� ���� ���� > 0 
        if (getQuizType() == "quiz") //OX����
        {
            if (getRemainQuizCountOX() > 0)
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
        else if (getQuizType() == "quiz2") //4��������
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
        else if (getQuizType() == "quiz3") //IT����
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
        //���ӳ��� ��� 0 ���� �̰ų� ���������� false�̰� ������۽ð��� 0���� �̰ų� ����Ǫ�½ð��� 0���� �۰ų�
        //���� �����γѾ�� �ð���(���ݼ����� �ð�) 0�� �����̰ų� ������� 0�����ϰ� �Ǹ� false
        return false;
    }
}