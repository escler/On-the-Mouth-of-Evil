using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon_JumpAttack : State
{
    private IllusionDemon d;
    private float _durationAnim;
    
    public IllusionDemon_JumpAttack(EnemySteeringAgent e)
    {
        d = e.GetComponent<IllusionDemon>();
    }
    public override void OnEnter()
    {
        d.Anim.castCopies = true;
    }

    public override void OnUpdate()
    {
        d.transform.LookAt(new Vector3(d.CharacterPos.position.x, d.transform.position.y, d.CharacterPos.position.z));
        if(d.finishCast)
        {
            d.Anim.run = true;
            d.copy1.GetComponentInChildren<BossDuplicationMovement>().ChangeRun(true);
            d.copy2.GetComponentInChildren<BossDuplicationMovement>().ChangeRun(true);
            d.finishCast = false;
        }

        if (d.Anim.run) d.transform.position += d.transform.forward * (d.speedRun * Time.deltaTime);
        if (Vector3.Distance(d.CharacterPos.position, d.transform.position) < d.rangeForSpecialAttack)
        {
            
            d.Anim.run = false;
            d.Anim.jumpAttack = true;
        }
        
        if(d.Anim.Animator.GetCurrentAnimatorStateInfo(0).IsName("BossJumpAttack") && 
           d.Anim.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= .8f)
        {
            d.DecisionTree.Execute(d);
        }
    }

    public override void OnExit()
    {
        d.Anim.jumpAttack = false;
        d.Anim.castCopies = false;
        d.Anim.run = false;
        d.finishCast = false;
    }
}
