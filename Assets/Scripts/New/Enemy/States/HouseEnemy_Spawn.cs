using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class HouseEnemy_Spawn : MonoBaseState
{
    [SerializeField] private HouseEnemy owner;
    private bool stateEnd;
    public override void UpdateLoop()
    {
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        print("Entre a Spawn");
        stateEnd = false;
        StartCoroutine(ShowEnemyLerp());
    }

    public override IState ProcessInput()
    {
        if (stateEnd && Transitions.ContainsKey(StateTransitions.ToAttacks))
            return Transitions[StateTransitions.ToAttacks];

        return this;
    }
    
    IEnumerator ShowEnemyLerp()
    {
        owner.EnemyAnimator.ChangeStateAnimation("Spawn", true);
        while (owner.enemyVisibility < 8)
        {
            owner.enemyVisibility += .2f;
            owner.enemyMaterial.SetFloat("_Power", owner.enemyVisibility);
            yield return new WaitForSeconds(0.1f);
        }
        owner.EnemyAnimator.ChangeStateAnimation("Spawn", false);


        owner.actualTimeToLost = 4;

        owner.trailPS.SetActive(true);
        owner.enemyVisible = true;
        stateEnd = true;
    }
}
