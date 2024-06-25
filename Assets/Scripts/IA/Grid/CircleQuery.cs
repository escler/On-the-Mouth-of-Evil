using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircleQuery : MonoBehaviour, IQuery
{
    public SpatialGrid targetGrid;
    public float radius;
    public IEnumerable<IGridEntity> selected = new List<IGridEntity>();
    
    public IEnumerable<IGridEntity> Query()
    {
        return targetGrid.Query(transform.position + new Vector3(-radius, 0, -radius),
        transform.position + new Vector3(radius,  0, radius),
        x => Vector3.Distance(x,transform.position) < radius);
    }

    private void OnDrawGizmos()
    {
        if (targetGrid == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius * radius);
    }
}
