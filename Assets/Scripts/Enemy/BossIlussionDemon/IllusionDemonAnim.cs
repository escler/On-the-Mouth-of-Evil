using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemonAnim : MonoBehaviour
{
    private Animator _animator;
    public float yAxis;
    private IllusionDemon _demon;

    public Animator Animator => _animator;

    public bool moving, death, hit, run, comboHit, jumpAttack, cast, castCopies, castFight;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _demon = GetComponentInParent<IllusionDemon>();
    }

    void Update()
    {
        _animator.SetBool("Moving", moving);
        _animator.SetFloat("yAxis", yAxis);
        _animator.SetBool("Death",death);
        _animator.SetBool("Hit",hit);
        _animator.SetBool("Run", run);
        _animator.SetBool("ComboHit", comboHit);
        _animator.SetBool("JumpAttack", jumpAttack);
        _animator.SetBool("Cast", cast);
        _animator.SetBool("CastCopy", castCopies);
        _animator.SetBool("CastFight", castFight);
    }
    
    public void AdjustPosition()
    {
        var modelPos = _demon._model.transform.position;
        _demon.transform.position = new Vector3(modelPos.x, _demon.transform.position.y, modelPos.z);
        _demon._model.transform.localPosition = Vector3.zero;
    }

    public void EnableHitBox()
    {
        _demon.spawnHitbox.SetActive(true);
    }

    public void DisableHitBox()
    {
        _demon.spawnHitbox.SetActive(false);
    }

    public void SpawnDemon()
    {
        _demon.InvokeDemon();
    }

    public void SpawnCopies()
    {
        _demon.InvokeCopies();
    }

    public void FinishCast()
    {
        _demon.finishCast = true;
    }

    public void FinishComb()
    {
        comboHit = false;
    }

    public void CastFightDuplications()
    {
        _demon.InvokeFightCopies();
    }
}
