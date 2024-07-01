using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetThrowItem : MonoBehaviour
{
    private void OnEnable()
    {
        GetObjectFromPool();
    }

    private void GetObjectFromPool()
    {
        var item = FactoryThrowItems.Instance.GetObject();
        item.transform.position = transform.position;
        item.transform.SetParent(transform);
        item.SetActive(true);
        ThrowManager.Instance._throwItems.Add(item.GetComponent<ThrowItem>());
    }
}
