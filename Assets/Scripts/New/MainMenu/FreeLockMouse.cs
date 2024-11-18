using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeLockMouse : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
