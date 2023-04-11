using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuizStart : MonoBehaviour
{
    public Canvas canvas;
    public MainClient mMainClient;

    public TextMeshProUGUI mQuizText;
    public TextMeshProUGUI mTimeText;
    public DateTime quizDate;
    public bool textOn = false;

    void Awake()
    {
        mMainClient = GameObject.FindObjectOfType<MainClient>();
    }
    
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
                mQuizText.text = quizName + System.Environment.NewLine + " 퀴즈 시작시간 : " + quizDate;
                string now = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");
                DateTime quiznow = DateTime.Parse(now);
                TimeSpan differ = quizDate - quiznow;

                if (differ == TimeSpan.Zero)
                {
                    canvas.gameObject.SetActive(true);
                    mMainClient.mQuizManager.mRemainCompetitionTime = 5.0f;
                    textOn = true;
                }
                else if(differ > TimeSpan.Zero)
                {
                    canvas.gameObject.SetActive(true);
                    if(differ.TotalMinutes == 10)
                    {
                        mTimeText.text = "10분 남았습니다!!!";
                    }
                    else if(differ.TotalMinutes == 1)
                    {
                        mTimeText.text = "게임 시작 1분전 입니다.!!!";
                    }
                    else
                    {
                        mTimeText.text = quizName + System.Environment.NewLine + " 선택한 Quiz : " + mMainClient.mQuizManager.quizType() +System.Environment.NewLine + " Quiz대회 남은시간 : " + mMainClient.mQuizManager.leftTime();
                    }
                }
            }
        }
    }
}