using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BibleHandler : MonoBehaviour
{
    [SerializeField] private int countMax;
    private int _count;

    public List<GameObject> bibles = new List<GameObject>();
    public static BibleHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddItem(GameObject itemObj)
    {
        if (bibles.Count >= countMax) return;
        var go = Instantiate(itemObj);
        var pos = bibles[_count].transform;
        go.transform.position = pos.position;
        go.transform.rotation = pos.rotation;
        bibles.Add(itemObj);
        _count++;
    }

    public void RemoveItem(GameObject itemObj)
    {
        if (!bibles.Contains(itemObj)) return;
        var go = bibles.Find(x => itemObj);
        bibles.Remove(go);
        Destroy(go);
        _count--;
    }
}
