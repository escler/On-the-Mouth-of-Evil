using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cross : Item
{
    private float _currentTime, _actualCdTime;
    public float neededTime, coolDown;
    private bool crossUsed, holdingPSActive;
    public ParticleSystem crossExplosion, holdingPS;
    public override void OnInteract(bool hit, RaycastHit i)
    {
        base.OnInteract(hit, i);

        if (crossUsed) return;
        _currentTime += Time.deltaTime;
        if (!holdingPSActive)
        {
            holdingPS.Play();
            holdingPSActive = true;
        }

        if (_currentTime >= neededTime)
        {
            CheckRoom();
            holdingPS.Stop();
            holdingPSActive = false;

            _currentTime = 0;
        }
    }

    private void Update()
    {
        if (!crossUsed) return;

        _actualCdTime += Time.deltaTime;

        if (_actualCdTime >= coolDown)
        {
            crossUsed = false;
            _actualCdTime = 0;
        }
    }

    public void CheckRoom()
    {
        var playerRoom = PlayerHandler.Instance.actualRoom;
        if (playerRoom.cantBlock) return;
        crossExplosion.Play();
        crossUsed = true;
        
        HouseEnemy.Instance.crossRoom = PlayerHandler.Instance.actualRoom;
        HouseEnemy.Instance.CheckRoom();
    }

    public void OnUpCross()
    {
        if (Input.GetMouseButtonUp(0))
        {
            holdingPS.Stop();
            holdingPSActive = false;
            _currentTime = 0;
        }
    }
}
