using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PieceTarotCard : Item, IInteractable
{
    public int cardCountPiece;
    public bool onHand;
    
    public override void OnInteractItem()
    {
        base.OnInteractItem();
        onHand = true;
    }

    public override void OnInteract(bool hit, RaycastHit i)
    {
    }

    public override void OnSelectItem()
    {
        onHand = true;
        CanvasManager.Instance.rotateInfo.SetActive(true);
    }

    private void Update()
    {
        if (!onHand) return;
        TarotCardPuzzle.Instance.PickUpObject(gameObject, cardCountPiece);
    }

    public override void OnDropItem()
    {
        base.OnDropItem();
        onHand = false;
        gameObject.layer = 9;
        TarotCardPuzzle.Instance.heldObj = null;
    }

    public override void OnDeselectItem()
    {
        TarotCardPuzzle.Instance.DeactivateMesh();
        TarotCardPuzzle.Instance.heldObj = null;
    }

    private void OnDisable()
    {
        onHand = false;
        if (TarotCardPuzzle.Instance != null) TarotCardPuzzle.Instance.heldObj = null;
        if (CanvasManager.Instance == null) return;
        CanvasManager.Instance.rotateInfo.SetActive(false);
    }

    public string ShowText()
    {
        return "Pick Piece of Card";
    }
}
