using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    public int distance;
    private Transform _targetAim, _cameraPos;
    public GameObject UIObject;

    private void Start()
    {
        _targetAim = Player.Instance.targetAim;
        _cameraPos = Camera.main.transform;
    }

    private void Update()
    {
        var dir = _targetAim.position - _cameraPos.position;
        
        RaycastHit hit;
        
        var ray = Physics.Raycast(_cameraPos.position, dir, out hit, distance, _layerMask);

        if (ray)
        {
            UIObject.SetActive(true);
            if (Input.GetButtonDown("Interact")) hit.transform.GetComponent<IInteractable>().OnInteract();
        }
        else
        {
            UIObject.SetActive(false);
        }
    }
}
