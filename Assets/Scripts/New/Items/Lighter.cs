using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lighter : Item
{
    [SerializeField] private List<Item> _itemsInteractables;
    public GameObject PSIdle;
    private LighterView lighterView;
    private bool cantUseItem;
    private Vector3 reference = Vector3.zero;
    public AudioSource _fire;
    public AudioSource _open;
    public AudioSource _onfire;
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
            _onfire.PlayOneShot(_onfire.clip);
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
        
        if (SceneManager.GetActiveScene().name == "Hub") return;
        
        SortInventoryBuyHandler.Instance.AddItemToHandler(this);
    }

    public override void OnSelectItem()
    {
        base.OnSelectItem();
        lighterView.animator.SetBool("Open", true);
        _open.PlayOneShot(_open.clip);
        _fire.Play();
        //PSIdle.SetActive(true);
    }

    public override void OnDeselectItem()
    {
        _fire.Stop();
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
        
        if (SceneManager.GetActiveScene().name == "Hub") return;
        
        SortInventoryBuyHandler.Instance.RemoveItemFromHandler(this);
    }

    IEnumerator WaitForUseAgain(IBurneable item)
    {
        transform.SetParent(null);
        float ticks = 0;
        Vector3 originalPos = transform.position;
        cantBobbing = true;
        while (ticks < 1)
        {
            ticks += Time.deltaTime * 2;
            transform.position = Vector3.Lerp(originalPos, item.Position, ticks);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        item.OnBurn();
        StartCoroutine(AdjustRot());
        yield return new WaitForSeconds(0.1f);
        originalPos = transform.position;
        ticks = 0;
        while (ticks < 1)
        {
            ticks += Time.deltaTime * 2;
            transform.position = Vector3.Lerp(originalPos, PlayerHandler.Instance.handPivot.position, ticks);
            yield return null;
        }

        transform.position = PlayerHandler.Instance.handPivot.position;

        transform.SetParent(PlayerHandler.Instance.handPivot);
        cantBobbing = false;
        cantUseItem = false;
        Inventory.Instance.cantSwitch = false;
        print("Lo puedo usar devuelta");
    }
    
    IEnumerator AdjustRot()
    {
        var idealEuler = angleHand;
        var qr = Quaternion.Euler(idealEuler);
        float time = 0;
        while (Vector3.Distance(transform.localPosition, idealEuler) > 0.1f)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, qr, Time.deltaTime*5);
            yield return new WaitForSeconds(0.01f);
        }

        transform.localEulerAngles = angleHand;
    }
}
