using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class HouseEnemy_Spawn : MonoBaseState
{
    [SerializeField] private HouseEnemy owner;
    private bool stateEnd;
    private bool dontAttack;
    public override void UpdateLoop()
    {
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        print("Entre a Spawn");
        if (owner.actualTime < owner.timeToShowMe)
        {
            dontAttack = true;
            return;
        }
        stateEnd = false;
        StartCoroutine(ShowEnemyLerp());
    }

    public override IState ProcessInput()
    {
        if (owner.voodooActivate && stateEnd && Transitions.ContainsKey(StateTransitions.ToVoodoo))
            return Transitions[StateTransitions.ToVoodoo];
        
        if (dontAttack && stateEnd && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];
        
        if (stateEnd && Transitions.ContainsKey(StateTransitions.ToAttacks))
            return Transitions[StateTransitions.ToAttacks];

        return this;
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        dontAttack = false;
        stateEnd = false;
        StopAllCoroutines();    
        owner.EnemyAnimator.ChangeStateAnimation("Spawn", false);
        return base.Exit(to);
    }
    
    IEnumerator ShowEnemyLerp()
    {
        if (owner.enemyVisibility >= 8)
        {
            stateEnd = true;
            yield break;
        }
        print("Coroutina");
        owner.EnemyAnimator.ChangeStateAnimation("Spawn", true);
        yield return new WaitForSeconds(0.01f);
        owner.EnemyAnimator.ChangeStateAnimation("Spawn", false);
        foreach (var ps in owner.smokePS)
        {
            ps.Play();
        }
        while (owner.enemyVisibility < 8)
        {
            owner.enemyVisibility += .2f;
            owner.enemyMaterial.SetFloat("_Power", owner.enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }

        owner.actualTimeToLost = 4;

        owner.enemyVisible = true;
        stateEnd = true;
    }
}
