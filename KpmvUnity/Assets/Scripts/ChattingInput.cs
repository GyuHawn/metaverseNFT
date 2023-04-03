using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static MainClient;

public class ChattingInput : MonoBehaviour
{
    private MainClient mClient;

    private TexstObj chatObj = new TexstObj();


    private void Awake()
    {
        mClient = GameObject.FindObjectOfType<MainClient>();
    }

    public void inputSummit1()
    {
        UnityEngine.UI.InputField if1 = GetComponent<UnityEngine.UI.InputField>();
        {
            string nameText = MainClient.currentUser.mUserName + " : " + if1.text;
            chatObj.textSend(mClient.mCt, nameText);   
        }
        if1.text = "";
    }
}