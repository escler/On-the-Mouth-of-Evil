using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Node[] nodesRoom;
    public Door[] doors;
    private int count;

    private void Awake()
    {
        count = doors.Length;
    }

    public void CheckDoors()
    {
        int actualCount = 0;
        foreach (var door in doors)
        {
            if(!door.saltBlock) continue;
            actualCount++;
        }

        if (actualCount != count) return;
        
        DisableNodes();
    }
    
    public void DisableNodes()
    {
        foreach (var node in nodesRoom)
        {
            node.gameObject.SetActive(false);
        }
    }
    
    public void EnableNodes()
    {
        foreach (var node in nodesRoom)
        {
            node.gameObject.SetActive(true);

        }
    }
}
