using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BibleHandler : InventoryItemHandler
{
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
        SceneManager.sceneLoaded += CreateItems;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= CreateItems;
    }

    private void CreateItems(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(SceneManager.GetActiveScene().name != "Hub") return;
        if (PlayerPrefs.HasKey("TutorialCompleted"))
        {
            if (PlayerPrefs.GetInt("TutorialCompleted") == 0)
            {
                count = 0;
                PlayerPrefs.SetInt("BibleCount", count);
                PlayerPrefs.Save();
            }
            else
            {
                count = PlayerPrefs.HasKey("BibleCount") ? PlayerPrefs.GetInt("BibleCount") : 0;
            }
        }
        else
        {
            count = PlayerPrefs.HasKey("BibleCount") ? PlayerPrefs.GetInt("BibleCount") : 0;
        }
        bibles.Clear();

        for (int i = 0; i < count; i++)
        {
            var go = Instantiate(handlerItem.gameObject);
            bibles.Add(go);
            if (SceneManager.GetActiveScene().name != "Hub") return;
            var pos = transform.GetChild(i).transform;
            go.transform.position = pos.position;
            go.transform.rotation = pos.rotation;
        }
    }
    
    public override void AddItem(GameObject itemObj)
    {
        if (bibles.Count >= countMax) return;
        var go = Instantiate(itemObj);
        bibles.Add(go);
        count++;
        SaveCount(true);
        if (SceneManager.GetActiveScene().name != "Hub") return;
        var pos = transform.GetChild(count - 1).transform;
        go.transform.position = pos.position;
        go.transform.rotation = pos.rotation;
    }

    public override void RemoveItem(GameObject itemObj)
    {
        if (!bibles.Contains(itemObj)) return;
        var go = bibles.Find(x => itemObj);
        bibles.Remove(go);
        Destroy(go);
        count--;
        SaveCount(false);
    }
}
