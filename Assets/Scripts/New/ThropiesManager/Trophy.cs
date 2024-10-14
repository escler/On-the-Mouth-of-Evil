using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trophy : Item
{
    private bool active, canInteract, canRotate;
    public Transform focusPos;
    public Quaternion startRot;
    private Vector3 startPos;
    private Vector3 reference = Vector3.zero;
    private float _sensX, _sensY;
    private PlayerCam _playerCam;
    
    private void Start()
    {
        active = false;
        focusPos = PlayerHandler.Instance.farFocusPos;
        startPos = transform.position;
        startRot = transform.rotation;
        _sensX = PlayerHandler.Instance.playerCam.sensX;
        _sensY = PlayerHandler.Instance.playerCam.sensY;
        _playerCam = PlayerHandler.Instance.playerCam;
    }
    
    public override void OnGrabItem()
    {
        active = !active;
        if(!canInteract) StartCoroutine(active ? FocusObject() : UnFocusObject());
    }

    private void Update()
    {
        if (!active) return;
        
        RotateObject();
        if (canRotate && Input.GetButtonDown("Interact")) StartCoroutine(UnFocusObject());
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

    public override void OnInteract(bool hit, RaycastHit i)
    {
        
    }

    public override bool CanShowText()
    {
        return true;
    }
    
    IEnumerator FocusObject()
    {
        canInteract = true;
        PlayerHandler.Instance.UnPossesPlayer();
        while (Vector3.Distance(transform.position, focusPos.position) > 0.1f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, focusPos.position, ref reference, .1f);
            yield return new WaitForSeconds(0.01f);
        }

        canRotate = true;
        transform.position = focusPos.position;
        canInteract = false;
    }

    IEnumerator UnFocusObject()
    {
        canRotate = false;
        canInteract = true;
        Inventory.Instance.cantSwitch = true;
        PlayerHandler.Instance.PossesPlayer();
        while (Vector3.Distance(transform.position, startPos) > 0.1f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, startPos, ref reference, .1f);
            yield return new WaitForSeconds(0.01f);
        }

        active = false;
        transform.position = startPos;
        transform.rotation = startRot;
        canInteract = false;
        Inventory.Instance.cantSwitch = false;
    }
}
