using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerName : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;
    public Transform playerTransform;
    public RectTransform canvasRectTransform;
    public TextMeshProUGUI playerText;
    
    void Start()
    {
        playerNameText.text = MainClient.currentUser.mUserName;
    }

    void LateUpdate()
    {
        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(playerTransform.position);
        Vector2 canvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, playerScreenPos, null, out canvasPos);
        playerNameText.rectTransform.localPosition = new Vector3(canvasPos.x, canvasPos.y + 65f);
    }
}