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
    private Vector3 reference = Vector3.zero;

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
            StartCoroutine(WaitForUseAgain(item));
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
        lighterView.animator.SetBool("Open", true);
    }

    public override void OnSelectItem()
    {
        base.OnSelectItem();
        lighterView.animator.SetBool("Open", true);
        //PSIdle.SetActive(true);
    }

    public override void OnDeselectItem()
    {
        base.OnDeselectItem();
        PSIdle.SetActive(false);
        lighterView.animator.SetBool("Open", false);
    }

    public override void OnDropItem()
    {
        GetComponentInChildren<SkinnedMeshRenderer>().gameObject.layer = 1;
        PSIdle.SetActive(false);
        gameObject.SetActive(true);
        lighterView.animator.SetBool("Open", false);
    }

    IEnumerator WaitForUseAgain(IBurneable item)
    {
        transform.SetParent(null);
        cantBobbing = true;
        while (Vector3.Distance(transform.position, item.Position) > 0.2f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, item.Position, ref reference, .25f);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.1f);
        item.OnBurn();
        yield return new WaitForSeconds(0.1f);
        while (Vector3.Distance(transform.position, PlayerHandler.Instance.handPivot.position) > 0.01f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, PlayerHandler.Instance.handPivot.position, ref reference, .25f);
            yield return new WaitForSeconds(0.01f);
        }

        transform.position = PlayerHandler.Instance.handPivot.position;

        transform.SetParent(PlayerHandler.Instance.handPivot);
        transform.localEulerAngles = angleHand;
        cantBobbing = false;
        cantUseItem = false;
        Inventory.Instance.cantSwitch = false;
        print("Lo puedo usar devuelta");
    }
}
