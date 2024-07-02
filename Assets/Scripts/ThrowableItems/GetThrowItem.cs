using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetThrowItem : MonoBehaviour
{
    private GameObject _item;
    private void OnEnable()
    {
        StartCoroutine(TryGetItem());
    }

    private void GetObjectFromPool()
    {
        if (_item != null) return;
        _item = FactoryThrowItems.Instance.GetObject();
        _item.transform.position = transform.position;
        _item.transform.SetParent(transform);
        _item.SetActive(true);
        ThrowManager.Instance._throwItems.Add(_item.GetComponent<ThrowItem>());
    }

    IEnumerator TryGetItem()
    {
        yield return new WaitForSeconds(0.5f);
        while (FactoryThrowItems.Instance.poolObjects.Count == 0)
        {
            yield return new WaitForSeconds(1f);
        }
        GetObjectFromPool();
    }
}
