using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class IllusionDuplications : EnemySteeringAgent
{
    private FiniteStateMachine _fsm;
    private Transform _characterPos;
    private BossDuplicationAnim _anim;
    [SerializeField] private DecisionNode _decisionTree;


    public float speedWalk, speedRun;
    public bool canHit;
    public float rangeForAttack;
    public actionsEnemy lastAction;


    public DecisionNode DecisionTree => _decisionTree;
    public BossDuplicationAnim Anim => _anim;
    
    private void Awake()
    {
        _fsm = new FiniteStateMachine();
        _characterPos = Player.Instance.transform;
        _anim = GetComponentInChildren<BossDuplicationAnim>();
        _fsm.AddState(States.Idle, new IllusionDemon_Idle(this));
        _fsm.AddState(States.Moving, new IllusionDemon_Moving(this));
        _fsm.AddState(States.Attack, new IllusionDemon_ComboHit(this));
        
        _fsm.ChangeState(States.Idle);
    }
    
    public void ChangeToMove()
    {
        _fsm.ChangeState(States.Moving);
    }

    public void ChangeToAttack()
    {
        _fsm.ChangeState(States.Attack);
    }
}
