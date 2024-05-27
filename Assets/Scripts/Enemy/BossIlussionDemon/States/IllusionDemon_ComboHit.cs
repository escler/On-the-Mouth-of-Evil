using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon_ComboHit : State
{
    private IllusionDemon d;
    public IllusionDemon_ComboHit(EnemySteeringAgent e)
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

        if (Vector3.Distance(d.CharacterPos.position, d.transform.position) < d.rangeForAttack)
        {
            d.transform.LookAt(new Vector3(d.CharacterPos.position.x, d.transform.position.y, d.CharacterPos.position.z));
            d.Anim.run = false;
            d.Anim.comboHit = true;
        }
    }

    public override void OnExit()
    {
        
    }
}
