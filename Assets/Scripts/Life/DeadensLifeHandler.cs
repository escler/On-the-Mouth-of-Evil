using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadensLifeHandler : LifeHandler
{
    public override void OnTakeDamage(int damage)
    {
        base.OnTakeDamage(damage);
                
        GetComponent<Deadens>().Animator.SetBool("Death",true);
        GetComponent<Deadens>().enabled = false;
    }
}
