using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThrowManager : MonoBehaviour
{
    public List<ThrowItem> _throwItems = new List<ThrowItem>();
    public static ThrowManager Instance { get; private set; }

    public List<ThrowItem> ThrowItems => _throwItems;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }


    private void RemoveAll()
    {
        if (_throwItems.Count <= 0) return;
        _throwItems.Clear();
    }

    public void RemoveFormList(ThrowItem item)
    {
        if (!_throwItems.Contains(item)) return;

        _throwItems.Remove(item);
    }

    public ThrowItem GetItem()
    {
        if (_throwItems.Count <= 0) return null;
        var actualItem = _throwItems.First();
        _throwItems.Remove(actualItem);
        return actualItem;
    }

    public ThrowItem GetNearestItem(Transform target) //IA2-P1
    {
        if (_throwItems.Count <= 0) return null;

        return _throwItems.Aggregate(_throwItems.First(), (acum, current) =>
        {
            var actualNearest = Vector3.Distance(acum.transform.position, target.position);
            if (Vector3.Distance(current.transform.position, target.position) < actualNearest)
                acum = current;
            return acum;
        });
    }
}
