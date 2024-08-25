using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask, _banishLayerMask;
    public int banishRadius;
    private Transform _targetAim, _cameraPos;
    public GameObject UIObject;
    private TypeManager _typeManager;
    private CircleQuery _query;
    public int lenghtWithouthBoss, lenghtWithBoss;

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
        var number = CheckEnemyType(entities);
        _typeManager.GenerateNewSequence(number);
        Player.Instance.DipposeControls();
        BanishManager.Instance.BanishStart(entities);
    }
    
    int CheckEnemyType(IEnumerable<IBanishable> entities) //IA2-P1
    {
        return entities.Select(x => (Enemy)x).Where(x => x != null)
            .Aggregate(lenghtWithouthBoss, (acum, current) =>
            {
                if (current.enemyType == EnemyType.Boss) acum = lenghtWithBoss;

                return acum;
            });
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, banishRadius);
    }
}
