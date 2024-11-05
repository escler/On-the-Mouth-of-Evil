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
        if (dontAttack && Transitions.ContainsKey(StateTransitions.ToIdle))
            return Transitions[StateTransitions.ToIdle];
        
        if (stateEnd && Transitions.ContainsKey(StateTransitions.ToAttacks))
            return Transitions[StateTransitions.ToAttacks];

        return this;
    }

    public override Dictionary<string, object> Exit(IState to)
    {
        dontAttack = false;
        stateEnd = false;
        return base.Exit(to);
    }
    
    IEnumerator ShowEnemyLerp()
    {
        owner.EnemyAnimator.ChangeStateAnimation("Spawn", true);
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
        owner.EnemyAnimator.ChangeStateAnimation("Spawn", false);


        owner.actualTimeToLost = 4;

        owner.enemyVisible = true;
        stateEnd = true;
    }
}
