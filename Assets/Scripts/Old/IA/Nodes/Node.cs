using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Node : MonoBehaviour
{
    [SerializeField] List<Node> _neighbors = new List<Node>();
    [SerializeField] private LayerMask _layerMask;
    private int _cost = 0;
    private bool blocked;

    private LineRenderer _lineRenderer;

    public int Cost
    {
        get { return _cost; }
    }
    private void Start()
    {
        CheckNeighboors();
    }

    public List<Node> GetNeighbors()
    {
        return _neighbors;
    }

    public void CheckNeighboors()
    {
        if (!blocked)
        {
            foreach (Vector3 direction in PathFindingManager.instance.Directions)
            {
                RaycastHit hit;
                bool ray = Physics.Raycast(transform.position, direction, out hit, _layerMask);

                if (ray)
                {
                    if (hit.collider.gameObject.layer == 6)
                    {
                        continue;
                    }
                    if(_neighbors.Contains(hit.collider.GetComponent<Node>()))continue;
                        
                    _neighbors.Add(hit.collider.GetComponent<Node>());
                }
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            blocked = true;
        }
    }
}
