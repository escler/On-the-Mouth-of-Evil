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
        if (Enemy.Instance != null) targetEnemy = Enemy.Instance.transform;
        if (PlayerHandler.Instance != null) targetPlayer = PlayerHandler.Instance.transform;
        _vfx = GetComponent<VisualEffect>();
    }

    void Update()
    {
        UpdateEnemyPos();
        UpdatePlayerPos();
    }

    void UpdateEnemyPos()
    {
        if (targetEnemy == null) return;
        _vfx.SetVector3("Transform", targetEnemy.position);
    }
    
    void UpdatePlayerPos()
    {
        if (targetPlayer == null) return;
        _vfx.SetVector3("AtractTarget", targetPlayer.position);
    }
}
