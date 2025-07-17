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

        FreezePosition(true); // Inicia congelado
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (onHand)
            {
                PlayerHandler.Instance.PossesPlayer();
            }

            PlayerHandler.Instance.movingObject = false;
            onHand = false;
            actualTarget = finalPos;
            _actualSpeed = normalSpeed;

            FreezePosition(false); // Liberar para moverse
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

        // Ignorar eje Y
        Vector3 flatTarget = new Vector3(target.x, _rb.position.y, target.z);
        Vector3 flatPosition = new Vector3(transform.position.x, _rb.position.y, transform.position.z);

        if (Vector3.Distance(flatPosition, flatTarget) < .1f)
        {
            transform.position = flatTarget;
            _rb.velocity = Vector3.zero;
            FreezePosition(true); // Congela al llegar

            if (flatTarget == new Vector3(initialPos.position.x, _rb.position.y, initialPos.position.z) && !relocated)
            {
                relocated = true;
                gameObject.layer = 8;
                PlayerHandler.Instance.PossesPlayer();
            }

            return;
        }

        Vector3 dir = flatTarget - _rb.position;
        dir /= Time.fixedDeltaTime;
        dir = Vector3.ClampMagnitude(dir, _actualSpeed);
        _rb.velocity = dir;
    }

    public void LockDoor()
    {
        actualTarget = finalPos;
        relocated = false;
        gameObject.layer = 9;

        FreezePosition(false); // Permite moverse
    }

    public void RelocateItem()
    {
        onHand = true;
        if (!relocated) PlayerHandler.Instance.movingObject = true;
        actualTarget = initialPos;
        _actualSpeed = _relocatedSpeed;

        FreezePosition(false); // Permite moverse
    }

    private void FreezePosition(bool freeze)
    {
        if (freeze)
        {
            _rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            _rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotation;
        }
    }

    public void OnInteractItem() { }

    public void OnInteract(bool hit, RaycastHit i) { }

    public void OnInteractWithObject() { }

    public string ShowText()
    {
        return "Hold to Relocation";
    }

    public bool CanShowText()
    {
        return false;
    }
}
