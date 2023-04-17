using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject mRocket;

    Vector3 startmove = new Vector3(37, 9.4f, 21);

    float spd = 0.2f;
    void Update()
    {
        if(mRocket != null)
        {
            if (mRocket.transform.position.x > -85) 
            {
                Vector3 currentPosition = mRocket.transform.position;
                currentPosition.x -= spd;

                mRocket.transform.position = currentPosition;
            }
            else if(mRocket.transform.position.x < -85)
            {
                mRocket.transform.position = startmove;
            }
        }
        
    }
}

