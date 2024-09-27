using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricMove : MonoBehaviour, IInteractableEnemy
{
    private bool _fabric;
    AnimationClip _clip;
    public void OnEndInteract()
    {
        _fabric = false;
    }

    public void OnStartInteract()
    {
        _fabric = true;
    }

    private void Awake()
    {
        _clip = GetComponent<Animation>().GetClip("WindMove");
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (_fabric)
        {
            GetComponent<Animation>().Play(_clip.name);
        }
    }
}
