using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IGridEntity, IBanishable
{
    private LifeHandler _life;
    public LifeHandler Life => _life;
    public int banishAmount = 20;
    private int _amount;
    protected void OnAwake()
    {
        _life = GetComponent<LifeHandler>();
    }

    protected void EnemyMove()
    {
        OnMove?.Invoke(this);
    }

    public event Action<IGridEntity> OnMove;
    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    public bool canBanish { get; set; }
    public bool onBanishing { get; set; }
    public bool banished { get; set; }

    public virtual void StartBanish()
    {
    }

    public virtual void FinishBanish()
    {
    }
}