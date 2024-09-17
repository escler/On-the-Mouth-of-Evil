using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSaltPaper : MonoBehaviour, IInteractable
{
    public void OnInteractItem()
    {
        CanvasManager.Instance.puzzleSaltPaper.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public string ShowText()
    {
        return "Press E To Grab Paper";
    }
}
