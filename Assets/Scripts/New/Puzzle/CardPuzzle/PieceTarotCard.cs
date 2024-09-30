using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PieceTarotCard : Item, IInteractable
{
    public int cardCountPiece;
    public bool onHand;
    private MaterialPropertyBlock _alpha;
    private MeshRenderer _mesh;

    private void Awake()
    {
        _alpha = new MaterialPropertyBlock();
        _alpha.SetFloat("_Alpha", 0);
        _mesh = GetComponent<MeshRenderer>();
        _mesh.SetPropertyBlock(_alpha);
    }

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
        if (TarotCardPuzzle.Instance.CanPlace)
        {
            if (_alpha.GetFloat("_Alpha") < .1f)
            {
                _alpha.SetFloat("_Alpha", .1f);
                _mesh.SetPropertyBlock(_alpha);
                
            }
        }
        else
        {
            _alpha.SetFloat("_Alpha",0);
            _mesh.SetPropertyBlock(_alpha);
        }
    }

    public override void OnDropItem()
    {
        base.OnDropItem();
        onHand = false;
        gameObject.layer = 9;
        TarotCardPuzzle.Instance.DeactivateMesh();
        TarotCardPuzzle.Instance.heldObj = null;
        CanvasManager.Instance.rotateInfo.SetActive(false);
        _alpha.SetFloat("_Alpha",0);
        _mesh.SetPropertyBlock(_alpha);
    }

    public override void OnDeselectItem()
    {
        TarotCardPuzzle.Instance.DeactivateMesh();
        TarotCardPuzzle.Instance.heldObj = null;
        _alpha.SetFloat("_Alpha",0);
        _mesh.SetPropertyBlock(_alpha);
    }

    private void OnDisable()
    {
        onHand = false;
        if (TarotCardPuzzle.Instance != null) TarotCardPuzzle.Instance.heldObj = null;
        if (CanvasManager.Instance == null) return;
        CanvasManager.Instance.rotateInfo.SetActive(false);
        _alpha.SetFloat("_Alpha",0);
        _mesh.SetPropertyBlock(_alpha);
    }

    public string ShowText()
    {
        return "Pick Piece of Card";
    }
}
