using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winner : MonoBehaviour
{
    public GameObject mTrophy;
    public PlayerMotion mWinner;

    public LcIPT lcipt;


    private void Awake()
    {
        lcipt = LcIPT.GetThis();
    }

    void Update()
    {
        if (mWinner)
        {
            mTrophy.transform.position = new Vector3(mWinner.transform.position.x, mWinner.transform.position.y + 2.5f, mWinner.transform.position.z);
        }
    }

}
