using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : Item
{
    public string misionName;

    public Item[] itemsNeededs;

    public override void OnDropItem()
    {
        base.OnDropItem();
        PlayerHandler.Instance.actualMission = null;
    }

    public virtual void OnGrabMission()
    {
    }

    public override void OnInteractItem()
    {
        base.OnInteractItem();
        OnGrabMission();
    }
}
