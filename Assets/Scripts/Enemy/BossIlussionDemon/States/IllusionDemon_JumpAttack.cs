using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class IllusionDemon_JumpAttack : MonoBaseState
{
    private float _durationAnim;
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
        owner.Anim.castCopies = true;
        owner.transform.position = owner.LocationForJumpAttack();
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        owner.Anim.jumpAttack = false;
        owner.Anim.castCopies = false;
        owner.Anim.run = false;
        owner.finishCast = false;
        _stateFinish = false;
        return base.Exit(to);
    }

    public override void UpdateLoop()
    {
        owner.EnemyIsMoving();
        
        if(owner.finishCast)
        {
            owner.Anim.run = true;
            owner.copy1.GetComponentInChildren<BossDuplicationMovement>().ChangeRun(true);
            owner.copy2.GetComponentInChildren<BossDuplicationMovement>().ChangeRun(true);
            owner.finishCast = false;
        }
        
        if(!owner.Anim.jumpAttack)
            owner.transform.LookAt(new Vector3(owner.CharacterPos.position.x, owner.transform.position.y, owner.CharacterPos.position.z));

        if (owner.Anim.run) owner.transform.position += owner.transform.forward * (owner.speedRun * Time.deltaTime);
        if (Vector3.Distance(owner.CharacterPos.position, owner.transform.position) < owner.rangeForJumpAttack)
        {
            owner.Anim.run = false;
            owner.Anim.jumpAttack = true;
        }
        
        if(owner.Anim.Animator.GetCurrentAnimatorStateInfo(0).IsName("BossJumpAttack") && 
           owner.Anim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .8f)
        {
            _stateFinish = true;
        }
    }
}
