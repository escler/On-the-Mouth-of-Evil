using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class HouseEnemy_GrabHead : MonoBaseState
{
    [SerializeField] private HouseEnemy owner;
    
    public override void UpdateLoop()
    {
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        owner.EnemyAnimator.ActivateGrabHead();
        StartCoroutine(WaitAnimState());
    }

    IEnumerator WaitAnimState()
    {
        yield return new WaitUntil(() =>
            owner.EnemyAnimator.animator.GetCurrentAnimatorStateInfo(0).IsName("Grabhead"));
        PlayerHandler.Instance.HeadGrabbed(owner.headPos);
    }
    
    public override IState ProcessInput()
    {
        if (!owner.grabHead && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];

        return this;
    }
}
