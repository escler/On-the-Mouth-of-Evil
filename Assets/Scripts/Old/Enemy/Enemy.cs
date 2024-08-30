using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IGridEntity, IBanishable
{
    public static Enemy Instance { get; private set; }
    private LifeHandler _life;
    public LifeHandler Life => _life;
    public int banishAmount = 20;
    private int _amount;
    public EnemyType enemyType;

    public Vector3 goalPosition;
    public bool bibleBurning;
    protected void OnAwake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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

    public void SetGoalPos(Vector3 position)
    {
        goalPosition = position;
        bibleBurning = true;
    }
}

public enum EnemyType
{
    Normal,
    Boss
}