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
    private Transform cameraPos;
    private RaycastHit _hit;
    private float distance = 3;
    public LayerMask layer;
    public bool canInteractWithSalt;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        camera = CameraFollow.Instance.GetComponentInChildren<Camera>();
        cameraPos = PlayerHandler.Instance.cameraPos;
    }

    private void Update()
    {
        if (canInteractWithSalt) return;
        
        bool ray = Physics.Raycast(cameraPos.position, cameraPos.forward, out _hit, distance, layer);

        if (!playerInTable)
        {
            currentRecipient = null;
            return;
        }
        if (!ray)
        {
            currentRecipient = null;
            return;
        }


        if (_hit.collider.TryGetComponent(out SaltRecipient saltRecipient))
        {
            if (saltRecipient != currentRecipient || currentRecipient == null)
            {
                currentRecipient = saltRecipient;
            }
            
            if (Input.GetMouseButtonDown(0)) saltRecipient.OnRecipientPress();
        }
    }

    public void OnInteractItem()
    {
        playerInTable = !playerInTable;
        CanvasManager.Instance.crossHairUI.gameObject.SetActive(!playerInTable);
        PlayerHandler.Instance.playerCam.CameraLock = playerInTable;
        PlayerHandler.Instance.cantPressInventory = playerInTable;
        PlayerHandler.Instance.ChangePlayerPosses(!playerInTable);
        Cursor.visible = playerInTable;
        Cursor.lockState = CursorLockMode.Confined;
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
