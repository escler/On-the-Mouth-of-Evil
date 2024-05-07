using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Key", menuName = "Keys/New Key")]
public class Key : MonoBehaviour, IInteractable
{
    public KeyType keyRoom;

    public void OnInteract()
    {
        KeyHandler.Instance.AddKey(this);
    }
}
