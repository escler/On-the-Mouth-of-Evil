using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float walkSpeed, runSpeed, _actualSpeed;
    private float _inputX, _inputY;
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
        if (ritualCinematic || absorbEnd)
        {
            _actualSpeed = walkSpeed;
            return;
        }
        _actualSpeed = _run ? runSpeed : walkSpeed;

        var inputVector = new Vector2(_inputX, _inputY);
        Vector3 velocity = transform.forward * inputVector.y + transform.right * inputVector.x;
        velocity.Normalize();

        _rb.velocity = velocity * (_actualSpeed * Time.fixedDeltaTime);
    }

    private void HandleWalkSound()
    {
        float vel = new Vector2(_rb.velocity.x, _rb.velocity.z).sqrMagnitude;
        bool isWalking = vel > 0.1f;
        
        if(isWalking && !walking.isPlaying) walking.Play();
        
        if(!isWalking && walking.isPlaying) walking.Stop();
    }

    public void GoToVoodoo(Vector3 item)
    {
        StartCoroutine(MoveToDoll(item));
    }

    private void MoveToRitualSpot()
    {
        if (!ritualCinematic || absorbEnd) return;

        Vector3 targetPosition = inSpot
            ? CameraCinematicHandler.Instance.ritual.position
            : CameraCinematicHandler.Instance.transform.position;

        targetPosition.y = transform.position.y;

        RotateTowards(targetPosition);

        if (inSpot) return;

        if (Vector3.Distance(transform.position, targetPosition) < 0.3f)
        {
            inSpot = true;
            _rb.velocity = Vector3.zero;
            return;
        }

        PlayerHandler.Instance.bobbingCamera.DoBobbing();
        MoveTowards(targetPosition);
    }

    private void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.5f);
    }

    private void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        _rb.velocity = direction * (_actualSpeed * Time.fixedDeltaTime);
    }

    IEnumerator MoveToDoll(Vector3 item)
    {
        float originalSpeed = walkSpeed;
        walkSpeed *= 0.5f;
        voodooMovement = true;
        inVoodooPos = false;

        Vector3 target = item;
        target.y = transform.position.y;

        while (Vector3.Distance(transform.position, target) > 1f)
        {
            PlayerHandler.Instance.bobbingCamera.DoBobbing();

            RotateTowards(target);
            MoveTowards(target);

            yield return new WaitForFixedUpdate(); // mejor que null si usás Rigidbody
        }

        walkSpeed = originalSpeed;
        _rb.velocity = Vector3.zero;
        voodooMovement = false;
        inVoodooPos = true;
    }
}
