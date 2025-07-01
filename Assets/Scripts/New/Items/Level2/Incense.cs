using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Incense : Item
{
    [SerializeField] ParticleSystem particle;
    private bool _incenseActivated;

    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (canInteractWithItem) return;
        if (_incenseActivated) return;
        var emit = particle.emission;
        emit.rateOverTime = 5;
        _incenseActivated = true;
        PlayerHandler.Instance.IncenseActivate();
        StartCoroutine(MoveToPivot());
    }

    IEnumerator MoveToPivot()
    {
        Inventory.Instance.DropItem(this, Inventory.Instance.countSelected);
        var initial = transform.position;
        var target = PlayerHandler.Instance.incensePivot.position;

        float time = 0;
        while (time < 1)
        {
            transform.position = Vector3.Lerp(initial,target,time);
            time += Time.deltaTime;
            yield return null;
        }
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        transform.SetParent(PlayerHandler.Instance.incensePivot.transform);
        transform.localPosition = Vector3.zero;
    }

    private void DestroyPlayer(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (!_incenseActivated) return;
        Destroy(gameObject);
    }
    void Awake()
    {
        var emit = particle.emission;
        emit.rateOverTime = 0;
        SceneManager.sceneLoaded += DestroyPlayer;
    }

    public void OnDestroy()
    {
        SceneManager.sceneLoaded -= DestroyPlayer;
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

    private void Update()
    {
        if (!_incenseActivated) return;
        
        var emit = particle.emission;
        emit.rateOverTime = PlayerHandler.Instance.incenseProtect ? 5 : 0;
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
        if (GetComponentInChildren<SkinnedMeshRenderer>() == null)
        {
            GetComponentInChildren<MeshRenderer>().gameObject.layer = 18;
        }
        else
        {
            GetComponentInChildren<SkinnedMeshRenderer>().gameObject.layer = 18;
        }
        transform.localEulerAngles = angleHand;
        Inventory.Instance.AddSpecialItem(this);
        SortInventoryBuyHandler.Instance.SaveCount(itemName, true);
    }

    public override void OnDropItem()
    {
        gameObject.SetActive(true);
        GetComponentInChildren<MeshRenderer>().gameObject.layer = 2;
        SortInventoryBuyHandler.Instance.SaveCount(itemName, false);
    }
}
