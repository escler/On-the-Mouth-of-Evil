using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticles : MonoBehaviour
{
     [SerializeField] private ParticleSystem _particleSystem;

    private void Start()
    {
       // _particleSystem = GetComponent<ParticleSystem>();
    }


    public void PlayP()
    {
        _particleSystem.Play();
    }

}
