using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class Rosary : Item
{
    public float chanceToProtect;
    [SerializeField] private List<SkinnedMeshRenderer> meshes;
    public Animator beadAnimator;
    [SerializeField] List<VisualEffect> particles = new List<VisualEffect>();
    private float _timeTobroke;
    private bool _broke;

    void Awake()
    {
        meshes = new List<SkinnedMeshRenderer>();
        meshes = GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
        beadAnimator = GetComponentInChildren<Animator>();
        beadAnimator.SetBool("used", false);
        
    }

    private void Update()
    {
       if(!_broke) return;
       
       _timeTobroke += Time.deltaTime;
       if (_timeTobroke >= 1.5f)
       {
           Inventory.Instance.DropItem(Inventory.Instance.selectedItem, Inventory.Instance.countSelected);
           Destroy(gameObject);
           _broke = false;
       }
    }

    public override void OnGrabItem()
    {
        Inventory.Instance.AddSpecialItem(this);
        foreach (SkinnedMeshRenderer smr in meshes)
        {
            smr.gameObject.layer = 18;
        }
        if (SceneManager.GetActiveScene().name == "Hub") return;
        
        SortInventoryBuyHandler.Instance.SaveCount(itemName, true);

    }
    
    public override void OnDropItem()
    {
        gameObject.SetActive(true);
        foreach (SkinnedMeshRenderer smr in meshes)
        {
            smr.gameObject.layer = 1;
        }
        
        if (SceneManager.GetActiveScene().name == "Hub") return;
        
        SortInventoryBuyHandler.Instance.SaveCount(itemName, false);

    }

    public bool RosaryProtect()
    {
        float random = Random.Range(0, 1);
        bool success = random <= chanceToProtect;
        BreakRosary();
        
        return success;
    }

    private void BreakRosary()
    {
        beadAnimator.SetBool("used", true);
        foreach (VisualEffect item in particles)
        {
            item.SendEvent("PlayPS");
        }

        _broke = true;
    }

}
