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
    private bool _collidedWithPlayer;
    private bool relocating; // <-- NUEVO

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        relocated = true;
        _relocatedSpeed = normalSpeed / 2;
        _actualSpeed = normalSpeed;
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
            relocating = false; // <-- deja de estar en relocate
        }
    }

    private void FixedUpdate()
    {
        if (actualTarget == null) return;
        if (_collidedWithPlayer && relocating) return; // <-- solo si está relocando
        MoveObject(actualTarget.position);
    }

    private void MoveObject(Vector3 target)
    {
        if (relocated) return;

        if (Vector3.Distance(transform.position, target) < .2f)
        {
            transform.position = target;
            if (target == initialPos.position && !relocated)
            {
                relocated = true;
                gameObject.layer = 8;
                PlayerHandler.Instance.PossesPlayer();
            }

            return;
        }

        Vector3 dir = target - _rb.position;
        dir.y = 0; // Ignorar eje Y si hace falta
        dir /= Time.fixedDeltaTime;
        dir = Vector3.ClampMagnitude(dir, _actualSpeed);
        _rb.velocity = dir;
    }

    public void LockDoor()
    {
        actualTarget = finalPos;
        relocated = false;
        gameObject.layer = 9;
        _collidedWithPlayer = false;
        relocating = false;
    }

    public void RelocateItem()
    {
        onHand = true;
        if (!relocated) PlayerHandler.Instance.movingObject = true;
        actualTarget = initialPos;
        _actualSpeed = _relocatedSpeed;
        _collidedWithPlayer = false;
        relocating = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (relocating && collision.gameObject.CompareTag("Player"))
        {
            _collidedWithPlayer = true;
            _rb.velocity = Vector3.zero;
        }
    }

    public void OnInteractItem() { }
    public void OnInteract(bool hit, RaycastHit i) { }
    public void OnInteractWithObject() { }
    public string ShowText() => "Hold to Relocation";
    public bool CanShowText() => false;
}
