using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEnemy : MonoBehaviour
{
    private Transform _characterPos;
    public float distanceToExplode, lifeTime, speed;
    private bool _explosion;
    public GameObject startExplode;

    private void OnEnable()
    {
        _characterPos = Player.Instance.transform;
        StartCoroutine(DisableObject());
    }
    private void Update()
    {
        transform.LookAt(new Vector3(_characterPos.position.x, transform.position.y, _characterPos.position.z));
        transform.position += transform.forward * (Time.deltaTime * speed);

        if (Vector3.Distance(_characterPos.position, transform.position) > distanceToExplode) return;
        
        ActiveExplosion();
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

    public void ActiveExplosion()
    {
        if (_explosion) return;
        StartCoroutine(DelayedExplosion());
    }

    IEnumerator DelayedExplosion()
    {
        startExplode.SetActive(true);
        yield return new WaitForSeconds(1f);
        transform.GetChild(0).gameObject.SetActive(true);
    }
    
    
}
