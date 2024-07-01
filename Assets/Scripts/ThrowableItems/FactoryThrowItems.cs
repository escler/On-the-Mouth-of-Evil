using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using Object = System.Object;
using Random = UnityEngine.Random;

public class FactoryThrowItems : MonoBehaviour
{
    public static FactoryThrowItems Instance { get; private set; }
    
    public GameObject[] prefabVariants;
    public List<GameObject> poolObjects = new List<GameObject>();
    void Start()
    {
        StartCoroutine(SpawnThrowItems());
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    IEnumerator ThrowItemsPoolCreation(int count, Func<GameObject> spawn, //IA2-P4
        Action<List<GameObject>> onEndCallBack)
    {
        List<GameObject> tempList = new List<GameObject>();
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        int frameCount = 0;

        while (prefabVariants != null)
        {
            if (tempList.Count > count - poolObjects.Count)
            {
                onEndCallBack?.Invoke(tempList);
                break;
            }
            
            GameObject newItem = spawn();
            if (tempList.Count > 0)
            {
                if(newItem == tempList.Last())Destroy(newItem);
                else
                {
                    newItem.transform.SetParent(gameObject.transform);
                    newItem.SetActive(false);
                    poolObjects.Add(newItem);
                }
              
            }
            else
            {
                if (poolObjects.Count > 0)
                {
                    if(newItem == poolObjects.Last()) Destroy(newItem);
                }
                else
                {
                    newItem.transform.SetParent(gameObject.transform);
                    newItem.SetActive(false);
                    tempList.Add(newItem);
                }
            }
            
            if (stopWatch.ElapsedMilliseconds > 1 / 60)
            {
                yield return new WaitForEndOfFrame();
                frameCount += 1;
                stopWatch.Restart();
            }
        }
    }

    void GetList(List<GameObject> tempList)
    {
        foreach (var item in tempList)
        {
            poolObjects.Add(item);
        }
    }

    public void BackToPool(GameObject item)
    {
        item.SetActive(false);
        if (poolObjects.Contains(item)) return;
        poolObjects.Add(item);
    }
    
    public GameObject GetObject()
    {
        var objectFromPool = poolObjects.First();
        poolObjects.Remove(poolObjects.First());
        return objectFromPool;
    }
    
    

    IEnumerator SpawnThrowItems()
    {
        yield return ThrowItemsPoolCreation(50,
            () => Instantiate(prefabVariants[Random.Range(0, prefabVariants.Length - 1)]),
            GetList);
    }
}
