using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimPlayer : MonoBehaviour
{
    private Animator _animator;
    private RigBuilder _rig;
    [SerializeField] private Controller _controller;
    private Rigidbody _rb;
    private bool _aiming, _shotgun, _running, _shooting, _banish;
    public bool throwObject, skillBook;
    public GameObject bookHips, bookHand;
    float speed;

    public bool Shooting
    {
        get { return _shooting; }
        set { _shooting = value; }
    }
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rig = GetComponent<RigBuilder>();

        _rb = GetComponentInParent<Rigidbody>();
    }

    private void Start()
    {
        TypeManager.Instance.OnStartBanish += BanishStart;
        TypeManager.Instance.OnFinishBanish += BanishEnd;
        _animator.SetFloat("Speed",1);
    }

    private void Update()
    {
        _aiming = Input.GetMouseButton(1) || _animator.GetBool("Shooting");
        _running = Input.GetButton("Run") && !_aiming && _rb.velocity != Vector3.zero;
        _animator.SetFloat("AxisX",_controller.GetMovementInput().x);
        _animator.SetFloat("AxisY",_controller.GetMovementInput().z);
        _animator.SetBool("Walking",_rb.velocity.magnitude >= 0.1f);
        _animator.SetBool("Aiming",_aiming);
        _animator.SetBool("Running", _running);
        _animator.SetBool("Shoot", _shooting);
        _animator.SetBool("Banish", _banish);
        _animator.SetBool("ThrowObject", throwObject);
        _animator.SetBool("BookSkill", skillBook);

        EnableRig();
    }

    private void BanishStart()
    {
        _banish = true;
    }

    private void BanishEnd()
    {
        _banish = false;
    }

    public void TakeBook()
    {
        bookHips.SetActive(false);
        bookHand.SetActive(true);
    }

    public void PlaceBook()
    {
        bookHips.SetActive(true);
        bookHand.SetActive(false);
    }

    public void OpenBook()
    {
        bookHand.GetComponent<Animator>().SetBool("Open", true);
    }

    public void CloseBook()
    {
        bookHand.GetComponent<Animator>().SetBool("Open", false);
    }

    public void ThrowItem()
    {
        GetComponentInParent<BossSkill>().ThrowItem();
        throwObject = false;
    }
    
    private void EnableRig()
    {
        if (_aiming || _shooting)
        {
            if (!_shotgun)
            {
                _rig.layers[0].active = true;
                _rig.layers[1].active = false;
                _rig.layers[2].active = false;
            }
            else
            {
                _rig.layers[0].active = false;
                _rig.layers[1].active = false;
                _rig.layers[2].active = true;
            }

        }
        else
        {
            _rig.layers[0].active = false;
            _rig.layers[1].active = true;
            _rig.layers[2].active = false;
        }
    }

    public void ChangeLayerHeight(int value)
    {
        switch (value)
        {
            case 0:
                _animator.SetLayerWeight(1,1);
                _animator.SetLayerWeight(2,0);
                _shotgun = false;
                break;
            case 1:
                _animator.SetLayerWeight(1,0);
                _animator.SetLayerWeight(2,1);
                _shotgun = true;
                break;
        }
        _rig.SyncLayers();
    }

    public void SetSpeedValue(float value)
    {
        _animator.SetFloat("Speed",value);
    }

    public void SKillBookActivate()
    {
        Player.Instance.bookSkill.StartExplosion();
        Player.Instance.bookSkill.ps.Play();
        
        skillBook = false;
    }
}
