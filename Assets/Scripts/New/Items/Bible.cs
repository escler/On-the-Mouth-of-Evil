using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bible : Item, IBurneable
{
    private bool _placed;
    public float timeToBurn;
    public void OnBurn()
    {
        if (!_placed) return;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        Enemy.Instance.SetGoalPos(transform.position);
        StartCoroutine(BibleBurning());
    }

    public override void OnInteract(bool hit, RaycastHit i)
    {
        Inventory.Instance.DropItem();
        _placed = true;
        base.OnInteract(hit,i);
    }

    IEnumerator BibleBurning()
    {
        while (timeToBurn > 0)
        {
            timeToBurn -= 1;

            yield return new WaitForSeconds(1f);
        }

        Enemy.Instance.bibleBurning = false;
        Destroy(gameObject);
    }
}
