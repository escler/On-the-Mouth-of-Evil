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
    public bool toRitual;

    private void Awake()
    {
        if (!toRitual)
        {
            if (PlayerHandler.Instance != null) targetPlayer = PlayerHandler.Instance.particlePivot;
        }
        else
        {
            if(BadRitual.Instance != null) targetPlayer = BadRitual.Instance.absorbPoint;
        }
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
