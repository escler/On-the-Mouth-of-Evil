using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using Random = UnityEngine.Random;

public class FactoryThrowItems : MonoBehaviour
{
    public GameObject[] prefabVariants;
    public List<GameObject> poolObjects = new List<GameObject>();
    void Start()
    {
        StartCoroutine(SpawnThrowItems());
    }

    IEnumerator ThrowItemsPoolCreation(int count, Func<GameObject> spawn,
        Action<List<GameObject>> OnEndCallBack)
    {
        List<GameObject> tempList = new List<GameObject>();
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        int frameCount = 0;

        if (tempList.Count == 50)
        {
            OnEndCallBack?.Invoke(tempList);
        }
        
        for (int i = 0; i < count; i++)
        {
            GameObject newItem = spawn();
            if (tempList.Count > 0)
            {
                //if(newItem == tempList[i-1]) Destroy(newItem);
              
                    newItem.transform.SetParent(gameObject.transform);
                    newItem.SetActive(false);
                    poolObjects.Add(newItem);
            }
            else
            {
                newItem.transform.SetParent(gameObject.transform);
                newItem.SetActive(false);
                tempList.Add(newItem);
            }
            

            
            if (stopWatch.ElapsedMilliseconds > 1 / 60)
            {
                yield return new WaitForEndOfFrame();
                frameCount += 1;
                stopWatch.Restart();
                print("stopWatch Restart");
            }
        }
    }

    void GetList(List<GameObject> asd)
    {
        foreach (var VARIABLE in asd)
        {
            poolObjects.Add(VARIABLE);
        }
    }
    
    

    IEnumerator SpawnThrowItems()
    {
        yield return ThrowItemsPoolCreation(50,
            () => Instantiate(prefabVariants[Random.Range(0, prefabVariants.Length - 1)]),
            GetList => print("Listo"));
    }
}
