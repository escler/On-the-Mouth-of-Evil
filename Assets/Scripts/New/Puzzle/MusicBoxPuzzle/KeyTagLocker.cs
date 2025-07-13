using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTagLocker : MonoBehaviour
{
    [SerializeField] private string tag;
    [SerializeField] private Door door;
    [SerializeField] private AudioSource audioSource;
    public string Tag => tag;
    public void OpenLocker()
    {
        audioSource.Play();
        door._locked = false;
        enabled = false;
    }
}
