using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaperMission1 : Mission
{
    public string interactableText;
    private bool active;
    public Transform cameraPos, handPos;
    private Vector3 reference = Vector3.zero;
    private float _sensX, _sensY;
    private PlayerCam _playerCam;
    private bool canInteract;
    public float offset;
    private bool contentActive;

    private void Start()
    {
        active = false;
        cameraPos = PlayerHandler.Instance.cameraPos;
        handPos = PlayerHandler.Instance.handPivot;
        _sensX = PlayerHandler.Instance.playerCam.sensX;
        _sensY = PlayerHandler.Instance.playerCam.sensY;
        _playerCam = PlayerHandler.Instance.playerCam;
    }
    

    public override void OnGrabMission()
    {
        base.OnGrabMission();
        PlayerHandler.Instance.actualMission = this;
        if (TutorialHub.Instance != null) TutorialHub.Instance.missionGrabbed = true;
    }

    public void OnInteract()
    {
        //CanvasManager.Instance.missionLevelHouse.SetActive(true);
    }

    public override void FocusObject()
    {
        if (canInteract) return;
        active = !active;
        CanvasManager.Instance.rotateInfo.SetActive(active);
        Inventory.Instance.cantSwitch = active;
        StartCoroutine(active ? FocusObjectCor() : UnFocusObject());
        transform.localScale = Vector3.one;
    }

    public override void OnDeselectItem()
    {
        base.OnDeselectItem();
        active = false;
    }

    public override void OnDropItem()
    {
        base.OnDropItem();
        active = false;
    }

    public string ShowText()
    {
        return interactableText;
    }

    IEnumerator FocusObjectCor()
    {
        if (TutorialHub.Instance != null) TutorialHub.Instance.missionInspect = true;
        cantBobbing = true;
        canInteract = true;
        transform.SetParent(null);
        PlayerHandler.Instance.UnPossesPlayer();
        while (Vector3.Distance(transform.position, cameraPos.position + cameraPos.forward * offset) > 0.1f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, cameraPos.position + cameraPos.forward * offset, ref reference, .1f);
            yield return new WaitForSeconds(0.01f);
        }

        transform.position = cameraPos.position + cameraPos.forward * offset;
        canInteract = false;
    }

    IEnumerator UnFocusObject()
    {
        cantBobbing = false;
        canInteract = true;
        DisableContent();
        Inventory.Instance.cantSwitch = true;
        transform.SetParent(handPos);
        PlayerHandler.Instance.PossesPlayer();
        while (Vector3.Distance(transform.position, handPos.position) > 0.1f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, handPos.position, ref reference, .1f);
            yield return new WaitForSeconds(0.01f);
        }

        transform.position = handPos.position;
        canInteract = false;
        Inventory.Instance.cantSwitch = false;
        cantBobbing = false;
    }

    private void Update()
    {
        if (!active) return;
        RotateObject();
        
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if(Input.GetButtonDown("Focus"))FocusObject();
        if (Input.GetMouseButtonDown(0)) GetDescriptionContent();
    }

    private void GetDescriptionContent()
    {
        if (CanvasManager.Instance.menu.activeInHierarchy) return;
        if (!active) return;

        if (contentActive)
        {
            DisableContent();
            return;
        }
        var content = misionName == "HouseLevel" ? CanvasManager.Instance.descriptionMissionContent : CanvasManager.Instance.descrptionMissionConten2;
        content.SetActive(true);
        contentActive = true;
    }

    private void DisableContent()
    {
        var content = misionName == "HouseLevel" ? CanvasManager.Instance.descriptionMissionContent : CanvasManager.Instance.descrptionMissionConten2;
        content.SetActive(false);
        contentActive = false;
    }

    void RotateObject()
    {
            
        float XaxisRotation = Input.GetAxis("Horizontal") * _sensX * Time.deltaTime;
        float YaxisRotation = Input.GetAxis("Vertical") * _sensY *Time.deltaTime;
        float ZaxisRotation = Input.GetAxis("ZAxis") * _sensY *Time.deltaTime;

        transform.RotateAround(transform.position, _playerCam.transform.right, YaxisRotation);
        transform.RotateAround(transform.position, _playerCam.transform.up, XaxisRotation);
        transform.RotateAround(transform.position, _playerCam.transform.forward, ZaxisRotation);
    }
}
