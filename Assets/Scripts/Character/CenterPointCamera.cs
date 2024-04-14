using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPointCamera : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10f));
    }
}
