using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon_JumpAttack : State
{
    private IllusionDemon d;

    
    public IllusionDemon_JumpAttack(EnemySteeringAgent e)
    {
        d = e.GetComponent<IllusionDemon>();
    }
    public override void OnEnter()
    {
        d.Anim.run = true;
    }

    public override void OnUpdate()
    {
        if(d.Anim.run) d.transform.position += d.transform.forward * (d.speedRun * Time.deltaTime);

        if (Vector3.Distance(d.CharacterPos.position, d.transform.position) < d.rangeForSpecialAttack)
        {
            d.transform.LookAt(new Vector3(d.CharacterPos.position.x, d.transform.position.y, d.CharacterPos.position.z));
            d.Anim.run = false;
            d.Anim.jumpAttack = true;
        }
    }

    public override void OnExit()
    {
        d.Anim.jumpAttack = false;
    }
}
