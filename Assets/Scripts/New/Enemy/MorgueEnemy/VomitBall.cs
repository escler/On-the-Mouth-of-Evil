using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VomitBall : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject particle;
    [SerializeField] LayerMask layer;
    [SerializeField] private float timeToExplode;
    [SerializeField] private float splashTime;
    private float _actualTime;
    bool trajectory;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void StartTrajectory()
    {
        trajectory = true;
    }
    
    private void Update()
    {
        if (!trajectory) return;

        _actualTime += Time.deltaTime;
        transform.position += transform.forward * speed * Time.deltaTime;
        if (_actualTime < timeToExplode) return;
        
        Explode();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!trajectory) return;
        if (other.gameObject.layer == 19 || other.gameObject.layer == 2 || other.gameObject.layer == 8)
        {
            if(other.gameObject.CompareTag("Player")) PlayerHandler.Instance.Vomit(splashTime);
            print("Pegue");
            Explode();
        }

    }

    private void Explode()
    {
        Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
