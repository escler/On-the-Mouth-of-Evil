using TMPro;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    public LayerMask layer;
    public Transform cameraPos;
    public int distance;
    public GameObject ui, ui2;
    private CrosshairUI _crosshairUI;
    private RaycastHit _hit, _hitDoors;
    private GameObject descriptionItem;

    private void Update()
    {
        if (cameraPos == null) cameraPos = PlayerHandler.Instance.cameraPos;
        if (ui == null) ui = CanvasManager.Instance.InteractionText;
        if (ui2 == null) ui2 = CanvasManager.Instance.moveObjectUI;
        if (_crosshairUI == null) _crosshairUI = CanvasManager.Instance.crossHairUI;
        if (Inventory.Instance == null) return;

        bool ray = Physics.Raycast(cameraPos.position, cameraPos.forward, out _hit, distance, layer);
        ui.SetActive(ray && !_hit.transform.TryGetComponent(out MovableItem movablei));
        ui2.SetActive(ray && _hit.transform.TryGetComponent(out MovableItem movable2) && Inventory.Instance.selectedItem == null);


        CheckInteractText();
        InputCheck();
        CheckDoors();
        CrossHair();
        DescriptionChecker();
    }

    private bool CheckRayCast()
    {
        bool ray = Physics.Raycast(cameraPos.position, cameraPos.forward, out _hit, distance, layer);
        return ray;
    }

    private bool CheckDoorsRayCast()
    {
        bool ray = Physics.Raycast(cameraPos.position, cameraPos.forward, out _hitDoors, distance / 2, layer);
        return ray;
    }

    private void CheckDoors()
    {
        if (!CheckDoorsRayCast()) return;

        if (!_hitDoors.transform.TryGetComponent(out Door doorC)) return;
        
        if(Input.GetButtonDown("Interact")) doorC.InteractDoor();
    }

    private void CheckInteractText()
    {
        var raycast = CheckRayCast();
        var raycastDoor = CheckDoorsRayCast();

        if (!raycast) return;
        
        
        ui.SetActive(raycast && !_hit.transform.TryGetComponent(out MovableItem movablei) && _hit.transform.GetComponent<IInteractable>().CanShowText() || raycastDoor && _hitDoors.transform.TryGetComponent(out Door door));
        ui2.SetActive(raycast && _hit.transform.TryGetComponent(out MovableItem movable2) && Inventory.Instance.selectedItem == null);
    }

    private void CrossHair()
    {
        var raycast = CheckRayCast();
        if(raycast) _crosshairUI.IncreaseUI();
        else _crosshairUI.DecreaseUI();
    }

    private void InputCheck()
    {
        var raycast = CheckRayCast();
        
        if (raycast && Input.GetButtonDown("Interact"))
        {
            _hit.transform.GetComponent<IInteractable>().OnInteractItem();
        }

        
        if (Inventory.Instance.selectedItem == null)
        {
            if (raycast && Input.GetMouseButton(0))
            {
                if (_hit.transform.TryGetComponent(out MovableItem movable))
                {
                    movable.RelocateItem();
                    return;
                }
            }
        }

        if (Inventory.Instance.selectedItem != null)
        {
            if (Inventory.Instance.selectedItem.itemName == "Cross")
            {
                if (Input.GetMouseButton(0))
                {
                    Inventory.Instance.selectedItem.OnInteract(raycast,_hit);
                }
                
                if(Input.GetMouseButtonUp(0))
                    Inventory.Instance.selectedItem.GetComponent<Cross>().OnUpCross();
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Inventory.Instance.selectedItem.OnInteract(raycast,_hit);
                }
            }
        }
        if (raycast && Input.GetMouseButtonDown(0))
        {
            _hit.transform.GetComponent<IInteractable>().OnInteractWithObject();
        }
    }

    public void DescriptionChecker()
    {
        var raycast = CheckRayCast();
        
        if (raycast)
        {
            if(_hit.transform.TryGetComponent(out IInteractable interactable))
            {
                if (_hit.transform.TryGetComponent(out Item item))
                {
                    var actualNewObject = CanvasManager.Instance.GetDescription(item.itemName);
                    if (descriptionItem != actualNewObject && descriptionItem != null)
                    {
                        descriptionItem.SetActive(false);
                    }

                    if (actualNewObject == null) return;
                    descriptionItem = actualNewObject;
                    descriptionItem.SetActive(true);
                }
            }
        }
        else
        {
            if (descriptionItem == null) return;
            descriptionItem.SetActive(false);
            descriptionItem = null;
        }
    }
}
