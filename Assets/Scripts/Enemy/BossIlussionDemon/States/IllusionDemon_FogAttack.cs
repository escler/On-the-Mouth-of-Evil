using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemon_FogAttack : State
{
    private IllusionDemon _d;
    private Vector3 _position;
    private Quaternion _rotation;
    private float _actualTime;
    public IllusionDemon_FogAttack(EnemySteeringAgent e)
    {
        _d = e.GetComponent<IllusionDemon>();
    }
    public override void OnEnter()
    {
        _actualTime = 2f;
        _position = _d.transform.position;
        _rotation = _d.transform.rotation;
        _d.transform.position = new Vector3(1000, _d.transform.position.y, 1000);
        _d.actualCopies = _d.copiesPerAttack;
        Player.Instance.sphere.SetActive(true);
        _d.StartFogAttack();
    }

    public override void OnUpdate()
    {
        if (_d.actualCopies > 0) return;
        
        Player.Instance.sphere.GetComponent<FogPlayer>().start = false;
        
        _actualTime -= Time.deltaTime;
        if(_actualTime < 0) _d.ChangeToIdle();
    }

    public override void OnExit()
    {
        _d.transform.position = _position;
        _d.transform.rotation = _rotation;
        _d.EndFogAttack();
        _d.Anim.cast = false;
        Player.Instance.sphere.SetActive(false);
        
    }
}
