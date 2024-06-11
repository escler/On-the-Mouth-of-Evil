using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask, _banishLayerMask;
    public int distance, banishRadius;
    private Transform _targetAim, _cameraPos;
    public GameObject UIObject;
    private TypeManager _typeManager;

    private void Start()
    {
        _targetAim = Player.Instance.targetAim;
        _typeManager = TypeManager.Instance;
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
        
        if (Input.GetButtonDown("Interact")) CheckBanish();
    }

    private void CheckBanish()
    {
        var checker = Physics.OverlapSphere(transform.position, banishRadius, _banishLayerMask);

        if (checker.Length <= 0) return;
        if (_typeManager.sequenceGenerated) return;
        _typeManager.GenerateNewSequence(8);
        Player.Instance.DipposeControls();
        foreach (var item in checker)
        {
            item.GetComponentInParent<IBanishable>().StartBanish();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, banishRadius);
    }
}
