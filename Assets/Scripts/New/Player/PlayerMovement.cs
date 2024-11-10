using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed, runSpeed;
    private float _actualSpeed, _inputX, _inputY;
    private Rigidbody _rb;
    private bool _run;
    private AudioSource _walkAudioSource;
    public bool ritualCinematic;
    public bool inSpot;
    private Vector3 reference = Vector3.zero;
    public bool Run => _run;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
        _inputY = Input.GetAxisRaw("Vertical");
        _run = Input.GetButton("Run");
        HandleWalkSound();
    }

    private void FixedUpdate()
    {
        Movement();
        MoveToRitualSpot();
    }

    private void Movement()
    {
        if (ritualCinematic) return;
        _actualSpeed = _run ? runSpeed : walkSpeed;

        var inputVector = new Vector2(_inputX, _inputY);
        Vector3 velocity = transform.forward * inputVector.y + transform.right * inputVector.x;
        velocity.Normalize();

        _rb.velocity = velocity * (_actualSpeed * Time.fixedDeltaTime);
    }

    private void HandleWalkSound()
    {
        bool isWalking = _inputX != 0 || _inputY != 0;

        if (isWalking && _walkAudioSource == null)
        {
            MusicManager.Instance.PlaySound("Footsteps-concrete", true, (source) =>
            {
                _walkAudioSource = source;
            });
        }
        else if (!isWalking && _walkAudioSource != null)
        {
            MusicManager.Instance.StopSound(_walkAudioSource);
            _walkAudioSource = null;
        }
    }


    private void MoveToRitualSpot()
    {
        if (!ritualCinematic) return;
        if (inSpot)
        {
            var ritualPos = CameraCinematicHandler.Instance.ritual.position;
            ritualPos.y = transform.position.y;
            transform.LookAt(Vector3.SmoothDamp(transform.position, ritualPos,
                ref reference, 5f));
            return;
        }
        var target = CameraCinematicHandler.Instance.transform.position;
        target.y = transform.position.y;

        if (Vector3.Distance(transform.position, target) < 0.3f)
        {
            inSpot = true;
            return;
        }
        
        PlayerHandler.Instance.bobbingCamera.DoBobbing();
        transform.LookAt(Vector3.SmoothDamp(transform.position, target, ref reference, 5f));
        Vector3 velocity = transform.forward * .5f;
        velocity.Normalize();
        _rb.velocity = velocity * (_actualSpeed * Time.fixedDeltaTime);

    }

}
