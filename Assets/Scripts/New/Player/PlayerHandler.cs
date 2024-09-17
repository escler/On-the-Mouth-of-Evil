using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler Instance { get; private set; }

    public PlayerMovement movement;
    public PlayerCam playerCam;
    public BobbingCamera bobbingCamera;
    public Transform handPivot, cameraPos, puzzlePivot;
    public Mission actualMission;
    public Room actualRoom;
    public bool cantPressInventory;


    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
        Instance = this;
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
    }


    public void PossesPlayer()
    {
        movement.enabled = true;
        playerCam.enabled = true;
        bobbingCamera.enabled = true;
    }

    public void ChangePlayerPosses(bool state)
    {
        movement.enabled = state;
        playerCam.enabled = state;
        bobbingCamera.enabled = state;
    }

    public void UnPossesPlayer()
    {
        movement.enabled = false;
        playerCam.enabled = false;
        bobbingCamera.enabled = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 16) return;

        actualRoom = other.GetComponent<Room>();
    }
    
}
