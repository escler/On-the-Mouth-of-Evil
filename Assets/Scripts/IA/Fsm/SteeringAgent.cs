using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SteeringAgent : MonoBehaviour
{
    [SerializeField] protected float _maxSpeed, _maxForce;

    [SerializeField] protected float _viewRadius, _viewRadiusForArrive, _flockingRadius, _separationRadius, _obstacleRay, _viewAngle;
    [SerializeField] protected LayerMask _obstacles;
    protected bool _obstacleDetectionLeft, _obstacleDetectionRight;

    protected Vector3 _velocity;

    protected void Move()
    {
        transform.position += _velocity * Time.deltaTime;
        if (_velocity != Vector3.zero) transform.forward = _velocity;
    }

    protected bool HastToUseObstacleAvoidance(float size)
    {
        Vector3 avoidanceObs = ObstacleAvoidance(size);
        AddForce(avoidanceObs);
        return avoidanceObs != Vector3.zero;
    }

    protected Vector3 Seek(Vector3 targetPos)
    {
        return Seek(targetPos, _maxSpeed);
    }

    protected Vector3 Seek(Vector3 targetPos, float speed)
    {
        Vector3 desired = (targetPos - transform.position).normalized * speed;

        Vector3 steering = desired - _velocity;

        steering = Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);

        return steering;
    }
  
    protected Vector3 Arrive(Vector3 targetPos)
    {
        float dist = Vector3.Distance(transform.position, targetPos);
        if (dist > _viewRadiusForArrive) return Seek(targetPos);

        _velocity = Vector3.zero;
        return _velocity;
    }

    protected Vector3 ObstacleAvoidance(float size)
    {
        size = size / 2;

        if (Physics.Raycast(transform.position - transform.right * size, transform.forward, _obstacleRay, _obstacles))
        {
            return Seek(transform.position + transform.right);
        }
        else if (Physics.Raycast(transform.position + transform.right * size, transform.forward, _obstacleRay, _obstacles))
        {
            return Seek(transform.position - transform.right);
        }
        return Vector3.zero;
    }

    protected bool CheckObstacle(float size)
    {
        _obstacleDetectionRight = Physics.Raycast(transform.position + transform.forward * size, transform.right,
            _obstacleRay, _obstacles);

        _obstacleDetectionLeft = Physics.Raycast(transform.position - transform.forward * size, transform.right,
            _obstacleRay, _obstacles);

        return _obstacleDetectionRight || _obstacleDetectionLeft;
    }

    protected Vector3 Pursuit(SteeringAgent targetAgent)
    {
        Vector3 futurePos = targetAgent.transform.position + targetAgent._velocity;
        Debug.DrawLine(transform.position, futurePos, Color.cyan);
        return Seek(futurePos);
    }

    protected Vector3 Evade(SteeringAgent targetAgent)
    {
        return -Pursuit(targetAgent);
    }
    
    protected Vector3 Alignment(SteeringAgent agent)
    {
        Vector3 desired = Vector3.zero;
        
        desired = agent._velocity;

        return CalculateSteering(desired.normalized * _maxSpeed);
    }

    protected Vector3 Separation(List<SteeringAgent> agents)
    {
        Vector3 desired = Vector3.zero;

        foreach (var item in agents)
        {
            if (item == this) continue;

            Vector3 dist = item.transform.position - transform.position;

            if (dist.sqrMagnitude > _separationRadius * _separationRadius) continue;

            desired += dist;
        }

        if (desired == Vector3.zero) return Vector3.zero;
        desired *= -1;
        return CalculateSteering(desired.normalized * _maxSpeed);
    }

    protected Vector3 CalculateSteering(Vector3 desired)
    {
        return Vector3.ClampMagnitude(desired - _velocity, _maxForce * Time.deltaTime);
    }

    protected void AddForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, _maxSpeed);
    }
    
    public Vector3 GetAngleFromDir(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _separationRadius);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _flockingRadius);

        Gizmos.color = Color.green;

        Vector3 leftRayPos = transform.position + transform.right * transform.localScale.x / 2;
        Vector3 rightRayPos = transform.position - transform.right * transform.localScale.x / 2;
        
        Gizmos.DrawLine(leftRayPos, leftRayPos + transform.forward * _obstacleRay);
        Gizmos.DrawLine(rightRayPos, rightRayPos + transform.forward * _obstacleRay);
        
        Vector3 DirA = GetAngleFromDir(_viewAngle / 2 + transform.eulerAngles.y);
        Vector3 DirB = GetAngleFromDir(-_viewAngle / 2 + transform.eulerAngles.y);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + DirA.normalized * _viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + DirB.normalized * _viewRadius);

    }
}
