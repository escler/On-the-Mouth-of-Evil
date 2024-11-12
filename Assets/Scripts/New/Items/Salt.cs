using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salt : Item
{
    private Animator _animator;
    public ParticleSystem ps;
    public float maxUses;
    public float _uses;
    private bool cantUseItem;
    private SaltView saltView;
    private MaterialPropertyBlock _saltFill;
    public MeshRenderer fill;
    private float currentSalt;
    private int index;
    public delegate void UpdateSaltUI();
    public event UpdateSaltUI OnSaltChange;

    
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
        fill.gameObject.layer = 18;

        var inventoryHub = Inventory.Instance.hubInventory;
        for (int i = 0; i < inventoryHub.Length; i++)
        {
            if(inventoryHub[i] == null) continue;
            if (inventoryHub[i] == this)
            {
                InventoryUI.Instance.fillGO.transform.GetChild(i).GetComponent<SliderUI>().SubscribeToSaltEvent(this);
                index = i;
                break;
            }
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

        if (i.transform.TryGetComponent(out VoodooDoll vodooDoll))
        {
            if (_uses <= 0) return;
            vodooDoll.InSalt();
            _uses--;
            OnSaltChange?.Invoke();
            StartCoroutine(FillSalt());
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
        InventoryUI.Instance.fillGO.transform.GetChild(index).GetComponent<SliderUI>().UnSubscribeToSaltEvent();

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
        if (ray.transform.TryGetComponent(out SaltRecipient saltRecipient) || ray.transform.TryGetComponent(out Door door2) || ray.transform.TryGetComponent(out VoodooDoll vodoo))
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
        if (blockDoor)
        {
            _uses--;
            OnSaltChange?.Invoke();
            print(_uses);
        }
        yield return new WaitUntil(() => saltView.animator.GetCurrentAnimatorStateInfo(0).IsName("SaltCloseIdle"));
        cantUseItem = false;
        Inventory.Instance.cantSwitch = false;
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
