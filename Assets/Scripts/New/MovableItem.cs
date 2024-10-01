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
    public float normalSpeed;
    private float _relocatedSpeed, _actualSpeed;
    private bool onHand;
    private bool canMove;
    public bool relocated;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        relocated = true;
        _relocatedSpeed = normalSpeed / 2;
        _actualSpeed = normalSpeed;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && !relocated)
        {
            onHand = false;
            actualTarget = finalPos;
            _actualSpeed = normalSpeed;
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
            transform.position = target;
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
        dir = Vector3.ClampMagnitude(dir, _actualSpeed);
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
        _actualSpeed = _relocatedSpeed;
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

    public bool CanShowText()
    {
        return false;
    }
}
