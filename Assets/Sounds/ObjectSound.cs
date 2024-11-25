using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSound : MonoBehaviour
{
    [SerializeField] private string soundName;
    AudioSource _audioSource;
    [SerializeField] private bool loop;
    [SerializeField] private float _volume;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.spatialBlend = 1;
        _audioSource.volume = _volume;
        //_audioSource.minDistance = 0.5f;
        //_audioSource.maxDistance = 1f;

    }
    void Start()
    {

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource component is not attached to the object.");
        }
        else
        {
            MusicManager.Instance.PlaySound(soundName, loop);
        }

    }

    void Update()
    {
        
    }
}
