using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BibleHandler : MonoBehaviour
{
    [SerializeField] private int countMax;
    private int _count;

    public List<GameObject> bibles;
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

    public void AddBible(GameObject bible)
    {
        if (bibles.Count >= countMax) return;
        var go = Instantiate(bible);
        var pos = bibles[_count].transform;
        go.transform.position = pos.position;
        go.transform.rotation = pos.rotation;
        bibles.Add(bible);
        _count++;
    }

    public void RemoveBible(GameObject bible)
    {
        if (bibles.Contains(bible)) return;
        var go = bibles.Find(x => bible);
        bibles.Remove(go);
        Destroy(go);
        _count--;
    }
}
