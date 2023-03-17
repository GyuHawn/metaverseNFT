using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class TimeDisplay : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    void Update()
    {
        // ���� �ð� ��������
        DateTime currentTime = DateTime.Now;

        // �ð� �ؽ�Ʈ ������Ʈ
        timeText.text = currentTime.ToString("HH:mm:ss");
    }
}
