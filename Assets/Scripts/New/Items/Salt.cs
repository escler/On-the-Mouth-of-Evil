using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private RaycastHit _hit;

    
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
        
        if (SceneManager.GetActiveScene().name == "Hub") return;
        
        SortInventoryBuyHandler.Instance.SaveCount(itemName, true);

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
        }

        if (i.transform.TryGetComponent(out VoodooDoll vodooDoll))
        {
            if (_uses <= 0) return;
            StartCoroutine(WaitForUseVoodoo(vodooDoll, _hit.point));
        }
    }

    IEnumerator WaitForUseVoodoo(VoodooDoll voodooDoll, Vector3 position)
    {
        Inventory.Instance.cantSwitch = true;
        transform.SetParent(null);
        position += PlayerHandler.Instance.transform.right * -0.25f;
        Vector3 originalPos = transform.position;

        float ticks = 0;
        while (ticks < 1)
        {
            ticks += Time.deltaTime * 2f;
            transform.position = Vector3.Lerp(originalPos, position, ticks);
            yield return null;
        }
        _animator.SetBool("PutSalt", true);
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("PutSalt"));
        _animator.SetBool("PutSalt", false);
        yield return new WaitUntil(() => !_animator.GetCurrentAnimatorStateInfo(0).IsName("PutSalt"));
        _uses--;
        OnSaltChange?.Invoke();
        StartCoroutine(FillSalt());
        voodooDoll.InSalt();

        ticks = 0;
        originalPos = transform.position;
        while (ticks < 1)
        {
            ticks += Time.deltaTime * 2;
            transform.position = Vector3.Lerp(originalPos, PlayerHandler.Instance.handPivot.position, ticks);
            yield return null;
        }
        transform.SetParent(PlayerHandler.Instance.handPivot);
        transform.localEulerAngles = angleHand;
        Inventory.Instance.cantSwitch = false;

    }
    
    public override void OnUpdate()
    {
        base.OnUpdate();
        _hit = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();
        canInteractWithItem = CanInteractWithItem();
        ChangeCrossHair();

        if (Input.GetMouseButtonDown(0)) OnInteract(rayConnected, _hit);
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
        GetComponentInChildren<SkinnedMeshRenderer>().gameObject.layer = 1;
        fill.gameObject.layer = 17;
        if (!SaltPuzzleTable.Instance) return;
        SaltPuzzleTable.Instance.playerInTable = false;
        InventoryUI.Instance.fillGO.transform.GetChild(index).GetComponent<SliderUI>().UnSubscribeToSaltEvent();

        if (SceneManager.GetActiveScene().name == "Hub") return;
        
        SortInventoryBuyHandler.Instance.SaveCount(itemName, false);

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
        float target = currentSalt + 2.1f / (maxUses + 2);
        while (currentSalt < target)
        {
            currentSalt += 0.1f;
            _saltFill.SetFloat("_Fill", currentSalt);
            fill.SetPropertyBlock(_saltFill);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
