using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour, IInteractable
{
    public int bulletAmount;
    public GunType gunType;

    public void OnInteract()
    {
        AmmoHandler.Instance.AddBullet(gunType, bulletAmount);
        gameObject.SetActive(false);
    }
}
