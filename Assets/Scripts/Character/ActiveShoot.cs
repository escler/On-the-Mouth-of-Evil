using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveShoot : MonoBehaviour
{
    public void ShootStart()
    {
        GetComponentInParent<AnimPlayer>().Shooting = true;
    }

    public void ShootEnd()
    {
        GetComponentInParent<AnimPlayer>().Shooting = false;
    }

    public void WeaponFeedback()
    {
        WeaponsHandler.Instance.FeedbackShoot();
    }
}
