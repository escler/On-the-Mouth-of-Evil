using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTagLocker : MonoBehaviour
{
    public void OpenLocker()
    {
        print("Se Abrio");
        GetComponent<BoxCollider>().enabled = false;
    }
}
