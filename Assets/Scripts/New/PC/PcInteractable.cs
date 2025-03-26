using System;
using System.Collections;
using UnityEngine;
using Cursor = UnityEngine.Cursor;

public class PcInteractable : MonoBehaviour, IInteractable
{
    private bool _inPC, _cantInteract;
    public Transform cameraPos;
    public void OnInteractItem()
    {
        if (_inPC) return;
        if (_cantInteract) return;
        _cantInteract = true;
        StartCoroutine(CantInteractCD());
        PlayerHandler.Instance.UnPossesPlayer();
        CameraFollow.Instance.SetNewCameraPos(cameraPos);
        GetComponent<BoxCollider>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _inPC = true;
        CanvasManager.Instance.crossHair.enabled = false;
    }

    private void Awake()
    {
        _inPC = false;
    }

    private void Update()
    {
        if (!_inPC) return;
        
        if(Input.GetButtonDown("Interact"))ExitPC();
    }

    private void ExitPC()
    {
        if (_cantInteract) return;
        _cantInteract = true;
        PlayerHandler.Instance.PossesPlayer();
        CameraFollow.Instance.SetNewCameraPos(PlayerHandler.Instance.cameraPos);
        GetComponent<BoxCollider>().enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        CanvasManager.Instance.crossHair.enabled = true;
        StartCoroutine(LeavingPC());
        StartCoroutine(CantInteractCD());
    }

    IEnumerator CantInteractCD()
    {
        yield return new WaitForSeconds(.25f);
        _cantInteract = false;
    }

    IEnumerator LeavingPC()
    {
        yield return new WaitForSeconds(.25f);
        _inPC = false;
    }
    
    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public void OnInteractWithObject()
    {
    }

    public string ShowText()
    {
        return "";
    }

    public bool CanShowText()
    {
        return true;
    }
}
