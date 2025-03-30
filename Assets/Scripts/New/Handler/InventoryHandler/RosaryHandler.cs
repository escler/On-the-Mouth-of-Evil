using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RosaryHandler : MonoBehaviour
{
    [SerializeField] private int countMax;
    private int _count;

    public List<GameObject> rosaries = new List<GameObject>();
    public static RosaryHandler Instance { get; private set; }

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
        if (rosaries.Count >= countMax) return;
        var go = Instantiate(itemObj);
        var pos = rosaries[_count].transform;
        go.transform.position = pos.position;
        go.transform.rotation = pos.rotation;
        rosaries.Add(itemObj);
        _count++;
    }

    public void RemoveItem(GameObject itemObj)
    {
        if (!rosaries.Contains(itemObj)) return;
        var go = rosaries.Find(x => itemObj);
        rosaries.Remove(go);
        Destroy(go);
        _count--;
    }
}
