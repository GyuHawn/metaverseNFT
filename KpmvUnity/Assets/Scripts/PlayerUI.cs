using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI nftText;

    bool textOn = false;

    void Update()
    {
        if (!textOn)
        {
            if (MainClient.currentUser != null)
            {
                nftText.text = "¿Ã∏ß : " + MainClient.currentUser.mUserName + " / NFT : " + MainClient.currentUser.mEOA;
                textOn = true;
            }
        }

    }
}
