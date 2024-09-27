using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour, IInteractableEnemy
{
    [SerializeField] private bool _blood;
    [SerializeField] private GameObject targetObject;
    MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = targetObject.GetComponent<MeshRenderer>();
        _blood = false;
    }

    public void OnStartInteract()
    {
        if (_meshRenderer != null)
        {
           
            _meshRenderer.enabled = true;
        }

    }

    private void Update()
    {
        if (_blood)
        {
            OnStartInteract();
        }
        else
        {
            OnEndInteract();
        }
    }

    public void OnEndInteract()
    {
       
            if (_meshRenderer != null)
            {
               
                _meshRenderer.enabled = false;
            }
       


    }
}
