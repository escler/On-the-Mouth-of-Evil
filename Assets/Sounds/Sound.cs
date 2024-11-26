using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField] private string soundName;
    private bool isPlayBackgroundMusic;

    void Awake()
    {
        if (MusicManager.Instance == null)
        {
            Debug.Log("MusicManager instance is not initialized.");
        }
    }

    void Start()
    {
        
    }

    private void Update()
    {
        if (!isPlayBackgroundMusic)
        {
            PlayBackGroundSound();
        }
    }

    private void PlayBackGroundSound()
    {
        if (MusicManager.Instance != null)
        {
            if (!isPlayBackgroundMusic)
            {
                MusicManager.Instance.PlayBkMusic(soundName);
                isPlayBackgroundMusic = true;
            }
        }
        else
        {
            Debug.LogError("MusicManager instance is not initialized.");
        }
    }
}
