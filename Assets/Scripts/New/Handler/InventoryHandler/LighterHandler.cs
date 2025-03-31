using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LighterHandler : InventoryItemHandler
{
    public List<GameObject> lighters = new List<GameObject>();
    public static LighterHandler Instance { get; private set; }
    

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
        count = PlayerPrefs.HasKey("LighterCount") ? PlayerPrefs.GetInt("LighterCount") : 0;
        lighters.Clear();

        for (int i = 0; i < count; i++)
        {
            var go = Instantiate(handlerItem.gameObject);
            lighters.Add(go);
            if (SceneManager.GetActiveScene().name != "Hub") return;
            var pos = transform.GetChild(i).transform;
            go.transform.position = pos.position;
            go.transform.rotation = pos.rotation;
        }
    }

    public override void AddItem(GameObject itemObj)
    {
        if (lighters.Count >= countMax) return;
        var go = Instantiate(itemObj);
        lighters.Add(itemObj);
        count++;
        SaveCount(true);
        if (SceneManager.GetActiveScene().name != "Hub") return;
        var pos = transform.GetChild(count - 1).transform;
        go.transform.position = pos.position;
        go.transform.rotation = pos.rotation;
    }

    public override void RemoveItem(GameObject itemObj)
    {
        if (lighters.Contains(itemObj)) return;
        var go = lighters.Find(x => itemObj);
        lighters.Remove(go);
        Destroy(go);
        count--;
        SaveCount(false);
    }
}
