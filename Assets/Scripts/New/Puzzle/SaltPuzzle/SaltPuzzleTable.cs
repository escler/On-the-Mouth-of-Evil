using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SaltPuzzleTable : MonoBehaviour, IInteractable
{
    public static SaltPuzzleTable Instance { get; private set; }
    public Transform cameraPosPuzzle;
    public bool playerInTable;
    public Camera camera;
    public Material hightlightMat;
    private SaltRecipient currentRecipient;

    private void Start()
    {
        camera = CameraFollow.Instance.GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (!playerInTable) return;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out SaltRecipient saltRecipient))
            {
                if (saltRecipient != currentRecipient || currentRecipient == null)
                {
                    if(currentRecipient != null) currentRecipient.UnHighlightObject();
                    currentRecipient = saltRecipient;
                    currentRecipient.HightlightObject(hightlightMat);
                }
                
                if (Input.GetMouseButtonDown(0)) saltRecipient.OnRecipientPress();
            }
        }
    }

    public void OnInteract()
    {
        playerInTable = !playerInTable;
        CanvasManager.Instance.crossHairUI.gameObject.SetActive(!playerInTable);
        PlayerHandler.Instance.playerCam.CameraLock = playerInTable;
        Cursor.visible = playerInTable;
        Cursor.lockState = CursorLockMode.Confined;
        PlayerHandler.Instance.playerCam.CameraLock = playerInTable;
        CameraFollow.Instance.SetNewCameraPos(playerInTable ? cameraPosPuzzle : PlayerHandler.Instance.cameraPos);
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public string ShowText()
    {
        return playerInTable ? "Press E To Exit Table" : "Press E To View Table";
    }
}
