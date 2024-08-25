using System.Collections;
using System.Collections.Generic;
using FSM;
using Unity.Mathematics;
using UnityEngine;

public class IllusionDemon_Hit : MonoBaseState
{

    [SerializeField] private IllusionDemon owner;

    private bool _stateFinish;
    public override IState ProcessInput()
    {
        if (owner.canBanish && Transitions.ContainsKey(StateTransitions.ToBanish))
            return Transitions[StateTransitions.ToBanish];

        if (_stateFinish && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];

        return this;
    }
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        owner.Anim.hit = true;
        if (owner.copy1.activeInHierarchy) owner.copy1.SetActive(false);
        if (owner.copy2.activeInHierarchy) owner.copy2.SetActive(false);
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        owner.Anim.hit = false;
        owner.enemyHit = false;
        _stateFinish = false;
        owner.hitCount = 0;
        owner._model.transform.localRotation = quaternion.identity;
        return base.Exit(to);
    }

    public override void UpdateLoop()
    {
        owner.EnemyIsMoving();
        if(owner.Anim.Animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") && 
           owner.Anim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .8f)
        {
            _stateFinish = true;
        }
    }

}
