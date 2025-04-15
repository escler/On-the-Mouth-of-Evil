using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Cube : Item
{
    [SerializeField] private int number;
    public int Number => number;

    public void MoveCube(Transform position)
    {
        StartCoroutine(MoveCubeCor(position));
    }

    IEnumerator MoveCubeCor(Transform position)
    {
        float time = 0;
        Vector3 start = transform.position;
        
        while (time < 1)
        {
            transform.position = Vector3.Lerp(start, position.position, time);
            time += Time.deltaTime;
            yield return null;
        }
        transform.rotation = quaternion.identity;
    }
    
    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (!hit) return;
        if (!canInteractWithItem) return;
        
        if (i.transform.TryGetComponent(out CubeSlot cubeSlot))
        {
            Inventory.Instance.DropItem(this, Inventory.Instance.countSelected);
            cubeSlot.PlaceCube(this);
        }
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
        if (ray.transform.TryGetComponent(out CubeSlot item))
        {
            if (item.CubeInSlot == null) return true;
        }
        return false;
    }
}
