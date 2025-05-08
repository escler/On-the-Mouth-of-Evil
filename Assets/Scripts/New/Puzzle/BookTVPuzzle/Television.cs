using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Television : MonoBehaviour
{
    public Material[] channels;
    public Material tvOffMat, changeChannelMat, tvMat;
    private Renderer mesh;
    public AudioSource _tv;
    private int _count;
    private bool _cantChangeChannel, _tvOn;

    private void Awake()
    {
        mesh = GetComponent<Renderer>();
        TvOn();
    }

    public void ChangeMaterial()
    {
        if (_cantChangeChannel || !_tvOn) return;
        _cantChangeChannel = true;
        _count++;
        _count = _count >= channels.Length ? 0 : _count;
        StartCoroutine(ChangeChannel(channels[_count]));
    }

    IEnumerator ChangeChannel(Material mat)
    {
        var distorsionMats = new Material[2];
        _tv.Play();
        distorsionMats[0] = tvMat;
        distorsionMats[1] = changeChannelMat;
        mesh.materials = distorsionMats;
        mesh.materials[1] = changeChannelMat;
        yield return new WaitForSeconds(1f);
        _tv.Stop();
        var newMat = new Material[2];
        newMat[0] = tvMat;
        newMat[1] = mat;
        mesh.materials = newMat;
        _cantChangeChannel = false;
    }

    public void TvOn()
    {
        StopAllCoroutines();
        _cantChangeChannel = false;
        var newMat = new Material[2];
        newMat[0] = tvMat;
        newMat[1] = channels[_count];
        mesh.materials = newMat;
        _tvOn = true;
    }

    public void TVOff()
    {
        StopAllCoroutines();
        _cantChangeChannel = false;
        var newMat = new Material[2];
        newMat[0] = tvMat;
        newMat[1] = tvOffMat;
        mesh.materials = newMat;
        _tvOn = false;
    }
    

    public void OnInteractItem()
    {
        ChangeMaterial();
    }
}
