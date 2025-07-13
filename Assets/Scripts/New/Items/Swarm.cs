using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Swarm : Item
{
    [SerializeField] private float cdTime;
    private float _actualTime;
    public GameObject swarmObj, swarmAround;
    public Transform swarmPivot, starPivot;
    public float speedSwarm;
    public Animator animator;
    public GameObject swarmPsIdle;
    public MeshRenderer[] meshRenderers;
    public AudioSource beeLoop;
    private bool _used;
    
    private void Awake()
    {
        _actualTime = 0;
    }

    private void Update()
    {
        if (_actualTime < 0)
        {
            if(!swarmPsIdle.activeInHierarchy) swarmPsIdle.SetActive(true);
            return;
        }
        
        _actualTime -= Time.deltaTime;
    }

    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (_actualTime > 0) return;
        if (canInteractWithItem) return;
        if (Enemy.Instance == null) return;
        
        Inventory.Instance.cantSwitch = true;
        StartCoroutine(LocateAndMove());
        _actualTime = cdTime;
    }

    private IEnumerator LocateAndMove()
    {
        animator.SetTrigger("Open");
        yield return new WaitForSeconds(1f);
        var goal = Enemy.Instance.transform.position;
        var swarm = Instantiate(swarmObj);
        swarm.transform.position = swarmPivot.position;
        swarmPsIdle.gameObject.SetActive(false);
        _used = true;
        beeLoop.Stop();

        Inventory.Instance.DropItem(this, Inventory.Instance.countSelected);
        Inventory.Instance.cantSwitch = false;
        gameObject.layer = 1;

        while (Vector3.Distance(goal, swarm.transform.position) > 0.1f)
        {
            goal = Enemy.Instance.transform.position;
            swarm.transform.LookAt(goal);
            swarm.transform.position += swarm.transform.forward * (Time.deltaTime * speedSwarm);
            yield return null;
        }

        Destroy(swarm);

        var swarmAroundObj = Instantiate(swarmAround);
        swarmAroundObj.transform.SetParent(Enemy.Instance.transform);
        swarmAroundObj.transform.localPosition = Vector3.zero;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();
        canInteractWithItem = CanInteractWithItem();
        ChangeCrossHair();
        if (Input.GetMouseButtonDown(0))
        {
            OnInteract(rayConnected, ray);
        }
    }

    public override bool CanInteractWithItem()
    {
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();
        
        if (rayConnected && ObjectDetector.Instance.InteractText()) return true;

        return false;
    }

    public override void OnGrabItem()
    {
        transform.localEulerAngles = angleHand;
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.gameObject.layer = 18;
        }
        Inventory.Instance.AddSpecialItem(this);
        if (SceneManager.GetActiveScene().name == "Hub") return;
        SortInventoryBuyHandler.Instance.SaveCount(itemName, true);
    }

    public override void OnDropItem()
    {
        gameObject.SetActive(true);
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.gameObject.layer = 1;
        }
        
        if (SceneManager.GetActiveScene().name == "Hub") return;
        
        print("Entre a Drop Incense y descarte el item");
        SortInventoryBuyHandler.Instance.SaveCount(itemName, false);
        
    }

    private void OnEnable()
    {
        if (_used) beeLoop.Stop();
        else beeLoop.Play();
    }
}
