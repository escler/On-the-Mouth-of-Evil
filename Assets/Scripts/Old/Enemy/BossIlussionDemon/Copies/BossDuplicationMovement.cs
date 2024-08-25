using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossDuplicationMovement : MonoBehaviour
{
    public float speedRun;
    public bool run;
    private Transform _characterPos;
    private Animator _animator;

    void OnEnable()
    {
        run = false;
        _characterPos = Player.Instance.transform;
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (run)
        {
            transform.position += transform.forward * (speedRun * Time.deltaTime);
            transform.LookAt(new Vector3(_characterPos.position.x, transform.position.y, _characterPos.position.z));
        }

        _animator.SetBool("Run", run);
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
