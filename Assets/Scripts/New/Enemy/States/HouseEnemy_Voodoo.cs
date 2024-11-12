using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class HouseEnemy_Voodoo : MonoBaseState
{
    [SerializeField] private HouseEnemy owner;

    public float timeStunned;
    private float _actualTime;

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        var target = owner.voodooPosition;
        target.y = transform.position.y;
        transform.position = target;
        _actualTime = timeStunned;
        StartCoroutine(ShowEnemyLerp());
        owner.EnemyAnimator.ChangeStateAnimation("Voodoo", true);
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        owner.HideEnemy();
        owner.EnemyAnimator.ChangeStateAnimation("Voodoo", false);
        owner.voodooActivate = false;
        return base.Exit(to);
    }

    
    public override void UpdateLoop()
    {
        _actualTime -= Time.deltaTime;
        owner.actualTime = 0;
    }

    public override IState ProcessInput()
    {
        if (owner.ritualDone && Transitions.ContainsKey(StateTransitions.ToRitual))
            return Transitions[StateTransitions.ToRitual];
        
        if (_actualTime <= 0 && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];
        
        return this;
    }
    
    IEnumerator ShowEnemyLerp()
    {
        while (owner.enemyVisibility < 8)
        {
            owner.enemyVisibility += .2f;
            owner.enemyMaterial.SetFloat("_Power", owner.enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }
        owner.enemyVisible = true;
    }
}
