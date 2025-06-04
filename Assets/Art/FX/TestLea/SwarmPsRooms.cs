using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SwarmPsRooms : MonoBehaviour
{
    private Room _room;
    ParticleSystem _particle;
    private void Awake()
    {
        _room = GetComponentInParent<Room>();
        _particle = GetComponent<ParticleSystem>();
        AdjustSize();
    }

    void AdjustSize()
    {
        var shapePS = _particle.shape;
        shapePS.scale = _room.GetComponent<BoxCollider>().size;
    }

    private void OnParticleCollision(GameObject other)
    {
        _particle.Stop();
    }

    public void PlayPS(float seconds)
    {
        print("Play PS");
        var psMain = _particle.main;
        psMain.duration = seconds;
        _particle.Play();
    }
}
