using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI nftText;

    public int randomIndex;

    bool textOn = false;

    void Update()
    {
        if (!textOn)
        {
            if (MainClient.pdbList != null)
            {

                randomIndex = Random.Range(0, MainClient.pdbList.Count);
                nftText.text = "¿Ã∏ß : " + MainClient.pdbList[randomIndex].mName + " / NFT : " + MainClient.pdbList[randomIndex].mNftAddr;
                textOn = true;
            }
        }

    }
}
