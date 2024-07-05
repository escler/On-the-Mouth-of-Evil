using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.PlayerLoop;

public class Movement : MonoBehaviour
{
    [SerializeField] private Controller _controller;
    [SerializeField] private Transform _mainCamera;
    private Rigidbody _rb;
    public Transform model;
    public bool running, isDashing;
    private PlayerView _view;

    public float walkSpeed, runSpeed, dashSpeed, dashTime;
    private float _actualSpeed, _dashSpeed;
    private bool _aiming, _canDash;
    public bool cantMove;
    
    private void Start()
    {
        _dashSpeed = dashSpeed;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _actualSpeed = walkSpeed;
        _view = GetComponent<PlayerView>();
    }
    private void LateUpdate()
    {
        model.transform.localPosition = new Vector3(0, -1, 0);
        model.transform.localRotation = Quaternion.identity;
    }

    void FixedUpdate()
    {
        Rotate();
        Move();
    }

    private void Update()
    {
        _aiming = Input.GetMouseButton(1);

        if (!isDashing && Input.GetButtonDown("Dash")) Dash();
        RunCheck();
    }

    private void Move()
    {
        if (cantMove)
        {
            _rb.velocity = Vector3.zero;
            return;
        }

        if (isDashing) return;
        Vector3 vel = transform.forward * (_controller.GetMovementInput().x * _actualSpeed * Time.fixedDeltaTime) +
                      transform.right * (_controller.GetMovementInput().z * _actualSpeed * Time.fixedDeltaTime);

        _rb.velocity = vel;
    }

    private void Rotate()
    {
        _rb.MoveRotation(Quaternion.Euler(0, _mainCamera.transform.eulerAngles.y, 0));
    }

    private void RunCheck()
    {
        if (Input.GetButton("Run") && !_aiming) _actualSpeed = runSpeed;
        else _actualSpeed = walkSpeed;
    }

    private void Dash()
    {
        StartCoroutine(DashAbility());
    }

    IEnumerator DashAbility()
    {
        isDashing = true;
        var dashDirection = transform.forward * _controller.GetMovementInput().x +
                            transform.right * _controller.GetMovementInput().z;
        _rb.velocity = dashDirection * (dashSpeed * Time.fixedDeltaTime);
        _view.ActivateTrail();

        yield return new WaitForSeconds(dashTime);

        _view.DeactivateTrail();
        isDashing = false;
        _canDash = false;

    }
}
