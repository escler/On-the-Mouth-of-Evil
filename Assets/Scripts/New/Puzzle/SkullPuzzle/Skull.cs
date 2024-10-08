using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : Item
{
    private bool _onHand, canInteract, _active;
    public Vector3[] rotPos;
    private int _actualCount;
    public Transform focusPos, handPos;
    private Vector3 reference = Vector3.zero;
    private float _sensX, _sensY;
    private PlayerCam _playerCam;
    public Mark mark;


    private void Start()
    {
        focusPos = PlayerHandler.Instance.farFocusPos;
        handPos = PlayerHandler.Instance.handPivot;
        _sensX = PlayerHandler.Instance.playerCam.sensX;
        _sensY = PlayerHandler.Instance.playerCam.sensY;
        _playerCam = PlayerHandler.Instance.playerCam;
    }

    public override void OnSelectItem()
    {
        _onHand = true;
    }

    public override void OnDeselectItem()
    {
        base.OnDeselectItem();
        _onHand = false;
    }

    public override void OnDropItem()
    {
        base.OnDropItem();
        _onHand = false;
    }

    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (!hit || !i.transform.TryGetComponent(out SkullPuzzleSlot puzzleSlot))
        {
            if (canInteract) return;
            _active = !_active;
            CanvasManager.Instance.rotateInfo.SetActive(_active);
            Inventory.Instance.cantSwitch = _active;
            StartCoroutine(_active ? FocusObject() : UnFocusObject());
            return;
        }
        
        puzzleSlot.PlaceSkull(this);
    }

    private void Update()
    {
        if (!_active) return;
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

    IEnumerator FocusObject()
    {
        canInteract = true;
        transform.SetParent(null);
        PlayerHandler.Instance.UnPossesPlayer();
        while (Vector3.Distance(transform.position, focusPos.position) > 0.1f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, focusPos.position, ref reference, .1f);
            yield return new WaitForSeconds(0.01f);
        }

        transform.position = focusPos.position;
        canInteract = false;
    }

    IEnumerator UnFocusObject()
    {
        canInteract = true;
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
    }
}