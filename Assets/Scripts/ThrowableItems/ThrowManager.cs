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
        
        SearchThrowItemsAndAdd();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.H)) SearchThrowItemsAndAdd();
        if(Input.GetKeyDown(KeyCode.J)) RemoveAll();
    }

    public void SearchThrowItemsAndAdd()
    {
        var activeItemsInScene = FindObjectsOfType<ThrowItem>();

        if (activeItemsInScene.Length <= 0) return;
        for (int i = 0; i < activeItemsInScene.Length; i++)
        {
            if (_throwItems.Contains(activeItemsInScene[i])) continue;
            _throwItems.Add(activeItemsInScene[i]);
        }
    }

    public void RemoveAll()
    {
        if (_throwItems.Count <= 0) return;
        _throwItems.Clear();
    }

    public void RemoveFormList(ThrowItem item)
    {
        if (!_throwItems.Contains(item)) return;

        _throwItems.Remove(item);
    }

    public ThrowItem MoveToLocation(Vector3 location)
    {
        if (_throwItems.Count <= 0) return null;
        var actualItem = _throwItems.First();
        _throwItems.Remove(actualItem);
        actualItem.SetLocation(location);
        return actualItem;
    }
}
