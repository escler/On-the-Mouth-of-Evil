using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoodooDoll : Item
{
    
    private RaycastHit _hit;
    public float distance;
    private bool ray;
    private bool _cantUse;
    public LayerMask layer;
    private Vector3 reference = Vector3.zero;
    private Vector3 referenceRot = Vector3.zero;
    public float timeActive;
    private float _actualTime;
    public VoodooVFXHandler voodooVFXHandler;

    private void Awake()
    {
        voodooVFXHandler = GetComponentInChildren<VoodooVFXHandler>();
    }

    public override void OnGrabItem()
    {
        Inventory.Instance.AddSpecialItem(this);
        if (GetComponentInChildren<SkinnedMeshRenderer>() == null)
        {
            GetComponentInChildren<MeshRenderer>().gameObject.layer = 18;
        }
        else
        {
            GetComponentInChildren<SkinnedMeshRenderer>().gameObject.layer = 18;
        }
        
        if (SceneManager.GetActiveScene().name == "Hub") return;
        
        SortInventoryBuyHandler.Instance.SaveCount(itemName, true);

        
    }

    public override void OnDropItem()
    {
        gameObject.SetActive(true);
        GetComponentInChildren<MeshRenderer>().gameObject.layer = 1;
        
        if (SceneManager.GetActiveScene().name == "Hub") return;
        
        SortInventoryBuyHandler.Instance.SaveCount(itemName, false);

    }
    
    public override void OnUpdate()
    {
        base.OnUpdate();
        ray = Physics.Raycast(PlayerHandler.Instance.cameraPos.position, PlayerHandler.Instance.cameraPos.forward, out _hit, distance, layer);
        canInteractWithItem = CanInteractWithItem();
        ChangeCrossHair();
        if (Input.GetMouseButtonDown(0)) OnInteract(ray, _hit);
    }
    
    public override void OnInteract(bool hit, RaycastHit i)
    {
        base.OnInteract(hit,i);
        if (_cantUse) return;
        if (ray)
        {
            if (_hit.transform.gameObject.layer != 19) return;
            Inventory.Instance.cantSwitch = true;
            StartCoroutine(WaitForLocation(_hit.point));
        }
    }
    
    public override bool CanInteractWithItem()
    {
        if (!ray) return false;
        if (ObjectDetector.Instance.InteractText()) return true;

        
        if (_hit.transform.gameObject.layer == 19 || _hit.transform.TryGetComponent(out RitualFloor ritualFloor)) return true;

        return false;
    }
    
    IEnumerator WaitForLocation(Vector3 hitPoint)
    {
        _cantUse = true;
        transform.SetParent(null);
        StartCoroutine(AdjustRot());
        var hitPointWithAltitude = hitPoint + Vector3.up * 0.01f;
        float ticks = 0;
        Vector3 originalPos = transform.position;
        while (ticks < 1)
        {
            ticks += Time.deltaTime * 2;
            transform.position = Vector3.Lerp(originalPos, hitPointWithAltitude, ticks);
            yield return null;
        }
        Inventory.Instance.DropItem(this, 4);
        transform.position = hitPointWithAltitude;
        _cantUse = false;
        Inventory.Instance.cantSwitch = false;
    }

    IEnumerator AdjustRot()
    {
        var idealEuler = Vector3.zero;
        idealEuler.x = 0;
        idealEuler.z = 0;
        idealEuler.y = transform.eulerAngles.y;
        var qr = Quaternion.Euler(idealEuler);
        float time = 0;
        while (Vector3.Distance(transform.eulerAngles, idealEuler) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, qr, Time.deltaTime*5);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void InSalt()
    {
        StartCoroutine(SaltPlace());
    }

    IEnumerator SaltPlace()
    {
        voodooVFXHandler.OpenPrison();
        if (HouseEnemy.Instance != null)
        {
            HouseEnemy.Instance.voodooPosition = transform.position;
            HouseEnemy.Instance.voodooActivate = true;
        }
        _actualTime = timeActive;
        var rb = GetComponent<Rigidbody>();
        var collider = GetComponent<BoxCollider>();
        rb.isKinematic = true;
        collider.enabled = false;
        while (_actualTime > 0)
        {
            _actualTime -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        if (HouseEnemy.Instance != null) HouseEnemy.Instance.voodooActivate = false;
        yield return new WaitForSeconds(2f);
        voodooVFXHandler.ClosePrison();
        collider.enabled = true;
        rb.isKinematic = false;
    }
}
