using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyGood : Item
{
    [SerializeField] MeshRenderer[] _meshRenderer;
    [SerializeField] private int part;
    private bool _keyCompleted;
    [SerializeField] private Light keyLight;
    public override void OnGrabItem()
    {
        var inventory = Inventory.Instance.enviromentInventory;
        var haveKey = inventory.Where(x => x != null && itemName == x.itemName);
        keyLight.enabled = false;
        if (haveKey.Any())
        {
            var item = haveKey.First().GetComponent<KeyGood>();
            item.ChangeMesh(part);
            item.CombineKey();
            Destroy(gameObject);
            return;
        }

        ChangeLayer(18);
        Inventory.Instance.AddItem(this,category);
        CheckPart();
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

    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (!hit) return;
        if (!_keyCompleted) return;
        
        if (i.transform.TryGetComponent(out CandlesGoodHandle candle))
        {
            candle.OpenDoor();
            Inventory.Instance.DropItem(this, Inventory.Instance.countSelected);
            Destroy(gameObject);
        }

    }

    public override bool CanInteractWithItem()
    {
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();

        if (!rayConnected) return false;
        if (ObjectDetector.Instance.InteractText()) return true;
        if (!_keyCompleted) return false;
        if (ray.transform.TryGetComponent(out CandlesGoodHandle item)) return true;

        return false;
    }

    public override void OnDropItem()
    {
        gameObject.SetActive(true);
        ChangeLayer(1);
        keyLight.enabled = true;
    }
    private void ChangeLayer(int numberLayer)
    {
        foreach (var mesh in _meshRenderer)
        {
            mesh.gameObject.layer = numberLayer;
        }
    }
    
    public void ChangeMesh(int part)
    {
        transform.GetChild(0).GetChild(part).gameObject.SetActive(true);
    }

    private void CheckPart()
    {
        var i = _keyCompleted ? 2 : part;
        InventoryUI.Instance.GetComponentInChildren<KeyGoodUI>().ChangeUI(i);
    }

    public void CombineKey()
    {
        InventoryUI.Instance.GetComponentInChildren<KeyGoodUI>().ChangeUI(2);
        _keyCompleted = true;
    }

    public void ChangeLight(bool state)
    {
        keyLight.enabled = state;
    }
}
