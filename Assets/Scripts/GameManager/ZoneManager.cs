using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZoneManager : MonoBehaviour
{
    private BoxCollider _collider;
    public Door doorRoom, nextRoomDoor;
    [SerializeField] private GameObject spawnPrefab;
    public int count, deadCount;
    public GameObject nextZone, previousZone;
    public SpatialGrid thisGrid;
    public int zone;
    public string roomName;

    private void OnEnable()
    {
        GameManager.Instance.activeZoneManager = this;
        GameManager.Instance.activeSpatialGrid = thisGrid;
        doorRoom.SetDoor(false);
        _collider = GetComponent<BoxCollider>();
        for (int i = 0; i < count; i++)
        {
            SpawnEnemy();
        }

        if (nextZone == null) return;
        nextZone.SetActive(true);
        previousZone.SetActive(false);
    }

    public void SpawnEnemy()
    {
        var randomPos = RandomPointInBounds(_collider.bounds);
        var spawn = Instantiate(spawnPrefab, new Vector3(randomPos.x, transform.position.y, randomPos.z), transform.rotation);
        spawn.GetComponent<SpawnEnemy>().SpawnWithDelay();
    }

    public void EnemyDead()
    {
        deadCount++;
        if (deadCount < count) return;
        if (doorRoom == null) return;
        doorRoom.SetDoor(true);
        nextRoomDoor.SetDoor(true);
        ListDemonsUI.Instance.AddText(zone, "<s><color=\"red\">" + roomName + "</s></color>");
    }
    
    public static Vector3 RandomPointInBounds(Bounds bounds) {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
