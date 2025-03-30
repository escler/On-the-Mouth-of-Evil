using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cross : Item
{
    private float _currentTime, _actualCdTime;
    public float neededTime, coolDown;
    private bool crossUsed, holdingPSActive;
    public ParticleSystem[] crossExplosion, holdingPS;
    private CrossCD _crossCd;
    public GameObject CrossLight;
    private int index;
    public AudioSource _crossSound;

    private void Start()
    {
        _crossCd = PlayerHandler.Instance.GetComponent<CrossCD>();
    }

    public override void OnGrabItem()
    {
        base.OnGrabItem();
        var inventoryHub = Inventory.Instance.hubInventory;
        for (int i = 0; i < inventoryHub.Length; i++)
        {
            if(inventoryHub[i] == null) continue;
            if (inventoryHub[i] == this)
            {
                InventoryUI.Instance.fillGO.transform.GetChild(i).GetComponent<SliderUI>().SubscribeToCrossEvent();
                index = i;
                break;
            }
        }
        transform.localEulerAngles = angleHand;
        
        if (SceneManager.GetActiveScene().name == "Hub") return;
        
        SortInventoryBuyHandler.Instance.AddItemToHandler(this);
    }

    public override void OnDropItem()
    {
        base.OnDropItem();
        InventoryUI.Instance.fillGO.transform.GetChild(index).GetComponent<SliderUI>().UnSubscribeToCrossEvent();
        
        if (SceneManager.GetActiveScene().name == "Hub") return;
        
        SortInventoryBuyHandler.Instance.RemoveItemFromHandler(this);
    }

    public override void OnInteract(bool hit, RaycastHit i)
    {
        base.OnInteract(hit, i);

        if(PlayerHandler.Instance.movingObject) OnUpCross();
        if (hit && i.transform.TryGetComponent(out SkullPuzzleSlot socket)) return;
        if (_crossCd.cantUse) return;
        _currentTime += Time.deltaTime;
        print(_currentTime);
        if (!holdingPSActive)
        {
            foreach (var holdPS in holdingPS)
            {
                holdPS.Play();
                _crossSound.Play();
                CrossLight.SetActive(true);
            }
            holdingPSActive = true;
        }

        if (_currentTime >= neededTime)
        {
            CheckRoom();
            foreach (var holdPS in holdingPS)
            {
                holdPS.Stop();
                holdPS.Clear();
                CrossLight.SetActive(false);
            }
            holdingPSActive = false;

            _currentTime = 0;
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();
        ChangeCrossHair();

        if (Input.GetMouseButton(0)) OnInteract(rayConnected, ray);
        if(Input.GetMouseButtonUp(0)) OnUpCross();
    }

    public void CheckRoom()
    {
        var playerRoom = PlayerHandler.Instance.actualRoom;
        if (playerRoom == null) return;
        if (playerRoom.cantBlock) return;
        foreach (var explosionPS in crossExplosion)
        {
            explosionPS.Play();
        }
        _crossCd.cantUse = true;
        _crossCd.SetCooldown(0);
        
        HouseEnemy.Instance.crossRoom = PlayerHandler.Instance.actualRoom;
        HouseEnemy.Instance.CheckRoom();
    }

    public void OnUpCross()
    {
        foreach (var holdPS in holdingPS)
        {
            holdPS.Stop();
            holdPS.Clear();
        }
        holdingPSActive = false;
        CrossLight.SetActive(false);
        _currentTime = 0;
    }
}
