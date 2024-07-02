using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class IllusionDemon_Death : MonoBaseState
{
    [SerializeField] private IllusionDemon owner;
    public override IState ProcessInput()
    {
        return this;
    }
    
    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(from, transitionParameters);
        owner.Anim.death = true;
        GetComponentInChildren<DissolveEnemy>().ActivateDissolve();
        Player.Instance.SkillAdquired = true;
        ObjetivesUI.Instance.BossWin();
        owner.OnBossDefeated?.Invoke();
        owner.DisableFSM();

    }

    public override void UpdateLoop()
    {
    }

}
