using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogAttackEnemy : MonoBehaviour
{
    public bool isExplosionEnemy;
    public float speed;
    private Transform _characterPos;
    private void Awake()
    {
        _characterPos = Player.Instance.transform;
    }

    private void Update()
    {
        transform.LookAt(new Vector3(_characterPos.position.x,transform.position.y,_characterPos.position.z));

        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            if (isExplosionEnemy)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}
