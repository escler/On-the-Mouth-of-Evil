using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class IllusionDemon_ChannelAttack : MonoBaseState
{
    private float _timeForAttack;
    [SerializeField] private IllusionDemon owner;
    private bool _stateFinish;

    public override IState ProcessInput()
    {
        if (owner.canBanish && Transitions.ContainsKey(StateTransitions.ToBanish))
            return Transitions[StateTransitions.ToBanish];
        
        if (owner.hitCount >= 3 && Transitions.ContainsKey(StateTransitions.ToHit))
            return Transitions[StateTransitions.ToHit];
        
        if (_stateFinish && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];
        
        return this;
    }
    
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        _timeForAttack = owner.timeForChannelAttack;
        owner.transform.position = owner.NewLocation();
        owner.canHit = true;
        owner.Anim.castFireball = true;
        owner.SpawnExplosionCopies(-10, -1);
        owner.SpawnExplosionCopies(0, 10);
        owner.startCast.SetActive(true);
        owner.EnemyIsMoving();
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        owner.startCast.SetActive(false);
        owner.Anim.castFireball = false;
        owner.canHit = false;
        _stateFinish = false;
        return base.Exit(to);
    }

    public override void UpdateLoop()
    {
        owner.transform.LookAt(owner.CharacterPos);
        _timeForAttack -= Time.deltaTime;
        if (_timeForAttack > 0) return;

        if (owner.Anim.Animator.GetCurrentAnimatorStateInfo(0).IsName("CastFireball") &&
            owner.Anim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .8f)
        {
            _stateFinish = true;
        }
    }

}
