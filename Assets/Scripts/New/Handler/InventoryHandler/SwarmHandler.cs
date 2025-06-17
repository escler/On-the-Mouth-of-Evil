using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwarmHandler : InventoryItemHandler
{
    public List<GameObject> swarm = new List<GameObject>();
    public static SwarmHandler Instance { get; private set; }

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
        count = PlayerPrefs.HasKey("SwarmCount") ? PlayerPrefs.GetInt("SwarmCount") : 0;
        swarm.Clear();
        
        for (int i = 0; i < count; i++)
        {
            var go = Instantiate(handlerItem.gameObject);
            swarm.Add(go);
            if (SceneManager.GetActiveScene().name != "Hub") return;
            var pos = transform.GetChild(i).transform;
            go.transform.position = pos.position;
            go.transform.rotation = pos.rotation;
        }
    }

    public override void AddItem(GameObject itemObj)
    {
        if (swarm.Count >= countMax) return;
        var go = Instantiate(itemObj);
        swarm.Add(go);
        count++;
        SaveCount(true);
        if (SceneManager.GetActiveScene().name != "Hub") return;
        var pos = transform.GetChild(count - 1).transform;
        go.transform.position = pos.position;
        go.transform.rotation = pos.rotation;
    }

    public override void RemoveItem(GameObject itemObj)
    {
        if (!swarm.Contains(itemObj)) return;
        var go = swarm.Find(x => itemObj);
        swarm.Remove(go);
        Destroy(go);
        count--;
        SaveCount(false);
    }
}
