using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static MainClient;
using TMPro;

public class Login : MonoBehaviour
{
    public InputField useridField;
    public InputField pwdField;
    public TMP_InputField serverField;

    public GameObject error;

    private string enteredId;
    private string enteredPwd;

    private MainClient mClient;

    private PlayerObj loginObj = new PlayerObj();
    private void Awake()
    {
        mClient = GameObject.FindObjectOfType<MainClient>();
        serverField.text = "127.0.0.1";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (useridField.isFocused)
            {
                pwdField.Select();
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (pwdField.isFocused)
            {
                GetLoginInfo();
            }
        }
    }
    

    public void GetLoginInfo()
    {
        serverAddress = serverField.text;
        mClient.mCt.connect(serverAddress, 7777);
        Debug.Log("Client Start 1111");

        enteredId = useridField.text;
        enteredPwd = pwdField.text;

        StartCoroutine(validate());

        IEnumerator validate()
        {          
            yield return new WaitForSeconds(1.5f);
            ValidateLogin();
        }
        useridField.text = "";
        pwdField.text = "";
        
    }

    public void ValidateLogin()
    {
        bool loginSuccess = false;
        string quizName = "";
        string quizKind = "";
        Debug.Log("MainClient.quizinfo.Count : " + MainClient.quizinfo.Count);
        for (int i = 0; i < MainClient.quizinfo.Count; i++)
        {
            if (MainClient.quizinfo[i].mWinner == "")
            {
                quizName = MainClient.quizinfo[i].mGame;
                quizKind = MainClient.quizinfo[i].mQuiz;
            }
        }
        foreach (var user in pdbList)
        {
            if (enteredId == user.mUserid && enteredPwd == user.mPassword)
            {
                if(quizKind == "quiz")
                {
                    SceneManager.LoadScene("Scenes/Quiz1");
                }
                else if(quizKind == "quiz2")
                {
                    SceneManager.LoadScene("Scenes/Quiz2");
                }else if(quizKind == "quiz3")
                {
                    SceneManager.LoadScene("Scenes/Quiz3");
                }
                else
                {
                    SceneManager.LoadScene("Scenes/Quiz1");
                }
                loginObj.loginSend(mClient.mCt, enteredId, enteredPwd);
                MainClient.currentUser = user;
                MainClient.currentUser.mGame = quizName;
                loginSuccess = true;
                break;
            }
        }
        if (!loginSuccess)
        {
            error.SetActive(true);
        }
    }


    public void Error()
    {
        error.SetActive(false);
    }
}
