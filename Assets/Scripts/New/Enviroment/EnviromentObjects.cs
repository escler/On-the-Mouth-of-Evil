using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentObjects : MonoBehaviour, IInteractable
{
    public Animator animator;

    private bool _open;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        _open = !_open;
        animator.SetBool("Open", _open);
    }

    public void OnInteractItem()
    {
        Interact();
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
        return true;
    }
}
