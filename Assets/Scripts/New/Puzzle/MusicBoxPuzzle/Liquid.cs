using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : Item
{
    [SerializeField] private MeshRenderer[] meshes;

    public override void OnGrabItem()
    {
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
}
