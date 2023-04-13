using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;

    private void Start()
    {
    }

    public void SetTarget(GameObject obj)
    {
        target = LcIPT.GetThis().go;
    }
    void LateUpdate()
    {
        if (target)
        {
            transform.position = target.transform.position + offset;
        }
    }
}
