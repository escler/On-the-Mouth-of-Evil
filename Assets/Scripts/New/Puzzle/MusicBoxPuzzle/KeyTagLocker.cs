using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTagLocker : MonoBehaviour
{
    [SerializeField] private string tag;
    public string Tag => tag;
    public void OpenLocker()
    {
        print("Se Abrio");
        GetComponent<BoxCollider>().enabled = false;
    }
}
