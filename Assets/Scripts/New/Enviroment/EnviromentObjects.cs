using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentObjects : MonoBehaviour, IInteractable
{
    public Animator animator;

    private bool _open;

    private bool cantInteract;
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
        if (cantInteract) return;
        _open = !_open;
        animator.SetBool("Open", _open);
        cantInteract = true;
        StartCoroutine(WaitToInteractAgaint(_open));
    }

    IEnumerator WaitToInteractAgaint(bool open)
    {
        string nameAnimation = open ? "IdleOpen" : "IdleClose";
        string currentAnimation = open ? "Open" : "Close";

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(currentAnimation));
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(nameAnimation));

        cantInteract = false;
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
        return !cantInteract;
    }
}
