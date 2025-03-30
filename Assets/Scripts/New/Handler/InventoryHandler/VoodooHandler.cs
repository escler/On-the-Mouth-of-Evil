using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoodooHandler : MonoBehaviour
{
    [SerializeField] private int countMax;
    private int _count;

    public List<GameObject> voodooes = new List<GameObject>();
    public static VoodooHandler Instance { get; private set; }

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
        if (voodooes.Count >= countMax) return;
        var go = Instantiate(itemObj);
        var pos = voodooes[_count].transform;
        go.transform.position = pos.position;
        go.transform.rotation = pos.rotation;
        voodooes.Add(itemObj);
        _count++;
    }

    public void RemoveItem(GameObject itemObj)
    {
        if (!voodooes.Contains(itemObj)) return;
        var go = voodooes.Find(x => itemObj);
        voodooes.Remove(go);
        Destroy(go);
        _count--;
    }
}
