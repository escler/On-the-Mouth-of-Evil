using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXTarget : MonoBehaviour
{
    public Transform targetEnemy, targetPlayer;
    private Vector3 oldPosEnemy, oldPosPlayer;
    private VisualEffect _vfx;

    private void Awake()
    {
        if (PlayerHandler.Instance != null) targetPlayer = PlayerHandler.Instance.particlePivot;
        _vfx = GetComponent<VisualEffect>();
    }

    void Update()
    {
        UpdateEnemyPos();
        UpdatePlayerPos();
    }

    void UpdateEnemyPos()
    {
    }
    
    void UpdatePlayerPos()
    {
        if (targetPlayer == null) return;
        _vfx.SetVector3("AtractTarget", targetPlayer.position);
    }
}
