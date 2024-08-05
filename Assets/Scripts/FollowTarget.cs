using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    private Transform target;
    private Vector3 reference = Vector3.zero;
    private bool canMoving;

    private void Awake()
    {
        StartCoroutine(DelayForMoving());
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref reference, .1f);
        
        if (!canMoving) return;
        if(Vector3.Distance(transform.position,target.position) <= 1) Destroy(this);
    }

    public void GetTarget(Transform targetReceive)
    {
        target = targetReceive;
    }

    IEnumerator DelayForMoving()
    {
        yield return new WaitForSeconds(.1f);
        canMoving = true;
    }
}
