using TMPro;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    public LayerMask layer;
    public Transform cameraPos;
    public int distance;
    public GameObject ui;
    private CrosshairUI _crosshairUI;
    private RaycastHit _hit;
    private GameObject descriptionItem;

    private void Update()
    {
        if (cameraPos == null) cameraPos = PlayerHandler.Instance.cameraPos;
        if (ui == null) ui = CanvasManager.Instance.InteractionText;
        if (_crosshairUI == null) _crosshairUI = CanvasManager.Instance.crossHairUI;

        bool ray = Physics.Raycast(cameraPos.position, cameraPos.forward, out _hit, distance, layer);
        ui.SetActive(ray);

        if (ray)
        {
            _crosshairUI.IncreaseUI();
            if(_hit.transform.TryGetComponent(out IInteractable interactable))
            {
                ui.GetComponent<TextMeshProUGUI>().text = interactable.ShowText();
                if (_hit.transform.TryGetComponent(out Item item))
                {
                    var actualNewObject = CanvasManager.Instance.GetDescription(item.itemName);
                    if (descriptionItem != actualNewObject && descriptionItem != null)
                    {
                        descriptionItem.SetActive(false);
                    }
                    descriptionItem = actualNewObject;
                    descriptionItem.SetActive(true);
                }
            }
        }
        else
        {
            _crosshairUI.DecreaseUI();
            if (descriptionItem != null)
            {
                descriptionItem.SetActive(false);
                descriptionItem = null;
            }

        }
        
        if (ray && Input.GetButtonDown("Interact"))
        {
            _hit.transform.GetComponent<IInteractable>().OnInteract();
        }
        
        if (Inventory.Instance.selectedItem == null) return;

        if (Inventory.Instance.selectedItem.itemName == "Cross")
        {
            if (Input.GetMouseButton(0))
            {
                Inventory.Instance.selectedItem.OnInteract(ray,_hit);
            }
            
            if(Input.GetMouseButtonUp(0))
                Inventory.Instance.selectedItem.GetComponent<Cross>().OnUpCross();
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Inventory.Instance.selectedItem.OnInteract(ray,_hit);
            }
        }
    }
}
