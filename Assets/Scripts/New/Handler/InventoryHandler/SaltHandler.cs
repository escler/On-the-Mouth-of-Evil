using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaltHandler : InventoryItemHandler
{
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
        SceneManager.sceneLoaded += CreateItems;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= CreateItems;
    }

    private void CreateItems(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(SceneManager.GetActiveScene().name != "Hub") return;
        salts.Clear();

        for (int i = 0; i < count; i++)
        {
            var go = Instantiate(handlerItem.gameObject);
            salts.Add(go);
            if (SceneManager.GetActiveScene().name != "Hub") return;
            var pos = transform.GetChild(i).transform;
            go.transform.position = pos.position;
            go.transform.rotation = pos.rotation;
        }
    }

    public override void AddItem(GameObject itemObj)
    {
        if (salts.Count >= countMax) return;
        var go = Instantiate(itemObj);
        salts.Add(itemObj);
        count++;
        if (SceneManager.GetActiveScene().name != "Hub") return;
        var pos = transform.GetChild(count - 1).transform;
        go.transform.position = pos.position;
        go.transform.rotation = pos.rotation;
    }

    public override void RemoveItem(GameObject itemObj)
    {
        if (!salts.Contains(itemObj)) return;
        var go = salts.Find(x => itemObj);
        salts.Remove(go);
        Destroy(go);
        count--;
    }
}
