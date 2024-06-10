using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private bool _death;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
            TypeManager.Instance.onResult += CheckResult;
    }

    private void CheckResult()
    {
        _death = TypeManager.Instance.ResultOfType();

        if (_death) gameObject.SetActive(false);
        else TypeManager.Instance.onResult -= CheckResult;
    }
}
