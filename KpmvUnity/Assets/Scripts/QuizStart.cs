using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuizStart : MonoBehaviour
{
    public Canvas canvas;
    public Canvas canvas2;

    public MainClient mMainClient;

    public TextMeshProUGUI mQuizText;
    public TextMeshProUGUI mTimeText;
    public DateTime quizDate;
    public bool textOn = false;

    void Awake()
    {
        mMainClient = LcIPT.GetThis().mMc;
    }
    public bool mbStartsend;
    private void Update()
    {
        if (!textOn)
        {
            if (MainClient.quizinfo != null)
            {
                string quizStart = "";
                string quizName = "";
                for (int i = 0; i < MainClient.quizinfo.Count; i++)
                {
                    if (MainClient.quizinfo[i].mWinner == "")
                    {
                        quizStart = MainClient.quizinfo[i].mTime;
                        quizName = MainClient.quizinfo[i].mGame;
                    }
                }
                DateTime quizDate = DateTime.Parse(quizStart);
                mQuizText.text = quizName + System.Environment.NewLine + " ���� ���۽ð� : " + quizDate;
                string now = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
                DateTime quiznow = DateTime.Parse(now);
                TimeSpan differ = quizDate - quiznow;

                if (differ == TimeSpan.Zero)
                {
                    if (!mbStartsend) //�ѹ��� ���������� �ӽ÷� ���
                    {
                        mbStartsend = true;
                        using (JcCtUnity1.PkWriter1Nm pkw = new JcCtUnity1.PkWriter1Nm(99))
                        {
                            pkw.wStr1("test");
                            mMainClient.mCt.send(pkw);
                        }
                    }
                }
                else if(differ > TimeSpan.Zero)
                {
                    canvas2.gameObject.SetActive(true);
                    if (differ.TotalMinutes == 10)
                    {
                        mTimeText.text = "10�� ���ҽ��ϴ�!!!";
                    }
                    else if(differ.TotalMinutes == 1)
                    {
                        mTimeText.text = "���� ���� 1���� �Դϴ�.!!!";
                    }
                    else
                    {
                        mTimeText.text = quizName + System.Environment.NewLine + " ������ Quiz : " + mMainClient.mQuizManager.quizType() +
                            System.Environment.NewLine + " Quiz��ȸ �����ð� : " + mMainClient.mQuizManager.leftTime();
                    }
                }
            }
        }
    }
}