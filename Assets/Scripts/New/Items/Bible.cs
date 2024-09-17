using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bible : Item, IBurneable
{
    private bool _placed;
    public float timeToBurn;
    public GameObject firePS; 
    public void OnBurn()
    {
        if (!_placed) return;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        if(Enemy.Instance != null) Enemy.Instance.SetGoalPos(transform.position);
        StartCoroutine(BibleBurning());
        GetComponentInChildren<MeshRenderer>().material.SetInt("_BurningON", 1);
    }

    public override void OnInteract(bool hit, RaycastHit i)
    {
        Inventory.Instance.DropItem();
        _placed = true;
        base.OnInteract(hit,i);
    }

    IEnumerator BibleBurning()
    {
        firePS.SetActive(true);
        while (timeToBurn > 0)
        {
            timeToBurn -= 1;

            yield return new WaitForSeconds(1f);
        }

        if(Enemy.Instance != null) Enemy.Instance.bibleBurning = false;
        Destroy(gameObject);
    }
}
