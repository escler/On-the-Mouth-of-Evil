using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salt : Item
{
    private Animator _animator;
    public ParticleSystem ps;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnInteract(bool hit, RaycastHit i)
    {
        base.OnInteract(hit, i);
        if (!hit) return;

        if (i.transform.TryGetComponent(out Door door))
        {
            door.BlockDoor();
            //Inventory.Instance.DropItem();
            //Destroy(gameObject);
        }
    }

    public override void OnSelectItem()
    {
        if (!SaltPuzzleTable.Instance) return;
        SaltPuzzleTable.Instance.playerInTable = true;
    }

    public override void OnDeselectItem()
    {
        if (!SaltPuzzleTable.Instance) return;
        SaltPuzzleTable.Instance.playerInTable = false;
    }

    public override void OnDropItem()
    {
        gameObject.SetActive(true);
        GetComponentInChildren<MeshRenderer>().gameObject.layer = 1;
        if (!SaltPuzzleTable.Instance) return;
        SaltPuzzleTable.Instance.playerInTable = false;
    }

    public void PlacingBool()
    {
        _animator.SetBool("PutSalt", true);
    }

    public void DisableBool()
    {
        _animator.SetBool("PutSalt", false);
    }

    public void ParticlePlay()
    {
        ps.Play();
    }

    public void ParticlePause()
    {
        ps.Stop();
    }

    public void ParticleStop()
    {
        ps.Stop();
    }
}
