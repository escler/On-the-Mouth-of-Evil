using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salt : Item
{
    private Animator _animator;
    public ParticleSystem ps;
    private SaltUI saltUI;
    public int maxUses;
    private int _uses;
    

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _uses = maxUses;
    }

    public override void OnGrabItem()
    {
        base.OnGrabItem();
        transform.localEulerAngles = angleHand;
        var inventory = Inventory.Instance.hubInventory;

        for (int i = 0; i < inventory.Length; i++)
        {
            if(inventory[i] == null) continue;
            if (inventory[i] != this) continue;
            saltUI = InventoryUI.Instance.hubInventoryUI.transform.GetChild(i).transform.GetChild(0).GetComponent<SaltUI>();
            saltUI.SetUses(_uses, maxUses);
            break;
        }
    }

    public override void OnInteract(bool hit, RaycastHit i)
    {
        base.OnInteract(hit, i);
        if (!hit) return;

        if (i.transform.TryGetComponent(out Door door))
        {
            if (_uses <= 0) return;
            var  blockingDoor = door.BlockDoor();
            if(blockingDoor) ChangeUI();
            //Inventory.Instance.DropItem();
            //Destroy(gameObject);
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();
        canInteractWithItem = CanInteractWithItem();
        ChangeCrossHair();

        if (Input.GetMouseButtonDown(0)) OnInteract(rayConnected, ray);
    }

    private void ChangeUI()
    {
        _uses--;
        _uses = Mathf.Clamp(_uses, 0, maxUses);
        saltUI.SaltUsed(_uses);
    }
    public override void OnSelectItem()
    {
        base.OnSelectItem();
        if (!SaltPuzzleTable.Instance) return;
        SaltPuzzleTable.Instance.playerInTable = true;
    }

    public override void OnDeselectItem()
    {
        base.OnDeselectItem();
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

    public override bool CanInteractWithItem()
    {
        if (_uses <= 0) return false;
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();

        if (!rayConnected) return false;
        if (ray.transform.TryGetComponent(out Door door))
        {
            if (door.saltBlock) return false;
        }
        if (ray.transform.TryGetComponent(out SaltRecipient saltRecipient) || ray.transform.TryGetComponent(out Door door2))
        {
            return true;
        }

        return false;
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
