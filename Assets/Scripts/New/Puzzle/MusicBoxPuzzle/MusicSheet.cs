using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSheet : Item
{
    [SerializeField] private int number;
    public int Number => number;

    public override void OnGrabItem()
    {
        base.OnGrabItem();
        GetComponentInChildren<AuraItem>().onHand = true;
    }

    public override void OnDropItem()
    {
        base.OnDropItem();
        GetComponentInChildren<AuraItem>().onHand = false;
    }
    
    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (!hit) return;
        if (!canInteractWithItem) return;
        
        if (i.transform.TryGetComponent(out SheetSlot sheetSlot))
        {
            Inventory.Instance.DropItem(this, Inventory.Instance.countSelected);
            sheetSlot.PlaceSheet(this);
            GetComponentInChildren<AuraItem>().onHand = false;
        }
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();
        canInteractWithItem = CanInteractWithItem();
        ObjectDetector.Instance.uiInteractionText.SetActive(CanInteractWithItem());
        if (Input.GetButtonDown("Interact"))
        {
            OnInteract(rayConnected, ray);
        }
    }
    
    public override bool CanInteractWithItem()
    {
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();

        if (!rayConnected) return false;
        if (ObjectDetector.Instance.InteractText()) return true;
        if (ray.transform.TryGetComponent(out SheetSlot item))
        {
            if (item.Sheet == null) return true;
        }
        return false;
    }

    public void MoveInMusicBox()
    {
        StartCoroutine(MoveCor());
    }

    IEnumerator MoveCor()
    {
        Vector3 initial = transform.position;
        Vector3 final = transform.position - Vector3.forward * 0.2f;
        float timer = 0;
        while (timer < 1)
        {
            transform.position = Vector3.Lerp(initial, final, timer);
            timer += Time.deltaTime * 0.5f;
            yield return null;
        }
    }
}
