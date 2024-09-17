using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void OnInteractItem();

    public void OnInteract(bool hit, RaycastHit i);

    public string ShowText();
}
