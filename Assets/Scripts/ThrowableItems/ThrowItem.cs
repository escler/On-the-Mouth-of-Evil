using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItem : MonoBehaviour
{
    private Vector3 _location, _playerPos;
    public Transform modelTransform;
    public bool _callBackHit;
    Vector3 zero = Vector3.zero;
    private float _time = 1f;
    public float speedRot;
    public float shootSpeed;
    public BoxCollider _collider;
    public int damage;
    private bool _locationCalculated, _moving, _locationReached;

    public bool LocationReached => _locationReached;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        modelTransform.Rotate(0,speedRot * Time.deltaTime,0);
    }

    public void SetLocation(Vector3 location)
    {
        _location = location;
        _moving = true;
        StartCoroutine(MoveObject());
    }

    IEnumerator MoveObject()
    {
        _locationReached = false;
        while (_moving)
        { 
            transform.position = Vector3.SmoothDamp(transform.position, _location, ref zero, _time);
            if (Vector3.Distance(transform.position, _location) <= .5) _moving = false;
            yield return new WaitForEndOfFrame();
        }

        _locationReached = true;
    }

    public void ThrowObject(Vector3 location)
    {
        StartCoroutine(ThrowObjectCo(location));
    }

    IEnumerator ThrowObjectCo(Vector3 location)
    {
        if (!_locationCalculated)
        {
            _locationCalculated = true;
            transform.LookAt(location);
        }

        while (!_callBackHit)
        {
            transform.position += transform.forward * (shootSpeed * Time.deltaTime);
            _collider.enabled = true;
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDisable()
    {
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            other.GetComponent<PlayerLifeHandler>().OnTakeDamage(damage);
        }
        
        _callBackHit = true;
        ThrowManager.Instance.RemoveFormList(this);
        FactoryThrowItems.Instance.BackToPool(this.gameObject);
    }
}
