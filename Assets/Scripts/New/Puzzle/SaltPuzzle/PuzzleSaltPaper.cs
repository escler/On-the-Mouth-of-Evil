using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSaltPaper : Item, IInteractable
{
    public Transform cameraPos, handPos;
    private Vector3 reference = Vector3.zero;
    private float _sensX, _sensY;
    private PlayerCam _playerCam;
    private bool active;
    private bool canInteract;
    public float offset;

    private void Start()
    {
        active = false;
        cameraPos = PlayerHandler.Instance.cameraPos;
        handPos = PlayerHandler.Instance.handPivot;
        _sensX = PlayerHandler.Instance.playerCam.sensX;
        _sensY = PlayerHandler.Instance.playerCam.sensY;
        _playerCam = PlayerHandler.Instance.playerCam;
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

    public override void OnInteractItem()
    {
        base.OnInteractItem();
        transform.localScale = Vector3.one;
    }
    public string ShowText()
    {
        return "Press E To Grab Paper";
    }


    IEnumerator FocusObjectCor()
    {
        cantBobbing = true;
        canInteract = true;
        transform.SetParent(null);
        transform.localScale = Vector3.one;
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
        Inventory.Instance.cantSwitch = true;
        canInteract = true;
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
        if(Input.GetButtonDown("Focus")) FocusObject();
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
