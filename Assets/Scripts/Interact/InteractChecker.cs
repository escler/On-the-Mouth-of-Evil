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

        var ray = _query.Query().
            Select(x => (IBanishable)x).
            Where(x => x != null && x.canBanish).ToList();//IA2-P2
        
        if (ray.Any())
        {
            UIObject.SetActive(true);
        }
        else
        {
            UIObject.SetActive(false);
        }
        
        if (Input.GetButtonDown("Interact")) MakeBanish(ray);
    }

    private void MakeBanish(List<IBanishable> entities)
    {
        if (!entities.Any() || _typeManager.sequenceGenerated) return;
        _typeManager.GenerateNewSequence(8);
        Player.Instance.DipposeControls();
        BanishManager.Instance.AmountOfEnergy(entities);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, banishRadius);
    }
}
