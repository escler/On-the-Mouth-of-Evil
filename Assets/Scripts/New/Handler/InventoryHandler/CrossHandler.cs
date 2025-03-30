using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHandler : MonoBehaviour
{
    [SerializeField] private int countMax;
    private int _count;

    public List<GameObject> crosses = new List<GameObject>();
    public static CrossHandler Instance { get; private set; }

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
        if (crosses.Count >= countMax) return;
        var go = Instantiate(itemObj);
        var pos = crosses[_count].transform;
        go.transform.position = pos.position;
        go.transform.rotation = pos.rotation;
        crosses.Add(itemObj);
        _count++;
    }

    public void RemoveItem(GameObject itemObj)
    {
        if (!crosses.Contains(itemObj)) return;
        var go = crosses.Find(x => itemObj);
        crosses.Remove(go);
        Destroy(go);
        _count--;
    }
}
