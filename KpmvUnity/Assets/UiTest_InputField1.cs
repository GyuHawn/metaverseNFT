using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UiTest_InputField1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ipttxtOnSummit2(string n1)
    {
        UnityEngine.UI.InputField if1 = GetComponent<UnityEngine.UI.InputField>();
        Debug.Log(this.name + "ipttxt on summit str:" + n1 + " txt:" + if1.text);
        if1.text = "";
    }

    public void ipttxtOnSummit1()
    {
        UnityEngine.UI.InputField if1 = GetComponent<UnityEngine.UI.InputField>();

        {
            Debug.Log(this.name + "ipttxt on summit txt:" + if1.text);
            var t1 = GameObject.Find("Text1");
            Debug.Log(this.name + " " + t1.name);
            var tmp = t1.GetComponent<TextMeshProUGUI>();
            tmp.SetText(tmp.text+"\n"+if1.text);
        }

        if1.text = "";
    }
}
