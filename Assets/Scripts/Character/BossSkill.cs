using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossSkill : MonoBehaviour
{
    public KeyCode keyAssign = KeyCode.F;
    private ThrowItem _item;
    public Transform pivotToObject;
    private bool _itemPicked;

    private void Update()
    {
        if (Input.GetKeyDown(keyAssign))
        {
            if(_item != null) print(_item.LocationReached);
            switch (_itemPicked)
            {
                case false:
                    PickItem();
                    break;
                case true when _item.LocationReached && _item != null:
                    _item.ThrowObject(Player.Instance.targetAim.position);
                    _itemPicked = false;
                    break;
            }
        }
    }

    private void PickItem()
    {
        if (_itemPicked) return;
        _itemPicked = true;
        _item = ThrowManager.Instance.GetNearestItem(transform);
        _item.SetLocation(pivotToObject.position);
    }
}
