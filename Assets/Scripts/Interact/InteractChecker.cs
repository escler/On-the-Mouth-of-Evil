using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask, _banishLayerMask;
    public int distance, banishRadius;
    private Transform _targetAim, _cameraPos;
    public GameObject UIObject;
    private TypeManager _typeManager;
    private CircleQuery _query;
    public GameObject banishRenderer;

    private void Start()
    {
        _targetAim = Player.Instance.targetAim;
        _typeManager = TypeManager.Instance;
        _cameraPos = Camera.main.transform;
        _query = GetComponentInChildren<CircleQuery>();
    }

    private void Update()
    {
        var dir = _targetAim.position - _cameraPos.position;
        
        RaycastHit hit;
        
        var ray = Physics.Raycast(_cameraPos.position, dir, out hit, distance, _layerMask);

        var ray2 = Physics.OverlapSphere(transform.position, banishRadius, _banishLayerMask)
            .Select(x => x.GetComponentInParent<IBanishable>()).Where(x => x != null && x.canBanish);
        if (ray2.Any())
        {
            UIObject.SetActive(true);
        }
        else
        {
            UIObject.SetActive(false);
        }
        
        if (Input.GetButtonDown("Interact")) CheckBanish();
    }

    private void CheckBanish()
    {
        var checker = _query.Query().
            Select(x => (IBanishable)x).
            Where(x => x != null && x.canBanish).ToList();

        if (!checker.Any() || _typeManager.sequenceGenerated) return;
        _typeManager.GenerateNewSequence(8);
        Player.Instance.DipposeControls();
        foreach (var entity in checker)
        {
             entity.StartBanish();
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, banishRadius);
    }
}
