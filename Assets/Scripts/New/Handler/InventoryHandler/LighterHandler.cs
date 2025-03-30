using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighterHandler : MonoBehaviour
{
    [SerializeField] private int countMax;
    private int _count;

    public List<GameObject> lighters = new List<GameObject>();
    public static LighterHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        Instance = this;
    }

    public void AddItem(GameObject itemObj)
    {
        if (lighters.Count >= countMax) return;
        var go = Instantiate(itemObj);
        var pos = transform.GetChild(_count).transform;
        go.transform.position = pos.position;
        go.transform.rotation = pos.rotation;
        lighters.Add(itemObj);
        _count++;
    }

    public void RemoveItem(GameObject itemObj)
    {
        if (lighters.Contains(itemObj)) return;
        var go = lighters.Find(x => itemObj);
        lighters.Remove(go);
        Destroy(go);
        _count--;
    }
}
