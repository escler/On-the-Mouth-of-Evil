using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollider : MonoBehaviour
{
    public int speed, damage;

    private AudioSource _fireballAudioSource;
    public AudioClip fire1, fire2;
    private void OnEnable()
    {
        transform.LookAt(Player.Instance.transform);
        _fireballAudioSource = GetComponent<AudioSource>();
        StartCoroutine(PlaySounds());
        Destroy(gameObject, 20);
    }

    IEnumerator PlaySounds()
    {
        _fireballAudioSource.clip = fire1;
        _fireballAudioSource.Play();
        yield return new WaitForSeconds(_fireballAudioSource.clip.length);
        _fireballAudioSource.clip = fire2;
        _fireballAudioSource.loop = true;
        _fireballAudioSource.Play();
    }

    private void Update()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            other.GetComponent<PlayerLifeHandler>().TakeDamage(damage, 0, 0);
        }
        
        Destroy(gameObject);
    }
}
