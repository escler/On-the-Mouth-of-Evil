using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandlesBadHandle : MonoBehaviour
{
    [SerializeField] private BoxCollider box; 
    public void OpenDoor()
    {
        box.enabled = false;
        gameObject.SetActive(false);
    }
}
