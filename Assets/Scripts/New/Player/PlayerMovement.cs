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
    public bool ritualCinematic;
    public bool inSpot;
    private Vector3 reference = Vector3.zero;
    public bool absorbEnd;
    public bool inVoodooPos;
    public bool voodooMovement;
    public AudioSource walking;
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

    private void OnEnable()
    {
        _rb.velocity = Vector3.zero;
    }

    private void OnDisable()
    {
        _rb.velocity = Vector3.zero;
        HandleWalkSound();
    }

    private void FixedUpdate()
    {
        Movement();
        MoveToRitualSpot();
    }

    private void Movement()
    {
        if (ritualCinematic || absorbEnd) return;
        _actualSpeed = _run ? runSpeed : walkSpeed;

        var inputVector = new Vector2(_inputX, _inputY);
        Vector3 velocity = transform.forward * inputVector.y + transform.right * inputVector.x;
        velocity.Normalize();

        _rb.velocity = velocity * (_actualSpeed * Time.fixedDeltaTime);
    }

    private void HandleWalkSound()
    {
        float vel = _rb.velocity.x + _rb.velocity.z;
        bool isWalking = vel > 0.1 || vel < -0.1;
        
        if(isWalking && !walking.isPlaying) walking.Play();
        
        if(!isWalking && walking.isPlaying) walking.Stop();
    }

    public void GoToVoodoo(Vector3 item)
    {
        StartCoroutine(MoveToDoll(item));
    }

    private void MoveToRitualSpot()
    {
        if (!ritualCinematic) return;
        if (absorbEnd) return;
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

    IEnumerator MoveToDoll(Vector3 item)
    {
        voodooMovement = true;
        Vector3 target = item;
        target.y = transform.position.y;
        Vector3 originalEuler = transform.position;
        float ticks = 0;
        while (Vector3.Distance(transform.position, target) > 1f)
        {
            PlayerHandler.Instance.bobbingCamera.DoBobbing();
            ticks += Time.deltaTime;
            transform.LookAt(Vector3.Lerp(originalEuler, target, ticks));
            Vector3 velocity = transform.forward;
            velocity.Normalize();
            _rb.velocity = velocity * (_actualSpeed * .5f * Time.fixedDeltaTime);
            yield return null;
        }

        voodooMovement = false;
        
        inVoodooPos = true;
        
    }

}
