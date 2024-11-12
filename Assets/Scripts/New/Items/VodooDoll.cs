using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class VodooDoll : Item
{
    
    private RaycastHit _hit;
    public float distance;
    private bool ray;
    private bool _cantUse;
    public LayerMask layer;
    private Vector3 reference = Vector3.zero;

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
    }

    public override void OnDropItem()
    {
        print("Me llame");
        gameObject.SetActive(true);
        GetComponentInChildren<MeshRenderer>().gameObject.layer = 1;
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
        
        if (_hit.transform.gameObject.layer == 19 || _hit.transform.TryGetComponent(out RitualFloor ritualFloor)) return true;

        return false;
    }
    
    IEnumerator WaitForLocation(Vector3 hitPoint)
    {
        _cantUse = true;
        transform.SetParent(null);
        var hitPointWithAltitude = hitPoint + Vector3.up * 0.01f;
        while (Vector3.Distance(transform.position, hitPointWithAltitude) > 0.05f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, hitPointWithAltitude, ref reference, .25f);
            yield return new WaitForSeconds(0.01f);
        }
        Inventory.Instance.DropItem(this, 4);
        transform.position = hitPointWithAltitude;
        _cantUse = false;
        Inventory.Instance.cantSwitch = false;
    }
}
