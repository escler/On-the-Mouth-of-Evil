using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon_ChannelAttack : State
{
    private float _timeForAttack;
    
    private IllusionDemon _d;
    public IllusionDemon_ChannelAttack(EnemySteeringAgent e)
    {
        _d = e.GetComponent<IllusionDemon>();
    }
    public override void OnEnter()
    {
        _timeForAttack = _d.timeForChannelAttack;
        _d.canHit = true;
        _d.transform.position = _d.NewLocation();
        _d.SpawnExplosionCopies(-10, -1);
        _d.SpawnExplosionCopies(0, 10);
    }

    public override void OnUpdate()
    {
        _d.transform.LookAt(_d.CharacterPos);
        _timeForAttack -= Time.deltaTime;
        _d.startCast.SetActive(true);
        
        if (_timeForAttack > 0) return;
        
        _d.FireSpell();
        _d.ChangeToIdle();
    }

    public override void OnExit()
    {
        _d.canHit = false;
        _d.startCast.SetActive(false);
    }
}
