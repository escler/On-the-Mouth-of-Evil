using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class IncenseHandler : InventoryItemHandler
{
    
    public List<GameObject> incienses = new List<GameObject>();
    public static IncenseHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SceneManager.sceneLoaded += CreateItems;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= CreateItems;
    }

    private void CreateItems(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(SceneManager.GetActiveScene().name != "Hub") return;
        count = PlayerPrefs.HasKey("IncienseCount") ? PlayerPrefs.GetInt("IncienseCount") : 0;
        incienses.Clear();
        
        for (int i = 0; i < count; i++)
        {
            var go = Instantiate(handlerItem.gameObject);
            incienses.Add(go);
            if (SceneManager.GetActiveScene().name != "Hub") return;
            var pos = transform.GetChild(i).transform;
            go.transform.position = pos.position;
            go.transform.rotation = pos.rotation;
        }
    }

    public override void AddItem(GameObject itemObj)
    {
        if (incienses.Count >= countMax) return;
        var go = Instantiate(itemObj);
        incienses.Add(go);
        count++;
        SaveCount(true);
        if (SceneManager.GetActiveScene().name != "Hub") return;
        var pos = transform.GetChild(count - 1).transform;
        go.transform.position = pos.position;
        go.transform.rotation = pos.rotation;
    }

    public override void RemoveItem(GameObject itemObj)
    {
        if (!incienses.Contains(itemObj)) return;
        var go = incienses.Find(x => itemObj);
        incienses.Remove(go);
        Destroy(go);
        count--;
        SaveCount(false);
    }
}
