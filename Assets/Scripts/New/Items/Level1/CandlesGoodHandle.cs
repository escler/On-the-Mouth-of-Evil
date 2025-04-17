using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandlesGoodHandle : MonoBehaviour
{
    [SerializeField] private BoxCollider box;
    public void OpenDoor()
    {
        box.enabled = false;
        gameObject.SetActive(false);
    }
}
