using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    
    public List<Transform> AStar(Node start, Node goal)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(start, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(start, null);

        Dictionary<Node, int> nodeCost = new Dictionary<Node, int>();
        nodeCost.Add(start, 0);

        Node current = default;

        while (frontier.Count != 0)
        {
            current = frontier.Dequeue();
            if(current.blockNode) goal = current;

            if (current == goal)
            {
                break;
            }
            

            foreach (var next in current.GetNeighbors())
            {
                if(next.blocked) continue;
                if (next != null)
                {
                    int newCost = nodeCost[current] + next.Cost;
            
                    if (!nodeCost.ContainsKey(next))
                    {
                      nodeCost.Add(next, newCost);
                      frontier.Enqueue(next, newCost + Heuristic(next.transform.position, goal.transform.position));
                      cameFrom.Add(next, current);
                    }
                    else if (newCost < nodeCost[current])
                    {
                        frontier.Enqueue(next, newCost + Heuristic(next.transform.position, goal.transform.position));
                        nodeCost[next] = newCost;
                        cameFrom[next] = current;
                    }
                }
            }
        }

        List<Transform> path = new List<Transform>();
        if (current != goal)
        {
            return path;
        }
        
        current = cameFrom[current];
        while (current != start && current != null)
        {
            path.Add(current.transform);
            current = cameFrom[current];
        }
        return path;
    }

    float Heuristic (Vector3 start, Vector3 goal) => Vector3.Distance(start, goal);
    
    List<Transform> _empty = new List<Transform>();
    
    public List<Transform>ThetaStar(Node start, Node goal, LayerMask obstacle)
    {
        if (start == null || goal == null) return _empty;

        var path = AStar(start, goal);

        int current = 0;

        while (current + 2 < path.Count)
        {
            if (InLineOfSight(path[current].position, path[current + 2].position, obstacle))
                path.RemoveAt(current + 1);
            else current++;
        }

        return path;
    }
    
    bool InLineOfSight(Vector3 start, Vector3 goal, LayerMask obstacle)
    {
        Vector3 dir = goal - start;
        return !Physics.Raycast(start, dir, dir.magnitude, obstacle);
    }

}
