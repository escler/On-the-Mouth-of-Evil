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
       canInteractWithItem = ObjectDetector.Instance.InteractText();
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

    public override void OnUpdate()
    {
        base.OnUpdate();
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();
        canInteractWithItem = CanInteractWithItem();
        ChangeCrossHair();
    }
    
    public override bool CanInteractWithItem()
    {
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();
        
        if (rayConnected && ObjectDetector.Instance.InteractText()) return true;

        return false;
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
        float random = Random.Range(0,0.6f);
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
        MusicManager.Instance.PlaySound("Broke Rosary", false);


        _broke = true;
    }

}
