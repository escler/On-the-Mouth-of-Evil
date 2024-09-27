using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Node[] nodesRoom;
    public Door[] doors;
    public GameObject[] interactableEnemy;
    public MovableItem[] movableItems;
    private int count;
    public bool cantBlock;
    private void Awake()
    {
        count = doors.Length;
        foreach (var node in nodesRoom)
        {
            node.room = this;
        }
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
        int actualCount = 0;
        foreach (var door in doors)
        {
            if(door.saltBlock) continue;
            actualCount++;
        }

        if (actualCount != count) return;
        
        foreach (var node in nodesRoom)
        {
            node.gameObject.SetActive(true);

        }
    }

    public void BlockDoors()
    {
        for (int i = 0; i < movableItems.Length; i++)
        {
            movableItems[i].LockDoor();
        }
    }

}
