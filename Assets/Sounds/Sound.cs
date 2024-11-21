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
            MusicManager.Instance.PlayBkMusic(soundName);
            isPlayBackgroundMusic = true; // Ensure it doesn't play repeatedly
        }
        else
        {
            Debug.LogError("MusicManager instance is not initialized.");
        }
    }
}
