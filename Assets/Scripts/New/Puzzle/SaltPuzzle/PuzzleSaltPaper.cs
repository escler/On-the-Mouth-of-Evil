using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSaltPaper : Item, IInteractable
{
    public Transform focusPos, handPos;
    private Vector3 reference = Vector3.zero;
    private float _sensX, _sensY;
    private PlayerCam _playerCam;
    private bool active;

    private void Start()
    {
        active = false;
        focusPos = PlayerHandler.Instance.closeFocusPos;
        handPos = PlayerHandler.Instance.handPivot;
        _sensX = PlayerHandler.Instance.playerCam.sensX;
        _sensY = PlayerHandler.Instance.playerCam.sensY;
        _playerCam = PlayerHandler.Instance.playerCam;
    }

    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (PlayerHandler.Instance.cantPressInventory) return;
        active = !active;
        CanvasManager.Instance.rotateInfo.SetActive(active);
        StartCoroutine(active ? FocusObject() : UnFocusObject());
        Inventory.Instance.cantSwitch = active;
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
    
    IEnumerator FocusObject()
    {
        PlayerHandler.Instance.UnPossesPlayer();
        while (Vector3.Distance(transform.position, focusPos.position) > 0.1f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, focusPos.position, ref reference, .1f);
            yield return new WaitForSeconds(0.01f);
        }

        transform.position = focusPos.position;
    }

    IEnumerator UnFocusObject()
    {
        PlayerHandler.Instance.PossesPlayer();
        while (Vector3.Distance(transform.position, handPos.position) > 0.1f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, handPos.position, ref reference, .1f);
            yield return new WaitForSeconds(0.01f);
        }

        transform.position = handPos.position;
    }

    private void Update()
    {
        if (!active) return;
        
        RotateObject();
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
