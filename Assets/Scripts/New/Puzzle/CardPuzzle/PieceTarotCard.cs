using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceTarotCard : MonoBehaviour, IInteractable
{
    public int cardCountPiece;
    
    public void OnInteract()
    {
        if (TarotCardPuzzle.Instance.heldObj != null) return;
        TarotCardPuzzle.Instance.PickUpObject(gameObject, cardCountPiece);
    }

    public void OnInteract(bool hit, RaycastHit i)
    {

    }

    public string ShowText()
    {
        return "Pick Piece of Card";
    }
}
