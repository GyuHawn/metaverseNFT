using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static MultiClient;
using UnityEngine.UI;

public class ChattingInput : MonoBehaviour
{
    private MultiClient mClient;
    private MainClient maClient;

    private TexstObj chatObj = new TexstObj();
    private MainClient.TexstObj machatObj = new MainClient.TexstObj();


    private void Awake()
    {
        mClient = LcIPT.GetThis().mMulti;
        maClient = LcIPT.GetThis().mMc;
    }

    public void inputSummit1()
    {
        UnityEngine.UI.InputField if1 = GetComponent<UnityEngine.UI.InputField>();
        {
            string nameText = MainClient.currentUser.mUserName + " : " + if1.text;
            if (LcIPT.GetThis().isOnline())
            {
                chatObj.textSend(mClient.mCt, nameText);
            }
            else
            {
                machatObj.textSend(maClient.mCt, nameText);
            }  
        }
        if1.text = "";
    }

    void Update()
    {
        //Check if the Input Field is in focus and able to alter
        if (GetComponent<InputField>().isFocused)
        {
            //Change the Color of the Input Field's Image to green
            GetComponent<Image>().color = Color.green;
        }
        else
        {
            GetComponent<Image>().color = Color.white;
        }
    }
}