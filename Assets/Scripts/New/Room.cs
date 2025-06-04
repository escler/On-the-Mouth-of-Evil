using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Node[] nodesRoom;
    public Door[] doors;
    public GameObject[] interactableEnemy;
    public MovableItem[] movableItems;
    private int count;
    public bool cantBlock;
    public bool roomBlocked;

    public SwarmPsRooms _ps;
    public bool swarmActivate;
    private void Awake()
    {
        count = doors.Length;
        foreach (var node in nodesRoom)
        {
            node.room = this;
        }
    }

    public void ActivateSwarm(float seconds)
    {
        if (swarmActivate) return;
        
        _ps.PlayPS(seconds);
        swarmActivate = true;
        StartCoroutine(TimerForSwarm(seconds));
    }

    IEnumerator TimerForSwarm(float seconds)
    {
        float timer = 0;
        while (timer < seconds)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        swarmActivate = false;
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
        roomBlocked = true;
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

        roomBlocked = false;
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
