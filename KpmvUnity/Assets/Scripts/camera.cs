using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private void Start()
    {
    }

    public void SetTarget(GameObject obj)
    {
        target = obj.transform;
    }
    void LateUpdate()
    {
        if (target)
        {
            transform.position = target.position + offset;
        }
    }
}
