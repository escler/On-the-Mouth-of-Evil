using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm : Item
{
    [SerializeField] private float cdTime;
    private float _actualTime;
    public GameObject swarmObj, swarmAround;
    public Transform swarmPivot, starPivot;
    public float speedSwarm;
    public Animator animator;
    public GameObject swarmPsIdle;
    
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
        Inventory.Instance.AddSpecialItem(this);
    }

}
