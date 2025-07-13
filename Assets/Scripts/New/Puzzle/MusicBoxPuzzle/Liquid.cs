using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : Item
{
    [SerializeField] private MeshRenderer[] meshes;
    [SerializeReference] Animator animator;
    private bool _textShowed;

    public override void OnGrabItem()
    {
        if (!_textShowed)
        {
            DialogHandler.Instance.ChangeText("They spread like a disease... maybe fire is the cure.");
            _textShowed = true;
        }
        Inventory.Instance.AddItem(this, category);
        transform.localEulerAngles = angleHand;
        foreach (var mesh in meshes)
        {
            mesh.gameObject.layer = 18;
        }
    }

    public override void OnDropItem()
    {
        gameObject.SetActive(true);
        foreach (var mesh in meshes)
        {
            mesh.gameObject.layer = 1;
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


    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (!hit) return;

        if (i.transform.TryGetComponent(out GoodRitual ritual))
        {
            if (!ritual.leverActivated)
            {
                DialogHandler.Instance.ChangeText("I need to get the furnaces working first.");
                return;
            }
            ritual.StartRitual();
            Inventory.Instance.DropItem(this, Inventory.Instance.countSelected);
            transform.position = NestHandler.Instance.LiquidPos.transform.position;
            transform.rotation = NestHandler.Instance.LiquidPos.transform.rotation;
            animator.SetTrigger("ThrowLiquid");
            GetComponent<Rigidbody>().isKinematic = true;
        }

    }
    
    public override bool CanInteractWithItem()
    {
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();

        if (!rayConnected) return false;
        
        if (ObjectDetector.Instance.InteractText()) return true;
        if (ray.transform.TryGetComponent(out GoodRitual ritual)) return true;
        return false;
    }


}
