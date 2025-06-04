using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class MorgueEnemy_Spawn : MonoBaseState
{
    [SerializeField] private MorgueEnemy owner;
    private bool stateEnd;
    private bool dontAttack;
    public override void UpdateLoop()
    {
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        if (owner.enemyVisible) stateEnd = true;
        //if(!owner.EnemyAnimator.spawnAudio.isPlaying)owner.EnemyAnimator.spawnAudio.Play();
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
        print("Sali de spawn");
        dontAttack = false;
        stateEnd = false;
        StopAllCoroutines();    
        //owner.EnemyAnimator.ChangeStateAnimation("Spawn", false);
        return base.Exit(to);
    }
    
    IEnumerator ShowEnemyLerp()
    {
        print("Entre a cor");
        if (owner.enemyVisibility >= 8)
        {
            stateEnd = true;
            yield break;
        }
        //owner.EnemyAnimator.ChangeStateAnimation("Spawn", true);
        yield return new WaitForSeconds(0.01f);
        //owner.EnemyAnimator.ChangeStateAnimation("Spawn", false);
        /*foreach (var ps in owner.smokePS)
        {
            ps.Play();
        }*/
        while (owner.enemyVisibility < 8)
        {
            print("Estoy en el loop");
            owner.enemyVisibility += .2f;
            owner.enemyMaterial.SetFloat("_Power", owner.enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }

        owner.actualTimeToLost = 4;

        owner.enemyVisible = true;
        stateEnd = true;
    }
}
