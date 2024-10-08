using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    public KeyType keyRoom;
    public string roomName;

    public void OnInteractItem()
    {
        KeyHandler.Instance.AddKey(keyRoom, roomName);
        Destroy(gameObject);
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
        
    }

    public void OnInteractWithObject()
    {
        
    }

    public string ShowText()
    {
        return "";
    }

    public bool CanShowText()
    {
        return false;
    }
}
