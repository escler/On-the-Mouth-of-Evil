using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEnemy : MonoBehaviour
{
    private Transform _characterPos;
    public float distanceToExplode, lifeTime, speed;

    private void OnEnable()
    {
        _characterPos = Player.Instance.transform;
        StartCoroutine(DisableObject());
    }
    private void Update()
    {
        transform.LookAt(_characterPos);
        transform.position += transform.forward * (Time.deltaTime * speed);

        if (Vector3.Distance(_characterPos.position, transform.position) > distanceToExplode) return;
        
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        StopCoroutine(DisableObject());
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(lifeTime);

        Destroy(gameObject);
    }
    
    
}
