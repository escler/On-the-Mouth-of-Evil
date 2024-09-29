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
        if(!HouseEnemy.Instance.compareRoom) return;
        for (int i = 0; i < movableItems.Length; i++)
        {
            movableItems[i].LockDoor();
        }

        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].open = false;
            doors[i].SetDoor(doors[i].open);
        }
    }

    public bool DoorsBlocked()
    {
        int count = 0;
        for (int i = 0; i < movableItems.Length; i++)
        {
            if (movableItems[i].relocated) count++;
            else break;
        }
        print(count);
        print(movableItems.Length);

        return count == movableItems.Length;
    }

}
