using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class IllusionDemon_FogAttack : MonoBaseState
{
    [SerializeField] private IllusionDemon owner;
    private Vector3 _position;
    private Quaternion _rotation;
    private bool _bossMoved, _stateFinish;
    
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
        _position = owner.transform.position;
        _rotation = owner.transform.rotation;
        owner.transform.position = new Vector3(1000, owner.transform.position.y, 1000);
        owner.actualCopies = owner.copiesPerAttack;
        Player.Instance.sphere.SetActive(true);
        owner.StartFogAttack();
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        owner.firstPhase = false;
        owner.secondPhase = false;
        owner.EndFogAttack();
        owner.Anim.cast = false;
        _bossMoved = false;
        owner.Anim.jumpAttack = false;
        Player.Instance.sphere.SetActive(false);
        _stateFinish = false;
        return base.Exit(to);
    }

    public override void UpdateLoop()
    {
        owner.EnemyIsMoving();
        if (owner.actualCopies > 0) return;
        if(!_bossMoved)
        {
            owner.transform.position = owner.MoveBoss();
            owner.transform.rotation = _rotation;
            _bossMoved = true;
        }

        if (Vector3.Distance(owner.CharacterPos.position, owner.transform.position) < owner.rangeForJumpAttack)
        {
            
            owner.Anim.run = false;
            owner.Anim.jumpAttack = true;
        }
        else
        {
            if(owner.Anim.run) transform.position += owner.transform.forward * (owner.speedRun * Time.deltaTime);
            owner.transform.LookAt(new Vector3(owner.CharacterPos.position.x, owner.transform.position.y, owner.CharacterPos.position.z));
            owner.Anim.run = true;
        }
        
        if(owner.Anim.Animator.GetCurrentAnimatorStateInfo(0).IsName("BossJumpAttack") && 
           owner.Anim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .8f)
        {
            _stateFinish = true;
            Player.Instance.sphere.GetComponent<FogPlayer>().start = false;
        }
    }
}
