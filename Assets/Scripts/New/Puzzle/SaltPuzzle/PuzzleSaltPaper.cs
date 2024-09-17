using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSaltPaper : Item, IInteractable
{
    private bool active;

    public override void OnInteract(bool hit, RaycastHit i)
    {
        CanvasManager.Instance.puzzleSaltPaper.SetActive(!active);
        active = CanvasManager.Instance.puzzleSaltPaper.activeInHierarchy;
        Inventory.Instance.cantSwitch = active;
    }


    public string ShowText()
    {
        return "Press E To Grab Paper";
    }
}
