using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltHandler : MonoBehaviour
{
    [SerializeField] private int countMax;
    private int _count;

    public List<GameObject> salts = new List<GameObject>();
    public static SaltHandler Instance { get; private set; }

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
        if (salts.Count >= countMax) return;
        var go = Instantiate(itemObj);
        var pos = salts[_count].transform;
        go.transform.position = pos.position;
        go.transform.rotation = pos.rotation;
        salts.Add(itemObj);
        _count++;
    }

    public void RemoveItem(GameObject itemObj)
    {
        if (!salts.Contains(itemObj)) return;
        var go = salts.Find(x => itemObj);
        salts.Remove(go);
        Destroy(go);
        _count--;
    }
}
