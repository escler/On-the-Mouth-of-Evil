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
            if(_hit.transform.TryGetComponent(out IInteractable interactable)) ui.GetComponent<TextMeshProUGUI>().text =
                interactable.ShowText();
        }
        else _crosshairUI.DecreaseUI();
        
        if (ray && Input.GetButtonDown("Interact"))
        {
            _hit.transform.GetComponent<IInteractable>().OnInteract();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Inventory.Instance.selectedItem == null) return;
            Inventory.Instance.selectedItem.OnInteract(ray,_hit);
        }
    }
}
