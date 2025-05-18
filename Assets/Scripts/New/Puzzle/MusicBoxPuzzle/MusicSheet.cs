using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSheet : Item
{
    [SerializeField] private int number;
    public int Number => number;
    
    public Transform cameraPos, handPos;
    private bool canInteract, active;
    public float offset;
    private Vector3 reference = Vector3.zero;
    private float _sensX, _sensY;
    private PlayerCam _playerCam;
    [SerializeField] private string tag;


    private void Start()
    {
        active = false;
        cameraPos = PlayerHandler.Instance.cameraPos;
        handPos = PlayerHandler.Instance.handPivot;
        _sensX = PlayerHandler.Instance.playerCam.sensX;
        _sensY = PlayerHandler.Instance.playerCam.sensY;
        _playerCam = PlayerHandler.Instance.playerCam;
    }

    public override void OnGrabItem()
    {
        base.OnGrabItem();
        GetComponentInChildren<AuraItem>().onHand = true;
    }

    public override void OnDropItem()
    {
        base.OnDropItem();
        GetComponentInChildren<AuraItem>().onHand = false;
    }
    
    public override void OnInteract(bool hit, RaycastHit i)
    {
        if (!hit) return;
        if (!canInteractWithItem) return;
        if (active) return;
        
        if (i.transform.TryGetComponent(out SheetSlot sheetSlot))
        {
            Inventory.Instance.DropItem(this, Inventory.Instance.countSelected);
            sheetSlot.PlaceSheet(this);
            GetComponentInChildren<AuraItem>().onHand = false;
            StopAllCoroutines();
            canInteract = false;
        }
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();
        canInteractWithItem = CanInteractWithItem();
        ObjectDetector.Instance.uiInteractionText.SetActive(CanInteractWithItem());
        if (Input.GetButtonDown("Interact"))
        {
            OnInteract(rayConnected, ray);
        }
        if(Input.GetButtonDown("Focus"))FocusObject();
    }
    
    public override bool CanInteractWithItem()
    {
        var ray = ObjectDetector.Instance._hit;
        var rayConnected = ObjectDetector.Instance.CheckRayCast();

        if (!rayConnected) return false;
        if (ObjectDetector.Instance.InteractText()) return true;
        if (ray.transform.TryGetComponent(out SheetSlot item))
        {
            if (item.Sheet == null) return true;
        }
        return false;
    }

    public void MoveInMusicBox()
    {
        StartCoroutine(MoveCor());
    }

    IEnumerator MoveCor()
    {
        Vector3 initial = transform.localPosition;
        Vector3 final = transform.localPosition + transform.right * 0.2f;
        float timer = 0;
        while (timer < 1)
        {
            transform.localPosition = Vector3.Lerp(initial, final, timer);
            timer += Time.deltaTime * 0.5f;
            yield return null;
        }
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
    
    IEnumerator FocusObjectCor()
    {
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
