using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighter : Item
{
    [SerializeField] private List<Item> _itemsInteractables;
    public GameObject PSIdle;
    private LighterView lighterView;
    private bool cantUseItem;

    private void Awake()
    {
        lighterView = GetComponentInChildren<LighterView>();
    }

    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (!hit) return;
        if (cantUseItem) return;
        
        if (i.transform.TryGetComponent(out IBurneable item))
        {
            cantUseItem = true;
            Inventory.Instance.cantSwitch = true;
            StartCoroutine(WaitForUseAgain());
            lighterView.animator.SetBool("Open", true);
            item.OnBurn();
            canUse = true;
        }
        else
        {
            canUse = false;
        }
        base.OnInteract(hit,i);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();
        canInteractWithItem = CanInteractWithItem();
        ChangeCrossHair();
        if (Input.GetMouseButtonDown(0))
        {
            OnInteract(rayConnected, ray);
        }
    }

    public override bool CanInteractWithItem()
    {
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();

        if (!rayConnected) return false;
        if (ray.transform.TryGetComponent(out IBurneable item)) return true;

        return false;
    }

    public override void OnGrabItem()
    {
        base.OnGrabItem();
        transform.localEulerAngles = angleHand;
    }

    public override void OnSelectItem()
    {
        base.OnSelectItem();
        //PSIdle.SetActive(true);
    }

    public override void OnDropItem()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().gameObject.layer = 1;
        PSIdle.SetActive(false);
        gameObject.SetActive(true);
    }

    IEnumerator WaitForUseAgain()
    {
        yield return new WaitUntil(() => !lighterView.animator.GetCurrentAnimatorStateInfo(0).IsName("CloseIdle"));
        yield return new WaitUntil(() => lighterView.animator.GetCurrentAnimatorStateInfo(0).IsName("CloseIdle"));
        cantUseItem = false;
        Inventory.Instance.cantSwitch = false;
        print("Lo puedo usar devuelta");
    }
}
