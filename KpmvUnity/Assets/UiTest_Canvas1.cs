using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTest_Canvas1 : MonoBehaviour
{
    public UnityEngine.UI.Button mBtn1, mBtn2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void btnClick1(string n1)
    {
        Debug.Log(this.name + "Btn2 click");

        if(mBtn1) {
            mBtn1.gameObject.SetActive(!mBtn1.gameObject.activeSelf);
        }
    }
}
