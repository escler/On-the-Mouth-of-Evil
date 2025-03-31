using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RosaryHandler : InventoryItemHandler
{
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
        SceneManager.sceneLoaded += CreateItems;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= CreateItems;
    }

    private void CreateItems(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(SceneManager.GetActiveScene().name != "Hub") return;
        count = PlayerPrefs.HasKey("RosaryCount") ? PlayerPrefs.GetInt("RosaryCount") : 0;
        rosaries.Clear();

        for (int i = 0; i < count; i++)
        {
            var go = Instantiate(handlerItem.gameObject);
            rosaries.Add(go);
            if (SceneManager.GetActiveScene().name != "Hub") return;
            var pos = transform.GetChild(i).transform;
            go.transform.position = pos.position;
            go.transform.rotation = pos.rotation;
        }
    }

    public override void AddItem(GameObject itemObj)
    {
        if (rosaries.Count >= countMax) return;
        var go = Instantiate(itemObj);
        rosaries.Add(itemObj);
        count++;
        SaveCount(true);
        if (SceneManager.GetActiveScene().name != "Hub") return;
        var pos = transform.GetChild(count - 1).transform;
        go.transform.position = pos.position;
        go.transform.rotation = pos.rotation;
    }

    public override void RemoveItem(GameObject itemObj)
    {
        if (!rosaries.Contains(itemObj)) return;
        var go = rosaries.Find(x => itemObj);
        rosaries.Remove(go);
        Destroy(go);
        count--;
        SaveCount(false);
    }

}
