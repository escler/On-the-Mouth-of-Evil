using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRDemonLifeHandler : LifeHandler
{
    public AudioSource _audioSource;
    public AudioClip[] hitSounds;
    
    public override void TakeDamage(int damage, int force, int hitCount)
    {
        base.TakeDamage(damage, force, hitCount);
        _audioSource.PlayOneShot(hitSounds[Random.Range(0,hitSounds.Length)]);
        var enemy = GetComponentInParent<DemonLowRange>();
        if (_actualLife > 0)
        {
            enemy.AddHitCount(hitCount);
            enemy.force = force;
            return;
        }

        enemy.canBanish = true;
    }
}
