using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

//Es opcional hacerlo, este script va a mostrar el pathfinding en este proyecto.
public class PathFindingManager : MonoBehaviour
{
    public static PathFindingManager instance;
    
    [SerializeField] private LayerMask _layerMask;
    
    [SerializeField] private List<Node> _nodes = new List<Node>();

    [SerializeField] private Vector3[] _directions;

    public List<Node> Nodes
    {
        get
        {
            return _nodes;
        }
    }

    public Vector3[] Directions
    {
        get
        {
            return _directions;
        }
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        /*foreach (var nodes in FindObjectsOfType<Node>())
        {
            _nodes.Add(nodes.GetComponent<Node>());
        }*/
        
    }
    

    public Node CalculateDistance(Vector3 position)
    {
        var nodes = _nodes;

        bool _firstNode = false;

        Node _nearstNode = null;
        float _nearDistance = 0;
        
        foreach (var node in nodes)
        {
            RaycastHit hit;
            bool ray = Physics.Raycast(position, (node.transform.position - position), out hit,
                (node.transform.position - position).magnitude, _layerMask);

            if (ray)
            {
                if (hit.collider.gameObject.layer != 8)
                {
                    var nodeDistance = Vector3.Distance(node.transform.position, position);

                    if (!_firstNode)
                    {
                        _nearstNode = node;
                        _nearDistance = nodeDistance;
                        _firstNode = true;
                    }
                    else
                    {
                        if (nodeDistance < _nearDistance)
                        {
                            _nearstNode = node;
                            _nearDistance = nodeDistance;
                        }
                    }
                }
            }
        }
        return _nearstNode;
    }

    public Node CalculateFarthest(Vector3 position)
    {
        var nodes = _nodes;

        bool _firstNode = false;

        Node _farNode = null;
        float _farDistance = 0;
        
        foreach (var node in nodes)
        {
            RaycastHit hit;
            bool ray = Physics.Raycast(position, (node.transform.position - position), out hit,
                (node.transform.position - position).magnitude, _layerMask);

            if (ray)
            {
                if (hit.collider.gameObject.layer != 6)
                {
                    var nodeDistance = Vector3.Distance(node.transform.position, position);

                    if (!_firstNode)
                    {
                        _farNode = node;
                        _farDistance = nodeDistance;
                        _firstNode = true;
                    }
                    else
                    {
                        if (nodeDistance > _farDistance)
                        {
                            _farNode = node;
                            _farDistance = nodeDistance;
                        }
                    }
                }
            }
        }
        return _farNode;
    }
}