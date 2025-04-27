using System.Collections;
using UnityEngine;

public class Bear : Item
{
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
        if (ray.transform.TryGetComponent(out OvenInside item)) return true;

        return false;
    }

    public override void OnInteract(bool hit, RaycastHit raycastHit)
    {
        if (!hit) return;

        if (!raycastHit.transform.TryGetComponent(out OvenInside item)) return;
        
        Inventory.Instance.DropItem(Inventory.Instance.selectedItem, Inventory.Instance.countSelected);

        var location = raycastHit.transform.position;
        StartCoroutine(MoveToLocation(location));
    }

    IEnumerator MoveToLocation(Vector3 location)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<BoxCollider>().enabled = false;
        float time = 0;
        Vector3 actualLocation = transform.position;

        var yHeight = transform.position;
        yHeight.y = location.y;
        while (time < 1)
        {
            transform.position = Vector3.Lerp(actualLocation, yHeight, time);
            time += Time.deltaTime * 2;
            yield return null;
        }

        time = 0;
        actualLocation = transform.position;
        while (time < 1)
        {
            transform.position = Vector3.Lerp(actualLocation, location, time);
            time += Time.deltaTime;
            yield return null;
        }

        Oven.Instance.BearInOven = true;
        Oven.Instance.bear = gameObject;
    }
}
