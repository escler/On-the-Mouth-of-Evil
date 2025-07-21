using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : Item
{
    private bool _onHand, canInteract, _active;
    public Vector3[] rotPos;
    private int _actualCount;
    public Transform cameraPos, handPos;
    private Vector3 reference = Vector3.zero;
    private float _sensX, _sensY;
    private PlayerCam _playerCam;
    public Mark mark;
    public float offset;
    public int numberSlot;
    [SerializeField] private GameObject aura;


    private void Start()
    {
        cameraPos = PlayerHandler.Instance.cameraPos;
        handPos = PlayerHandler.Instance.handPivot;
        _sensX = PlayerHandler.Instance.playerCam.sensX;
        _sensY = PlayerHandler.Instance.playerCam.sensY;
        _playerCam = PlayerHandler.Instance.playerCam;
    }

    public override void OnSelectItem()
    {
        base.OnSelectItem();
        _onHand = true;
    }


    public override void OnGrabItem()
    {
        base.OnGrabItem();
        transform.localScale = Vector3.one;
        aura.SetActive(false);
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
        if (canInteract) return;
        _active = !_active;
        CanvasManager.Instance.rotateInfo.SetActive(_active);
        Inventory.Instance.cantSwitch = _active;
        StartCoroutine(_active ? FocusObject() : UnFocusObject());
        transform.localScale = Vector3.one;
    }

    private void PlaceSkull(bool hit, RaycastHit i)
    {
        if (canInteract) return;
        if (_active) return;
        if (!hit || !i.transform.TryGetComponent(out SkullPuzzleSlot puzzleSlot)) return;
        
        puzzleSlot.InteractWithSkull(this);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();
        canInteractWithItem = CanInteractWithItem();
        ChangeCrossHair();
        
        if(Input.GetButtonDown("Focus")) OnInteract(rayConnected,ray);
        if(Input.GetButtonDown("Interact")) PlaceSkull(rayConnected, ray);
    }

    public override bool CanInteractWithItem()
    {
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();

        if (!rayConnected) return false;
        if (ObjectDetector.Instance.InteractText()) return true;
        if (ray.transform.TryGetComponent(out SkullPuzzleSlot item)) return true;

        return false;
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
        cantBobbing = true;
        PlayerHandler.Instance.UnPossesPlayer();
        while (Vector3.Distance(transform.position, cameraPos.position + cameraPos.forward * offset) > 0.1f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, cameraPos.position + cameraPos.forward * offset, ref reference, .1f);
            yield return new WaitForSeconds(0.01f);
        }

        transform.position = cameraPos.position + cameraPos.transform.forward * offset;
        canInteract = false;
    }

    IEnumerator UnFocusObject()
    {
        canInteract = true;
        Inventory.Instance.cantSwitch = true;
        PlayerHandler.Instance.PossesPlayer();
        while (Vector3.Distance(transform.position, handPos.position) > 0.1f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, handPos.position, ref reference, .1f);
            yield return new WaitForSeconds(0.01f);
        }

        transform.position = handPos.position;
        cantBobbing = false;
        canInteract = false;
        Inventory.Instance.cantSwitch = false;
    }
}