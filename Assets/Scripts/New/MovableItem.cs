using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovableItem : MonoBehaviour, IInteractable
{
    public Transform initialPos, finalPos;
    private Transform actualTarget;
    private Rigidbody _rb;
    public float speed;
    private bool onHand;
    private bool canMove;
    private bool relocated;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        relocated = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && !relocated)
        {
            onHand = false;
            actualTarget = finalPos;
        }
    }

    private void FixedUpdate()
    {
        if (actualTarget == null) return;
        MoveObject(actualTarget.position);
    }


    private void MoveObject(Vector3 target)
    {
        if (relocated) return;
        
        if (Vector3.Distance(transform.position, target) < .1f)
        {
            if (target == initialPos.position && !relocated)
            {
                relocated = true;
                gameObject.layer = 8;
            }
            return;
        }
        Vector3 dir = target - _rb.position;
        // Get the velocity required to reach the target in the next frame
        dir /= Time.fixedDeltaTime;
        // Clamp that to the max speed
        dir = Vector3.ClampMagnitude(dir, speed);
        // Apply that to the rigidbody
        _rb.velocity = dir;
    }

    public void LockDoor()
    {
        print("ASd");
        actualTarget = finalPos;
        relocated = false;
        gameObject.layer = 9;
    }
    
    public void RelocateItem()
    {
        onHand = true;
        actualTarget = initialPos;
    }

    public void OnInteractItem()
    {
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public string ShowText()
    {
        return "Hold to Relocation";
    }
}
