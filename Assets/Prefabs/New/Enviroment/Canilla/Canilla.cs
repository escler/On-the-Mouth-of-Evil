using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canilla : MonoBehaviour, IInteractable
{
    public ParticleSystem ps;
    public ParticleSystem.EmissionModule emission;
    private Animator _animator;
    private bool _on;
    // Start is called before the first frame update
    void Awake()
    {
        emission = ps.emission;
        emission.enabled = false;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteractItem()
    {
        _on = !_on;
        emission.enabled = _on;
        _animator.SetBool("Open", _on);
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
