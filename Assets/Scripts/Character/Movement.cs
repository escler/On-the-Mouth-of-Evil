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

    public float walkSpeed, runSpeed, dashSpeed, sensRot;
    private float _actualSpeed, _dashSpeed;
    private bool _aiming, _canDash;
    private Transform _targetAim, _weaponPos;
    public Transform spine;

    private void Start()
    {
        _targetAim = Player.Instance.targetAim;
        _weaponPos = Player.Instance.chest;
        _dashSpeed = dashSpeed;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _actualSpeed = walkSpeed;
        _targetAim = GetComponentInChildren<CenterPointCamera>().transform;
    }

    private void LateUpdate()
    {        
        model.transform.localPosition = new Vector3(0, -1, 0);
        model.transform.localRotation = Quaternion.identity;
    }

    void FixedUpdate()
    {

        if(_canDash) Dash();
        Rotate();
        Move();
    }

    private void Update()
    {
        _aiming = Input.GetMouseButton(1);

        if (!isDashing) _canDash = Input.GetButton("Dash");
        RunCheck();
    }

    private void Move()
    {
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
        if (Input.GetButton("Run")) _actualSpeed = runSpeed;
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
        //_rb.AddForce(dashDirection * _dashSpeed * Time.deltaTime, ForceMode.Impulse);

        yield return new WaitForSeconds(0.3f);

        isDashing = false;

    }
}
