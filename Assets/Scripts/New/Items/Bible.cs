using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bible : Item
{
    private bool _placed;
    public float timeToBurn;
    public GameObject firePS;
    private MaterialPropertyBlock _burning;
    private MeshRenderer _mesh;
    public GameObject paperBible;
    private RaycastHit _hit;
    public LayerMask layer;
    public float distance;
    private bool ray;
    private BibleCD _bibleCD;
    private BibleView _bibleView;
    private bool _cantUse;
    public SkinnedMeshRenderer[] meshes;
    public MeshRenderer[] paperMesh;
    private int index;
    public AudioSource _paper;

    private void Start()
    {
        _bibleCD = PlayerHandler.Instance.GetComponent<BibleCD>();
    }

    public override void OnGrabItem()
    {
        Inventory.Instance.AddItem(this, category);
        transform.localEulerAngles = angleHand;
        foreach (var mesh in meshes)
        {
            mesh.gameObject.layer = 18;
        }
        
        foreach (var mesh in paperMesh)
        {
            mesh.gameObject.layer = 18;
        }
        
        var inventoryHub = Inventory.Instance.hubInventory;
        for (int i = 0; i < inventoryHub.Length; i++)
        {
            if(inventoryHub[i] == null) continue;
            if (inventoryHub[i] == this)
            {
                InventoryUI.Instance.fillGO.transform.GetChild(i).GetComponent<SliderUI>().SubscribeToBibleEvent();
                index = i;
                break;
            }
        }
        
        if (TutorialHub.Instance != null)
        {
            TutorialHub.Instance.countItemGrabbed++;
            TutorialHub.Instance.CheckGrabbedItems();
        }

        if (SceneManager.GetActiveScene().name == "Hub") return;
        
        SortInventoryBuyHandler.Instance.SaveCount(itemName, true);
    }

    public override void OnDropItem()
    {
        gameObject.SetActive(true);
        foreach (var mesh in meshes)
        {
            mesh.gameObject.layer = 1;
        }

        foreach (var mesh in paperMesh)
        {
            mesh.gameObject.layer = 1;
        } 
        InventoryUI.Instance.fillGO.transform.GetChild(index).GetComponent<SliderUI>().UnSubscribeToBibleEvent();
        
        if (TutorialHub.Instance != null)
        {
            TutorialHub.Instance.countItemGrabbed--;
            TutorialHub.Instance.CheckGrabbedItems();
        }
        
        if (SceneManager.GetActiveScene().name == "Hub") return;
        
        SortInventoryBuyHandler.Instance.SaveCount(itemName, false);
    }
    

    private void Awake()
    {
        _burning = new MaterialPropertyBlock();
        _mesh = GetComponentInChildren<MeshRenderer>();
        _mesh.SetPropertyBlock(_burning);
        _burning.SetInt("_BurningON", 0);
        _bibleView = GetComponentInChildren<BibleView>();
    }

    public override void OnInteract(bool hit, RaycastHit i)
    {
        base.OnInteract(hit,i);
        if (_bibleCD.cantUse) return;
        if (_cantUse) return;
        if (ray)
        {
            if (_hit.transform.TryGetComponent(out RitualFloor ritual))
            {
                Inventory.Instance.cantSwitch = true;
                _bibleView.animator.SetTrigger("OpenTrigger");
                StartCoroutine(WaitForAnimRitual());
                return;
            }
            if (_hit.transform.gameObject.layer != 19) return;
            Inventory.Instance.cantSwitch = true;
            _bibleView.animator.SetTrigger("OpenTrigger");
            StartCoroutine(WaitForAnim(_hit.point));
        }
    }
    
    public override bool CanInteractWithItem()
    {
        if (!ray || _bibleCD.Cooldown < 10) return false;
        
        if (_hit.transform.gameObject.layer == 19 || _hit.transform.TryGetComponent(out RitualFloor ritualFloor)) return true;

        return false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        ray = Physics.Raycast(PlayerHandler.Instance.cameraPos.position, PlayerHandler.Instance.cameraPos.forward, out _hit, distance, layer);
        canInteractWithItem = CanInteractWithItem();
        ChangeCrossHair();
        if (Input.GetMouseButtonDown(0)) OnInteract(ray, _hit);
    }

    IEnumerator WaitForAnim(Vector3 hitPoint)
    {
        _cantUse = true;
        _paper.Play();
        yield return new WaitUntil(() => _bibleView.animator.GetCurrentAnimatorStateInfo(0).IsName("CutBook"));
        yield return new WaitUntil(() => !_bibleView.animator.GetCurrentAnimatorStateInfo(0).IsName("CutBook"));

        var paper = Instantiate(paperBible);
        paper.transform.position = transform.position;
        float ticks = 0;
        Vector3 originalPos = paper.transform.position;
        while (ticks < 1)
        {
            ticks += Time.deltaTime;
            paper.transform.position = Vector3.Lerp(originalPos, hitPoint + Vector3.up * 0.01f, ticks);
            yield return null;
        }
        _bibleCD.SetCooldown(0);
        yield return new WaitUntil(() => _bibleView.animator.GetCurrentAnimatorStateInfo(0).IsName("CloseIdle"));
        _cantUse = false;
        Inventory.Instance.cantSwitch = false;
    }
    
    IEnumerator WaitForAnimRitual()
    {
        _cantUse = true;
        yield return new WaitUntil(() => _bibleView.animator.GetCurrentAnimatorStateInfo(0).IsName("CutBook"));
        yield return new WaitUntil(() => !_bibleView.animator.GetCurrentAnimatorStateInfo(0).IsName("CutBook"));
        var paperRitual = Instantiate(paperBible);
        paperRitual.transform.position = transform.position;
        float ticks = 0;
        Vector3 originalPos = paperRitual.transform.position;
        while (ticks < 1)
        {
            ticks += Time.deltaTime;
            paperRitual.transform.position = Vector3.Lerp(originalPos, RitualManager.Instance.ritualFloor.transform.position, ticks);
            yield return null;
        }
        paperRitual.GetComponent<BiblePaper>().paperOnRitual = true;
        _bibleCD.SetCooldown(0);
        yield return new WaitUntil(() => _bibleView.animator.GetCurrentAnimatorStateInfo(0).IsName("CloseIdle"));
        _cantUse = false;
        Inventory.Instance.cantSwitch = false;
    }
}
