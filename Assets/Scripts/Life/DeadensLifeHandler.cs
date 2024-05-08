using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadensLifeHandler : LifeHandler
{
    public override void OnTakeDamage(int damage)
    {
        _actualLife -= damage;

        if (_actualLife > 0) return;
                
        GetComponent<Deadens>().Animator.SetBool("Death",true);
        GetComponent<Deadens>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
    }
}
