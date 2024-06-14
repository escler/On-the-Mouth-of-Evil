using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon_ThrowObjects : State
{
    private IllusionDemon _d;
    private float _actualTime;
    private ThrowItem _item;
    public IllusionDemon_ThrowObjects(EnemySteeringAgent e)
    {
        _d = e.GetComponent<IllusionDemon>();
    }
    public override void OnEnter()
    {
        _actualTime = 3;
        _item = ThrowManager.Instance.MoveToLocation(_d.throwObjectPos.position);
    }

    public override void OnUpdate()
    {
        if (_item == null)
        {
            _d.ChangeToIdle();
            return;
        }

        if(_item._callBackHit) _d.ChangeToIdle();
    }

    public override void OnExit()
    {
        _item = null;
    }
}
