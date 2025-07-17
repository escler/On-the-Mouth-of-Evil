using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Item : MonoBehaviour, IInteractable
{
    public string itemName;
    public GameObject objectPrefab, uiElement;
    private bool _canInteract;
    protected bool canUse;
    public string uiText;
    public ItemCategory category;
    public bool canShowText, canInspectItem, canInteractWithItem;

    public Vector3 angleHand;

    protected bool cantBobbing;
    private AudioSource audioSource;
    public string soundName;
    public bool CantBobbing
    {
        set => cantBobbing = value;
    }
    
    private float _timer;
    private float bobbingSpeed = 5;
    private float bobbingAmount = .005f;

    private void Awake()
    {
        SceneManager.sceneLoaded += DestroyOnMenu;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= DestroyOnMenu;
    }

    public virtual void OnGrabItem()
    {
        Inventory.Instance.AddItem(this, category);
        if (GetComponentInChildren<SkinnedMeshRenderer>() == null)
        {
            GetComponentInChildren<MeshRenderer>().gameObject.layer = 18;
        }
        else
        {
            GetComponentInChildren<SkinnedMeshRenderer>().gameObject.layer = 18;
        }
            
    }

    public virtual void OnUpdate()
    {
        if(!cantBobbing) BobbingItem();
    }

    public virtual void FocusObject()
    {
        
    }

    public virtual void OnInteractItem()
    {
        MusicManager.Instance.PlaySound(soundName, false);
        OnGrabItem();
    }

    public virtual void OnSelectItem()
    {
        CanInspectItem();
    }

    public virtual void OnDeselectItem()
    {
        CanvasManager.Instance.inspectImage.SetActive(false);
    }

    public virtual void OnDropItem()
    {
        gameObject.SetActive(true);
        GetComponentInChildren<MeshRenderer>().gameObject.layer = 1;
    }

    public virtual void OnInteract(bool hit, RaycastHit i)
    {
        canUse = false;
    }

    public void OnInteractWithObject()
    {
        
    }

    public string ShowText()
    {
        return uiText;
    }

    public virtual bool CanShowText()
    {
        return canShowText;
    }

    public virtual bool CanInpectItem()
    {
        return canInspectItem;
    }

    public virtual bool CanInteractWithItem()
    {
        return canInteractWithItem;
    }

    protected void CanInspectItem()
    {
        CanvasManager.Instance.inspectImage.SetActive(canInspectItem);
    }

    public virtual void ChangeCrossHair()
    {
        if(canInteractWithItem) CanvasManager.Instance.crossHairUI.IncreaseUI();
        else CanvasManager.Instance.crossHairUI.DecreaseUI();
    }

    void BobbingItem()
    {
        _timer += Time.deltaTime * bobbingSpeed / 4;
        transform.localPosition = new Vector3(transform.localPosition.x,
            transform.localPosition.y + Mathf.Cos(_timer) * bobbingAmount / 8,
            transform.localPosition.z);
    }

    void DestroyOnMenu(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.name == "Menu") Destroy(gameObject);
    }
}
