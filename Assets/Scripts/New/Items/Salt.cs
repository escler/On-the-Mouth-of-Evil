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
    private bool cantUseItem;
    private SaltView saltView;
    private MaterialPropertyBlock _saltFill;
    public MeshRenderer fill;
    private float currentSalt;
    private void Awake()
    {
        saltView = GetComponentInChildren<SaltView>();
        _animator = GetComponent<Animator>();
        _uses = maxUses;
        _saltFill = new MaterialPropertyBlock();
        fill.SetPropertyBlock(_saltFill);
        currentSalt = 2.5f;
        _saltFill.SetFloat("_Fill",currentSalt);
    }

    public override void OnGrabItem()
    {
        base.OnGrabItem();
        var inventory = Inventory.Instance.hubInventory;
        fill.gameObject.layer = 18;

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
            if (door.saltBlock) return;
            if (cantUseItem) return;
            saltView.animator.SetTrigger("PutSalt");
            Inventory.Instance.cantSwitch = true;
            cantUseItem = true;
            StartCoroutine(WaitForUseAgain(door));
            
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
        print("Ola");
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
        GetComponentInChildren<SkinnedMeshRenderer>().gameObject.layer = 1;
        fill.gameObject.layer = 17;
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
        saltView.animator.SetBool("PuzzleSalt", true);
    }

    public void DisableBool()
    {
        _animator.SetBool("PuzzleSalt", false);
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
    
    IEnumerator WaitForUseAgain(Door door)
    {
        yield return new WaitUntil(() => !saltView.animator.GetCurrentAnimatorStateInfo(0).IsName("SaltCloseIdle"));
        yield return new WaitUntil(() => saltView.animator.GetCurrentAnimatorStateInfo(0).IsName("SaltToss"));
        StartCoroutine(FillSalt());
        var blockDoor = door.BlockDoor();
        if(blockDoor) ChangeUI();
        yield return new WaitUntil(() => saltView.animator.GetCurrentAnimatorStateInfo(0).IsName("SaltCloseIdle"));
        cantUseItem = false;
        Inventory.Instance.cantSwitch = false;
        print("Lo puedo usar devuelta");
    }

    IEnumerator FillSalt()
    {
        float target = currentSalt + 2.1f / 6;
        while (currentSalt < target)
        {
            currentSalt += 0.1f;
            _saltFill.SetFloat("_Fill", currentSalt);
            fill.SetPropertyBlock(_saltFill);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
