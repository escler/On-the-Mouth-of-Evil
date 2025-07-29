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
    [SerializeField] private GameObject goodAura;
    [SerializeField] private BoxCollider handleMusic;
    
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
        else
        {
            WrongCode();
        }
    }

    private void WrongCode()
    {
        var count = 0;

        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].Sheet != null) count++;
        }

        if (count < slots.Length)
        {
            DialogHandler.Instance.ChangeText("It won’t play right without all the sheets in place.");
            return;
        }
        
        DialogHandler.Instance.ChangeText("Each music sheet has a strange little icon… I feel like I’ve seen them somewhere else.");
    }

    private void CorrectCode()
    {
        goodAura.SetActive(true);
        _animator.SetBool("Open", true);
        handleMusic.enabled = false;
        foreach (var s in slots)
        {
            s.Sheet.MoveInMusicBox();
            s.Sheet.GetComponent<BoxCollider>().enabled = false;
            s.gameObject.layer = 1;
        }

        var duration = music.length;
        var psC = ps.GetComponent<ParticleSystem>().main;
        psC.duration = duration;
        ps.SetActive(true);
        _animator.speed = animation.length / music.length;
        audioSource.Play();
    }
}
