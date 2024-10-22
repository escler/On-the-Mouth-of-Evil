using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_LookAtPlayer : MonoBehaviour
{
    private Transform player;
    private Vector3 pos;
    private Vector3 reference = Vector3.zero;

    private void Awake()
    {
        player = PlayerHandler.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        pos = player.transform.position;
        pos.y = transform.position.y;
        
        transform.LookAt(Vector3.SmoothDamp(transform.position, pos, ref reference, 45 * Time.deltaTime));
    }
}
