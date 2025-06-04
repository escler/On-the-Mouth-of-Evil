using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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

        StartCoroutine(CheckNodesCor());
        SceneManager.sceneLoaded += CheckNodes;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= CheckNodes;
    }

    public void CheckNodes(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (SceneManager.GetActiveScene().name != "HouseLevel") return;
        StartCoroutine(CheckNodesCor());
    }
    

    IEnumerator CheckNodesCor()
    {
        yield return new WaitForSeconds(3f);
        foreach (var nodes in FindObjectsOfType<Node>())
        {
            nodes.CheckNeighboors();
        }
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

    public Node CalculateOtherRoomNode(Node start)
    {
        if (start == null)
        {
            print("Start null");
            return null;
        }

        var nodesActuals = _nodes.Where(x => x.room != start.room && x.room != HouseEnemy.Instance.crossRoom && !x.doorNode);

        if (nodesActuals.Count() == 0) return null;
        return nodesActuals.ElementAt(Random.Range(0, nodesActuals.Count()));
    }
    
    public Node CalculateOtherRoomNodeMorgue(Node start)
    {
        if (start == null)
        {
            print("Start null");
            return null;
        }

        var nodesActuals = _nodes.Where(x => x.room != start.room && x.room != MorgueEnemy.Instance.crossRoom && !x.doorNode);

        if (nodesActuals.Count() == 0) return null;
        return nodesActuals.ElementAt(Random.Range(0, nodesActuals.Count()));
    }

    public Node CalculateNearnestNodeAndRoom(Vector3 position)
    {

        var closestNode = _nodes.Where(x => x.room == PlayerHandler.Instance.actualRoom && !x.doorNode);
        Node[] nodes = closestNode.OrderBy(x => (position - x.transform.position).sqrMagnitude).ToArray();
        
        for (int i = 0; i < nodes.Length; i++)
        {
            if (Vector3.Distance(position, nodes[i].transform.position) < 2)
            {
                print("Continuo");
                continue;
            }
            
            print("Retorne el nodo " + i);
            return nodes[i];
        }

        return nodes.Length == 0 ? null : nodes.First();
    }

    public Node CalculateFarthestNodeAndRoom(Vector3 position)
    {
        var closestNode = _nodes.Where(x => x.room == PlayerHandler.Instance.actualRoom && !x.doorNode);
        Node[] nodes = closestNode.OrderBy(x => (position - x.transform.position).sqrMagnitude).ToArray();
        var nodesFarthest = nodes.Reverse().ToArray();
        
        for (int i = 0; i < nodesFarthest.Length; i++)
        {
            if (Vector3.Distance(position, nodes[i].transform.position) < 6)
            {
                print("Continuo");
                continue;
            }
            
            print("Retorne el nodo " + i);
            return nodes[i];
        }

        return nodes.Length == 0 ? null : nodes.First();
        
    }
}