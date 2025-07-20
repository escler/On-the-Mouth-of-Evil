using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetSlot : MonoBehaviour, IInteractable
{
    private MusicSheet _sheet;
    public MusicSheet Sheet => _sheet;
    private bool _sheetPlaced;
    private bool _canTake;
    public void OnInteractItem()
    {
        if(!_sheetPlaced) return;
        RemoveSheet();
    }

    private void RemoveSheet()
    {
        if(!_sheetPlaced) return;
        
        Sheet.GetComponent<BoxCollider>().enabled = true;
        Sheet.OnGrabItem();
        _sheet = null;
        _sheetPlaced = false;
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
        return _sheet == null ? false : true;

    }

    public void PlaceSheet(MusicSheet musicSheet)
    {
        if (_sheet != null) return;
        musicSheet.GetComponent<BoxCollider>().enabled = false;
        musicSheet.GetComponent<Rigidbody>().isKinematic = true;
        musicSheet.transform.position = transform.position;
        musicSheet.transform.rotation = transform.rotation;
        musicSheet.transform.parent = transform;
        StartCoroutine(PlaceSheetCor(musicSheet));
    }
    
    IEnumerator PlaceSheetCor(MusicSheet musicSheet)
    {
        yield return new WaitForSeconds(0.1f);
        _sheet = musicSheet;
        _sheetPlaced = true;
    }
}
