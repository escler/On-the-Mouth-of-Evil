using System;
using System.Collections.Generic;
using System.Resources;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicManager : Singleton<MusicManager>
{
    private AudioSource bgMusic = null;
    public AudioMixerGroup ambientGroup;
    public AudioMixerGroup fxGroup;
    [SerializeField] private float bgValue = 1;
    private GameObject soundObj = null;
    public List<AudioSource> soundList = new List<AudioSource>();
    [SerializeField] private float soundValue = 1;
    
    protected override void Awake()
    {
        base.Awake();
        if (UpdateManager.Instance != null)
        {
            UpdateManager.Instance.AddUpdateListener(MyUpdate);
        }
        else
        {
            Debug.Log("UpdateManager instance is null");
        }
        SceneManager.sceneLoaded += ClearAudios;

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ClearAudios;
    }

    private void ClearAudios(Scene scene, LoadSceneMode loadSceneMode)
    {
        soundList.Clear();
        StopSound(bgMusic);
    }

    private void MyUpdate()
    {
        for (int i = soundList.Count - 1; i >= 0; --i)
        {
            if (!soundList[i].isPlaying)
            {
                GameObject.Destroy(soundList[i]);
                soundList.RemoveAt(i);
            }
        }
    }

    public void PlayBkMusic(string name)
    {
        if (bgMusic == null)
        {
            GameObject obj = new GameObject("BgMusic");
            bgMusic = obj.AddComponent<AudioSource>();
            bgMusic.outputAudioMixerGroup = ambientGroup;
        }
        ResourceManager.Instance.LoadAsync<AudioClip>("MusicPath/" + name, (clip) =>
        {
            bgMusic.clip = clip;
            bgMusic.loop = true;
            bgMusic.volume = bgValue;
            bgMusic.Play();
        });
    }

    public void PauseBgMusic()
    {
        if (bgMusic == null)
            return;
        bgMusic.Pause();
    }

    public void StopBgMusic()
    {
        if (bgMusic == null)
            return;
        bgMusic.Stop();
    }

    public void ChangeBKValue(float v)
    {
        bgValue = v;
        if (bgMusic == null)
            return;
        bgMusic.volume = bgValue;
    }

    public void PlaySound(string name, bool isLoop, UnityAction<AudioSource> callBack = null)
    {
        if (soundObj == null)
        {
            soundObj = new GameObject("Sound");
        }
        ResourceManager.Instance.LoadAsync<AudioClip>("MusicPathVFX/" + name, (clip) =>
        {
            AudioSource source = soundObj.AddComponent<AudioSource>();
            source.clip = clip;
            source.outputAudioMixerGroup = fxGroup;
            source.loop = isLoop;
            source.volume = soundValue;
            source.Play();
            Debug.Log("Playing sound: " + name);
            soundList.Add(source);
            if (callBack != null)
                callBack(source);
        });
    }

    public void ChangeSoundValue(float value)
    {
        soundValue = value;
        for (int i = 0; i < soundList.Count; ++i)
            soundList[i].volume = value;
    }

    public void StopSound(AudioSource source)
    {
        if (soundList.Contains(source))
        {
            soundList.Remove(source);
            source.Stop();
            GameObject.Destroy(source);
        }
    }
}

