using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectDetector : MonoBehaviour
{
    public static ObjectDetector Instance { get; private set; }
    
    public LayerMask layer, layerDoors;
    public Transform cameraPos;
    public int distance;
    public GameObject uiInteractionText, ui2;
    private CrosshairUI _crosshairUI;
    public RaycastHit _hit, _hitDoors;
    private GameObject descriptionItem;
    public Item selectedItem;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        SceneManager.sceneLoaded += StartParameters;

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= StartParameters;
    }

    private void Update()
    {
        if (Inventory.Instance == null || cameraPos == null) return;
        selectedItem = Inventory.Instance.selectedItem == null ? null : Inventory.Instance.selectedItem;
        if (selectedItem != null) selectedItem.OnUpdate();
        else OnUpdateWithoutItem();


        CheckInteractText();
        GrabCheck();
        CheckDoors();
        DescriptionChecker();
        EmptyHandCheck();
        //CheckEnviromentObjects();
    }

    private void CheckEnviromentObjects()
    {
        var ray = CheckRayCast();
        if (!ray) return;

        if (Input.GetButtonDown("Interact"))
        {
            if (_hit.transform.TryGetComponent(out EnviromentObjects obj))
            {
                obj.Interact();
            }
        }
    }

    bool GrabText()
    {
        var ray = CheckRayCast();
        var rayDoor = CheckDoorsRayCast();
        
        if (!ray && !rayDoor)
        {
            return false;
        }

        if (_hit.transform.TryGetComponent(out SkullPuzzleSlot socket))
        {
            if (socket.currentSkull != null) return true;
            if (Inventory.Instance.selectedItem == null) return false;
            if (socket.currentSkull == null && Inventory.Instance.selectedItem.itemName == "Skull") return true;
        }
        
        if (_hit.transform.TryGetComponent(out CandleRitual candleRitual))
        {
            if (candleRitual.candle != null) return true;
            if (Inventory.Instance.selectedItem == null) return false;
            if (candleRitual.candle == null && Inventory.Instance.selectedItem.itemName == "Candle")
            {
                if(RitualManager.Instance.firstCandlePlaced == null) return true;
                if (RitualManager.Instance.firstCandlePlaced.badCandle ==
                    Inventory.Instance.selectedItem.GetComponent<Candle>().badCandle) return true;
                return false;
            }
        }
        
        if (_hit.transform.TryGetComponent(out CubeSlot slot))
        {
            if (slot.RotatingPhase) return false;
            if (slot.CubeInSlot != null) return true;
            if (Inventory.Instance.selectedItem == null) return false;
            if (slot.CubeInSlot == null && Inventory.Instance.selectedItem.itemName == "Cube") return true;
        }
        
        if (_hit.transform.TryGetComponent(out BookSpot bookSlot))
        {
            if (bookSlot.BookPuzzleTV != null) return true;
            if (Inventory.Instance.selectedItem == null) return false;
            if (bookSlot.BookPuzzleTV == null && Inventory.Instance.selectedItem.itemName == "Book") return true;
        }

        if (_hit.transform.TryGetComponent(out SaltPuzzleTable table))
        {
            if (Inventory.Instance.selectedItem == null) return false;
            if (Inventory.Instance.selectedItem.itemName == "Salt Recipient") return true;
        }

        if (_hit.transform.TryGetComponent(out SheetSlot sheetSlot))
        {
            if (sheetSlot.Sheet != null) return true;
            if (Inventory.Instance.selectedItem == null) return false;
            if (Inventory.Instance.selectedItem.itemName == "Music Sheet") return true;
        }
        
        if (!_hit.transform.TryGetComponent(out MovableItem movableItem) && _hit.transform.TryGetComponent(out IInteractable interactable))
        {
            if (interactable.CanShowText()) return true;
        }

        if (rayDoor && _hitDoors.transform.TryGetComponent(out Door door)) return true;

        return false;
    }

    void OnUpdateWithoutItem()
    {
        CrossHairCheck();
        if (CanvasManager.Instance.inspectImage.activeInHierarchy) CanvasManager.Instance.inspectImage.SetActive(false);
    }

    private void CrossHairCheck()
    {
        var ray = CheckRayCast();

        if (!ray)
        {
            _crosshairUI.DecreaseUI();
            return;
        }
        
        if(_hit.transform.TryGetComponent(out IInteractObject interactObject)) _crosshairUI.IncreaseUI();
        else _crosshairUI.DecreaseUI();
    }

    void StartParameters(Scene scene, LoadSceneMode loadSceneMode)
    {
        StartCoroutine(StartParametersCor());
    }
    
    IEnumerator StartParametersCor()
    {
        yield return new WaitForSeconds(0.1f);
        if (cameraPos == null) cameraPos = PlayerHandler.Instance.cameraPos;
        if (uiInteractionText == null) uiInteractionText = CanvasManager.Instance.InteractionText;
        if (ui2 == null) ui2 = CanvasManager.Instance.moveObjectUI;
        if (_crosshairUI == null) _crosshairUI = CanvasManager.Instance.crossHairUI;
    }

    public bool CheckRayCast()
    {
        bool ray = Physics.Raycast(cameraPos.position, cameraPos.forward, out _hit, distance, layer);
        return ray;
    }

    private bool CheckDoorsRayCast()
    {
        bool ray = Physics.Raycast(cameraPos.position, cameraPos.forward, out _hitDoors, 1.7f, layerDoors);
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

        uiInteractionText.SetActive(GrabText());
        ui2.SetActive(raycast && _hit.transform.TryGetComponent(out MovableItem movable2) && !movable2.relocated);
    }

    private void CrossHair()
    {
        var raycast = CheckRayCast();
        if(raycast) _crosshairUI.IncreaseUI();
        else _crosshairUI.DecreaseUI();
    }

    private void GrabCheck()
    {
        if (PlayerHandler.Instance.cantInteract) return;
        var raycast = CheckRayCast();
        
        if (raycast && Input.GetButtonDown("Interact"))
        {
            if (!_hit.transform.TryGetComponent(out IInteractable interactable)) return;
            interactable.OnInteractItem();
        }
        
        //if(Input.GetButtonDown("Focus") && selectedItem != null) selectedItem.FocusObject();

        
        if (raycast && Input.GetMouseButton(0))
        {
            if (_hit.transform.TryGetComponent(out MovableItem movable) && !movable.relocated)
            {
                movable.RelocateItem();
                PlayerHandler.Instance.UnPossesPlayer();
            }
        }
    }

    public void EmptyHandCheck()
    {
        var raycast = CheckRayCast();

        if (raycast && Input.GetMouseButtonDown(0))
        {
            if (_hit.transform.TryGetComponent(out IInteractObject i))
            {
                i.OnInteractWithThisObject();
            }
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
