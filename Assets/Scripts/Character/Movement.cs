using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Controller _controller;
    [SerializeField] private Transform _mainCamera;
    private Rigidbody _rb;
    private Animator _animator;
    public Transform model;
    private bool running;

    public float walkSpeed, runSpeed, sensRot;
    private float _actualSpeed;
    private bool _aiming;
    private Transform _targetAim, _weaponPos;
    public Transform spine;


    private void Start()
    {
        _targetAim = Player.Instance.targetAim;
        _weaponPos = Player.Instance.chest;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _actualSpeed = walkSpeed;
        _targetAim = GetComponentInChildren<CenterPointCamera>().transform;
        _animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        Move();
        Rotate();
        _aiming = Input.GetMouseButton(1);
    }

    private void Update()
    {
        _animator.SetBool("Walking",_rb.velocity != Vector3.zero);
        _animator.SetBool("Aiming",_aiming);
        RunCheck();
    }

    /*private void LateUpdate()
    {
        if (_aiming)
        {
            //RotateSpine();
        }

    }*/


    private void Move()
    {
        Vector3 vel = transform.forward * (_controller.GetMovementInput().x * _actualSpeed * Time.fixedDeltaTime) +
                      transform.right * (_controller.GetMovementInput().z * _actualSpeed * Time.fixedDeltaTime);
        _rb.velocity = vel;
    }

    private void Rotate()
    {
        if (!_aiming)
        {
            if (_rb.velocity != Vector3.zero)
            {
                var newRot = Quaternion.Euler(0, _mainCamera.transform.eulerAngles.y, 0);
                transform.rotation = newRot;
            }
        }
        else
        {
            Vector3 aimVector = _targetAim.position - _weaponPos.position;
            Quaternion rotation = Quaternion.LookRotation(aimVector,transform.up);
            var newRot = transform.rotation;
            newRot.y = rotation.y;
            transform.rotation = rotation;
            transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);

        }
    }
    private void RotateSpine()
    {
        var rotation = spine.rotation;
        Quaternion rot = Quaternion.LookRotation(_targetAim.position - _weaponPos.position,Vector3.up);
        Quaternion rot1 = Quaternion.FromToRotation(spine.forward, _targetAim.position - _weaponPos.position);
        rotation.x = rot.x;

        //spine.rotation = rotation;
        spine.localEulerAngles = new Vector3(rotation.eulerAngles.x, 0, 0);
    }

    public void RunCheck()
    {
        if (Input.GetButton("Run")) _actualSpeed = runSpeed;
        else _actualSpeed = walkSpeed;
    }
}
