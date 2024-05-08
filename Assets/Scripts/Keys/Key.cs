using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    public KeyType keyRoom;

    public void OnInteract()
    {
        KeyHandler.Instance.AddKey(this);
    }
}
