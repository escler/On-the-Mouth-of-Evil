using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidAnim : MonoBehaviour
{
    public void ActivateRigid()
    {
        GetComponentInParent<Rigidbody>().isKinematic = false;
    }

    public void FireNest()
    {
        NestHandler.Instance.FireMainNest();
    }
}
