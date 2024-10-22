using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bible : Item
{
    private bool _placed;
    public float timeToBurn;
    public GameObject firePS;
    private MaterialPropertyBlock _burning;
    private MeshRenderer _mesh;
    public GameObject paperBible;
    private RaycastHit _hit;
    public LayerMask layer;
    public float distance;
    private bool ray;
    private BibleCD _bibleCD;

    
    private void Start()
    {
        _bibleCD = PlayerHandler.Instance.GetComponent<BibleCD>();
    }

    public override void OnGrabItem()
    {
        base.OnGrabItem();
        transform.localEulerAngles = angleHand;
    }

    private void Awake()
    {
        _burning = new MaterialPropertyBlock();
        _mesh = GetComponentInChildren<MeshRenderer>();
        _mesh.SetPropertyBlock(_burning);
        _burning.SetInt("_BurningON", 0);
    }

    public override void OnInteract(bool hit, RaycastHit i)
    {
        base.OnInteract(hit,i);
        if (_bibleCD.Cooldown > 0) return;
        if (ray)
        {
            if (_hit.transform.TryGetComponent(out RitualFloor ritual))
            {
                var paperRitual = Instantiate(paperBible);
                paperRitual.transform.position = RitualManager.Instance.ritualFloor.transform.position;
                paperRitual.GetComponent<BiblePaper>().paperOnRitual = true;
                _bibleCD.SetCooldown(10);
                return;
            }
            if (_hit.transform.gameObject.layer != 19) return;
            var paper = Instantiate(paperBible);
            paper.transform.position = _hit.point + Vector3.up * 0.01f;
            _bibleCD.SetCooldown(10);
            print("Instancie papel");
        }
    }
    
    public override bool CanInteractWithItem()
    {
        if (!ray || _bibleCD.Cooldown > 0) return false;
        
        if (_hit.transform.gameObject.layer == 19 || _hit.transform.TryGetComponent(out RitualFloor ritualFloor)) return true;

        return false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        ray = Physics.Raycast(PlayerHandler.Instance.cameraPos.position, PlayerHandler.Instance.cameraPos.forward, out _hit, distance, layer);
        canInteractWithItem = CanInteractWithItem();
        ChangeCrossHair();
        if (Input.GetMouseButtonDown(0)) OnInteract(ray, _hit);
    }
}
