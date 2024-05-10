using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupShotgun : MonoBehaviour,IInteractable
{
    [SerializeField] private RangedWeapon _shotgun;


    public void OnInteract()
    {
        WeaponsHandler.Instance.AddWeapon(_shotgun);
        //EventsManager.Instance.SetCurrentEvent(GetComponentInParent<IEvent>());
        gameObject.SetActive(false);
    }
}
