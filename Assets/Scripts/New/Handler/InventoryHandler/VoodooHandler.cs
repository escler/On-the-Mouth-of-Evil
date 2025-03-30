using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoodooHandler : InventoryItemHandler
{
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
        SceneManager.sceneLoaded += CreateItems;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= CreateItems;
    }

    private void CreateItems(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(SceneManager.GetActiveScene().name != "Hub") return;
        voodooes.Clear();

        for (int i = 0; i < count; i++)
        {
            var go = Instantiate(handlerItem.gameObject);
            voodooes.Add(go);
            if (SceneManager.GetActiveScene().name != "Hub") return;
            var pos = transform.GetChild(i).transform;
            go.transform.position = pos.position;
            go.transform.rotation = pos.rotation;
        }
    }

    public override void AddItem(GameObject itemObj)
    {
        if (voodooes.Count >= countMax) return;
        var go = Instantiate(itemObj);
        voodooes.Add(itemObj);
        count++;
        if (SceneManager.GetActiveScene().name != "Hub") return;
        var pos = transform.GetChild(count - 1).transform;
        go.transform.position = pos.position;
        go.transform.rotation = pos.rotation;
    }

    public override void RemoveItem(GameObject itemObj)
    {
        if (!voodooes.Contains(itemObj)) return;
        var go = voodooes.Find(x => itemObj);
        voodooes.Remove(go);
        Destroy(go);
        count--;
    }
}
