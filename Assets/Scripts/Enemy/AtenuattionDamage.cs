using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtenuattionDamage : MonoBehaviour
{
    public float attenuationPart;

    public void TakeDamage(int damage, int force, int hitCount)
    {
        var newDamage = Mathf.RoundToInt(damage * attenuationPart);
        print(gameObject.name + " " + newDamage);
        GetComponentInParent<LifeHandler>().TakeDamage(newDamage, force, hitCount);
    }
}
