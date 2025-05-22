using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongboxPuzzle : MonoBehaviour
{
    public static StrongboxPuzzle Instance { get; private set; }
    [SerializeField] private StrongboxWheel[] wheels = new StrongboxWheel[3];
    [SerializeField] private int correctCode;
    [SerializeField] private StrongboxHandle handle;
    private Animator _animator;
    [SerializeField] private BoxCollider body;
    [SerializeField] private GameObject keyGood;
    [SerializeField] private AudioSource audiosource;
    [SerializeField] private AudioClip lockedSound, openSound;
    [SerializeField] private GameObject goodEffectAura;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _animator = GetComponent<Animator>();
    }

    public void CheckCode()
    {
        var code = "";

        foreach (var w in wheels)
        {
            code += w.number.ToString();
        }

        if (code == correctCode.ToString())
        {
            OpenDoor();
        }
        else _animator.SetTrigger("WrongCode");
    }

    private void OpenDoor()
    {
        goodEffectAura.SetActive(true);
        DisableColliders();
        _animator.SetBool("Open", true);
        keyGood.SetActive(true);
        keyGood.GetComponent<KeyGood>().ChangeLight(true);
    }

    private void DisableColliders()
    {
        body.enabled = false;
        foreach (var w in wheels)
        {
            w.GetComponent<BoxCollider>().enabled = false;
        }

        handle.GetComponent<BoxCollider>().enabled = false;
    }

    public void PlayLockedSound()
    {
        audiosource.PlayOneShot(lockedSound);
    }

    public void PlayOpenSound()
    {
        audiosource.PlayOneShot(openSound);
    }
}
