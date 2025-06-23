using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPuzzle : MonoBehaviour
{
    public static BodyPuzzle Instance { get; private set; }
    private Animator _animator;
    [SerializeField] private GameObject badAura;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private GameObject itemGained;
    [SerializeField] private AudioSource knifeCut;
    public bool bloodDrained;

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

    public void OpenBody()
    {
        knifeCut.Play();
        badAura.SetActive(true);
        _animator.SetBool("Open", true);
        enabled = false;
        boxCollider.enabled = false;
    }

    public void GiveItem()
    {
        var item = Instantiate(itemGained);
        item.GetComponent<Item>().OnGrabItem();
    }
}
