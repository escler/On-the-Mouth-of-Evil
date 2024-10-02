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
        if (ray)
        {
            if (_hit.transform.TryGetComponent(out RitualFloor ritual))
            {
                var paperRitual = Instantiate(paperBible);
                paperRitual.transform.position = RitualManager.Instance.ritualFloor.transform.position;
                paperRitual.GetComponent<BiblePaper>().paperOnRitual = true;
                return;
            }
            if (_hit.transform.gameObject.layer != 19) return;
            var paper = Instantiate(paperBible);
            paper.transform.position = _hit.point + Vector3.up * 0.01f;
            print("Instancie papel");
        }
    }

    private void Update()
    {
        ray = Physics.Raycast(PlayerHandler.Instance.cameraPos.position, PlayerHandler.Instance.cameraPos.forward, out _hit, distance, layer);
        print(ray);
    }
}
