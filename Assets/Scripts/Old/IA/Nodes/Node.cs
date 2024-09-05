using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Node : MonoBehaviour
{
    [SerializeField] List<Node> _neighbors = new List<Node>();
    [SerializeField] private LayerMask _layerMask;
    public Room room;
    private int _cost = 0;
    public bool blocked;
    public bool blockNode;

    private LineRenderer _lineRenderer;

    public int Cost
    {
        get { return _cost; }
    }

    public bool Blocked => blocked;

    private void OnEnable()
    {
        StartCoroutine(AddNodeWithDelay());
    }

    private void OnDisable()
    {
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
            bool ray = Physics.Raycast(transform.position, direction, out hit,10f, _layerMask);

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

}
