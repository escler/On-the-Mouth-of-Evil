using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighterHandler : MonoBehaviour
{
    [SerializeField] private int countMax;
    private int _count;

    public List<GameObject> lighters;
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

    public void AddLighter(GameObject lighter)
    {
        if (lighters.Count >= countMax) return;
        var go = Instantiate(lighter);
        var pos = transform.GetChild(_count).transform;
        go.transform.position = pos.position;
        go.transform.rotation = pos.rotation;
        lighters.Add(lighter);
        _count++;
    }

    public void RemoveLighter(GameObject lighter)
    {
        if (lighters.Contains(lighter)) return;
        var go = lighters.Find(x => lighter);
        lighters.Remove(go);
        Destroy(go);
        _count--;
    }
}
