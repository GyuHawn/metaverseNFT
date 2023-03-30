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
    public DateTime quizDate;
    public bool textOn = false;
 
    void Awake()
    {
        mMainClient = GameObject.FindObjectOfType<MainClient>();
    }
   
    void Update()
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
                TimeSpan differ =  quizDate - quiznow;

                if (differ == TimeSpan.Zero)
                {
                    canvas.gameObject.SetActive(true);
                    mMainClient.mQuizManager.mRemainCompetitionTime = 5.0f;
                    textOn = true;
                }
            }
        }
    }
}
