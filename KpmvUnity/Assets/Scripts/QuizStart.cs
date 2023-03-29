using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizStart : MonoBehaviour
{
    public Canvas canvas;
    public MainClient mMainClient;

    private bool isColliding = false;

    void Awake()
    {
        mMainClient = GameObject.FindObjectOfType<MainClient>();
    }

    private void Update()
    {
        if (isColliding && Input.GetKeyDown(KeyCode.F))
        {
            canvas.gameObject.SetActive(true);
            mMainClient.mQuizManager.mRemainCompetitionTime = 5.0f;
        }

        DateTime currentTime = DateTime.Now;
        if (currentTime.Second == 0)
        {
            canvas.gameObject.SetActive(true);
        }
    }

    public void EndQuiz()
    {
        canvas.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isColliding = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isColliding = false;
        }
    }
}
