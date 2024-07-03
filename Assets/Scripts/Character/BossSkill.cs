using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossSkill : MonoBehaviour
{
    public KeyCode keyAssign = KeyCode.F;
    public ThrowItem _item;
    public ThrowItem Item => _item;
    public Transform pivotToObject;
    private bool _itemPicked;

    private void Update()
    {
        if (Input.GetKeyDown(keyAssign))
        {
            switch (_itemPicked)
            {
                case false:
                    PickItem();
                    break;
                case true when _item.LocationReached && _item != null:
                    Player.Instance.playerAnim.throwObject = true;
                    break;
            }
        }
    }

    public void ThrowItem()
    {
        _item.ThrowObject(Player.Instance.targetAim.position);
        _itemPicked = false;
        _item = null;
    }
    private void PickItem()
    {
        if (_itemPicked) return;
        _itemPicked = true;
        _item = ThrowManager.Instance.GetNearestItem(transform);
        _item.SetLocation(pivotToObject.position);
    }
}
