using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cube : Item
{
    [SerializeField] private int number;
    public int Number => number;

    public void MoveCube(Transform position)
    {
        transform.position = position.position;
        var random = Random.Range(0, 3);
        transform.eulerAngles = CubePuzzle.Instance.rotations[random];
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
        if (ray.transform.TryGetComponent(out CubeSlot item))
        {
            if (item.CubeInSlot == null) return true;
        }
        return false;
    }
}
