using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoxPuzzle : MonoBehaviour
{
    public static MusicBoxPuzzle Instance {get; private set;}
    [SerializeField] private int code;
    [SerializeField] private SheetSlot[] slots;
    private Animator _animator;
    [SerializeField] private GameObject ps;
    [SerializeField] private AnimationClip animation;
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioSource audioSource;
    
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
        string actualCode = "";
        foreach (var s in slots)
        {
            if (s.Sheet == null) break;
            actualCode += s.Sheet.Number;
        }

        if (actualCode == code.ToString()) CorrectCode();
    }
    
    private void CorrectCode()
    {
        _animator.SetBool("Open", true);
        foreach (var s in slots)
        {
            s.Sheet.MoveInMusicBox();
        }

        var duration = music.length;
        var psC = ps.GetComponent<ParticleSystem>().main;
        psC.duration = duration;
        ps.SetActive(true);
        _animator.speed = animation.length / music.length;
        audioSource.Play();
    }
}
