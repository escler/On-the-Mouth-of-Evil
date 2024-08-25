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
    public int force = 1;
    public GameObject reference;
    public int hitCount = 3;
    public float timeToEnable = 10f;
    private int _layer;
    public GameObject hit;

    public bool LocationReached => _locationReached;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        timeToEnable = 10f;
    }

    private void Update()
    {
        modelTransform.Rotate(0,speedRot * Time.deltaTime,0);

        if (_callBackHit) timeToEnable -= Time.deltaTime;

        if (timeToEnable <= 0) gameObject.SetActive(false);
    }

    public void SetLocation(Vector3 location, GameObject refe)
    {
        _location = location;
        _moving = true;
        reference = refe;
        if (reference == null) return;
        _layer = reference.layer == 6 ? 7 : 6;
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

        while (!_callBackHit)
        {
            var refePos = new Vector3(reference.transform.position.x, transform.position.y,
                reference.transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, refePos, ref zero, _time);
            yield return new WaitForEndOfFrame();
        }
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

        yield return new WaitUntil(() => _callBackHit);
        
        while (_callBackHit)
        {
            transform.position += transform.forward * (shootSpeed * Time.deltaTime);
            _collider.enabled = true;
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDisable()
    {
        _collider.enabled = false;
        reference = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != _layer) return;
        
        if(_layer == 7)
            other.GetComponentInParent<LifeHandler>().TakeDamage(damage,force, hitCount);
        else 
            other.GetComponent<LifeHandler>().TakeDamage(damage,force, hitCount);

        Instantiate(hit, transform.position, transform.rotation);
        ThrowManager.Instance.RemoveFormList(this);
        FactoryThrowItems.Instance.BackToPool(gameObject);
    }
}
