using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrailSpawner : MonoBehaviour

{
    public GameObject particlePrefab;  
    public float spawnDistance ;  
    public float spawnRate ;    
    private Vector3 lastSpawnPosition; 
    private List<Vector3> spawnPositions; 
    private float timeSinceLastSpawn;  

    void Start()
    {
        lastSpawnPosition = transform.position;
        spawnPositions = new List<Vector3> { lastSpawnPosition };
        timeSinceLastSpawn = 0f;
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        float distanceMoved = Vector3.Distance(transform.position, lastSpawnPosition);

        if (distanceMoved >= spawnDistance && timeSinceLastSpawn >= spawnRate)
        {
            SpawnParticles();

            lastSpawnPosition = transform.position;

            spawnPositions.Add(lastSpawnPosition);

            timeSinceLastSpawn = 0f;
        }
    }

    void SpawnParticles()
    {
        GameObject particleInstance = Instantiate(particlePrefab, transform.position, transform.rotation);

        ParticleSystem particleSystem = particleInstance.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            Destroy(particleInstance, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);
        }
        else
        {
            Destroy(particleInstance, 5f);
        }
    }
}