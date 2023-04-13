using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winner : MonoBehaviour
{
    public GameObject winner;

    //public Transform target;
    public LcIPT lcipt;
    public bool isFollowing;

    private void Awake()
    {
        lcipt = LcIPT.GetThis();
    }

    void Update()
    {
        if (isFollowing)
        {
            winner.transform.position = new Vector3(lcipt.go.transform.position.x, lcipt.go.transform.position.y + 2.5f, lcipt.go.transform.position.z);
        }
    }

}
