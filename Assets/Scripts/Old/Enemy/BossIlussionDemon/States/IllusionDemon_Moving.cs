using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;
using UnityEngine.UI;

public class IllusionDemon_Moving : MonoBaseState
{
    [SerializeField] private IllusionDemon owner;
    private float _yDirection, _speed, _actualTimer, nextAttack;

    /*public override void OnEnter()
    {
        CalculateDirection();
        _d.Anim.moving = true;
        _speed = _d.speedWalk;
        var randomNum = Random.Range(1, 3);
        _actualTimer = 1 + randomNum;
    }


    public override void OnUpdate()
    {
        _d.EnemyIsMoving();
        _d.transform.LookAt(new Vector3(_d.CharacterPos.position.x, _d.transform.position.y, _d.CharacterPos.position.z));
        _d.transform.position += _d.transform.right * (_yDirection * _speed * Time.deltaTime);

        _actualTimer -= Time.deltaTime;

        if (_actualTimer <= 0)
        {
            _d.DecisionTree.Execute(_d);
        }
    }

    public override void OnExit()
    {
        _d.Anim.moving = false;
    }*/
    
    private void CalculateDirection()
    {
        _yDirection = Random.Range(-1, 2);
        if(_yDirection == 0) CalculateDirection();
        owner.Anim.yAxis = _yDirection;
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        CalculateDirection();
        owner.Anim.moving = true;
        _speed = owner.speedWalk;
        var randomNum = Random.Range(1, 3);
        _actualTimer = 1 + randomNum;
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        owner.Anim.moving = false;
        return base.Exit(to);
    }

    public override void UpdateLoop()
    {
        owner.EnemyIsMoving();
        owner.transform.LookAt(new Vector3(owner.CharacterPos.position.x, owner.transform.position.y, owner.CharacterPos.position.z));
        owner.transform.position += owner.transform.right * (_yDirection * _speed * Time.deltaTime);

        if(_actualTimer > 0) _actualTimer -= Time.deltaTime;
    }

    public override IState ProcessInput()
    {
        if (owner.canBanish && Transitions.ContainsKey(StateTransitions.ToBanish))
            return Transitions[StateTransitions.ToBanish];

        if (_actualTimer <= 0)
        {
            if (owner.firstPhase || owner.secondPhase && Transitions.ContainsKey(StateTransitions.ToFogAttack))
            {
                owner.lastsStateTransitionAttack = StateTransitions.ToFogAttack;
                return Transitions[StateTransitions.ToFogAttack];
            }
            
            nextAttack = GetRandomNumber();
            if (nextAttack < 3 && owner.lastsStateTransitionAttack != StateTransitions.ToChannelAttack &&
                Transitions.ContainsKey(StateTransitions.ToChannelAttack))
            {
                owner.lastsStateTransitionAttack = StateTransitions.ToChannelAttack;
                return Transitions[StateTransitions.ToChannelAttack];
            }
            
            if (nextAttack < 6 && owner.lastsStateTransitionAttack != StateTransitions.ToJumpAttack &&
                Transitions.ContainsKey(StateTransitions.ToJumpAttack))
            {
                owner.lastsStateTransitionAttack = StateTransitions.ToJumpAttack;
                return Transitions[StateTransitions.ToJumpAttack];
            }
            
            if (nextAttack < 9 && owner.lastsStateTransitionAttack != StateTransitions.ToThrowAttack &&
                Transitions.ContainsKey(StateTransitions.ToThrowAttack))
            {
                owner.lastsStateTransitionAttack = StateTransitions.ToThrowAttack;
                return Transitions[StateTransitions.ToThrowAttack];
            }
        }
        return this;
    }

    public float GetRandomNumber()
    {
        return Random.Range(0, 10);
    }
}
