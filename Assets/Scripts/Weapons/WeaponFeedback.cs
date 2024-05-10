using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WeaponFeedback : MonoBehaviour
{
    [SerializeField] private ParticleSystem _psBlood, _psFireShoot;
    [SerializeField] private GameObject _bulletHole;

    public void WeaponShootFeedback(Vector3 hitPos, int targetLayer, Vector3 normal)
    {
        switch (targetLayer)
        {
            //Enemy
            case 7: 
                PlayParticle(_psBlood, hitPos);
                break;
            //Obstacle
            case 8:
                CreateHole(_bulletHole,hitPos,normal);
                break;
        }
    }

    public void SetFireParticle(ParticleSystem psFire)
    {
        _psFireShoot = psFire;
    }
    
    public void FireParticle()
    {
        _psFireShoot.gameObject.SetActive(true);
    }

    public void CreateHole(GameObject bulletHole, Vector3 hitPos, Vector3 normal)
    {
        Instantiate(bulletHole, hitPos + (normal * 0.1f), Quaternion.FromToRotation(Vector3.up, normal));
    }

    public void PlayParticle(ParticleSystem ps, Vector3 hitPos)
    {
        Instantiate(ps, hitPos, Quaternion.identity);
    }
}
