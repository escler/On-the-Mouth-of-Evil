using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDuplicationMovement : MonoBehaviour
{
    public float speedRun;
    public bool run;
    private Transform _characterPos;

    void Awake()
    {
        run = false;
        _characterPos = Player.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (run)
        {
            transform.position += transform.forward * (speedRun * Time.deltaTime);
            transform.LookAt(new Vector3(_characterPos.position.x, transform.position.y, _characterPos.position.z));
        }
    }

    private void OnDisable()
    {
        run = false;
    }

    public void ChangeRun(bool value)
    {
        run = value;
    }
}
