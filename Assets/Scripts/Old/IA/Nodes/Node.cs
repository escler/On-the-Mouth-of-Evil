using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

public class Node : MonoBehaviour
{
    [SerializeField] List<Node> _neighbors = new List<Node>();
    [SerializeField] private LayerMask _layerMask;
    public Room room;
    private int _cost = 0;
    public bool blocked;
    public bool blockNode;
    public bool doorNode;

    private LineRenderer _lineRenderer;

    public int Cost
    {
        get { return _cost; }
    }

    public bool Blocked => blocked;

    private void OnEnable()
    {
        blocked = false;
        StartCoroutine(AddNodeWithDelay());
    }

    private void OnDisable()
    {
        blocked = true;
        PathFindingManager.instance.Nodes.Remove(this);
    }

    public List<Node> GetNeighbors()
    {
        return _neighbors;
    }

    public void CheckNeighboors()
    {
        foreach (Vector3 direction in PathFindingManager.instance.Directions)
        {
            RaycastHit hit;
            bool ray = Physics.Raycast(transform.position, direction, out hit,3f, _layerMask);

            if (ray)
            {
                if (hit.collider.gameObject.layer == 8)
                {
                    continue;
                }

                if (_neighbors.Contains(hit.collider.GetComponent<Node>())) continue;

                _neighbors.Add(hit.collider.GetComponent<Node>());
            }
        }
        
        if(_neighbors.Count == 0) print(gameObject.name + " No Tiene nodos vecimnos");
    }

    IEnumerator AddNodeWithDelay()
    {
        yield return new WaitForSeconds(1f);
        AddNode();
    }

    void AddNode()
    {
        PathFindingManager.instance.Nodes.Add(this);
    }

    private void OnDrawGizmos()
    {
        foreach (var direction in PathFindingManager.instance.Directions)
        {
            Gizmos.DrawRay(transform.position, direction * 3);
        }
    }
}
