using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bible : Item, IBurneable
{
    private bool _placed;
    public float timeToBurn;
    public GameObject firePS;
    private MaterialPropertyBlock _burning;
    private MeshRenderer _mesh;
    private void Awake()
    {
        _burning = new MaterialPropertyBlock();
        _mesh = GetComponentInChildren<MeshRenderer>();
        _mesh.SetPropertyBlock(_burning);
        _burning.SetInt("_BurningON", 0);
    }

    public void OnBurn()
    {
        if (!_placed) return;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        if(Enemy.Instance != null) Enemy.Instance.SetGoalPos(transform.position);
        StartCoroutine(BibleBurning());        
        _burning.SetInt("_BurningON", 1);
        _mesh.SetPropertyBlock(_burning);

    }

    public override void OnInteract(bool hit, RaycastHit i)
    {
        Inventory.Instance.DropItem(this, Inventory.Instance.countSelected);
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
