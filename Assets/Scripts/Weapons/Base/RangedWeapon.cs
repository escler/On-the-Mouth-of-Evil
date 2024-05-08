using Cinemachine;
using UnityEngine;


public abstract class RangedWeapon : Weapon
{
    public int bulletsPerCharge;
    public int initialMaxAmmo;
    private int _chargerBullets, _maxBullets, _actualBullets;
    protected WeaponFeedback _weaponFeedback;
    private bool _aiming;
    private CinemachineFreeLook _cmf;
    protected Transform cameraPos, targetAim;
    [SerializeField] private Crosshair _crosshair;
    protected float actualCd;
    public float reloadTime;
    private float _actualReloadCd;
    private WeaponsHandler _weaponsHandler;
    [SerializeField] private GunType _gunType;

    public int ChargerBullets => _chargerBullets;

    public int MaxBullets
    {
        get { return _maxBullets; }
        set { _maxBullets = value; }
    }
    private void Start()
    {
        cameraPos = Camera.main.transform;
        targetAim = Player.Instance.targetAim;
        _cmf = FindObjectOfType<CinemachineFreeLook>();
        _weaponFeedback = GetComponent<WeaponFeedback>();
        AmmoHandler.Instance.AddBullet(_gunType,initialMaxAmmo);
        ObtainedBullet();
        _chargerBullets = bulletsPerCharge;
        _weaponsHandler.RefreshData();
    }

    protected void OnUpdate()
    {
        _aiming = Input.GetMouseButton(1);
        if (actualCd > 0) actualCd -= Time.deltaTime;
        if (_actualReloadCd > 0) _actualReloadCd -= Time.deltaTime;
        if (_aiming)
        {
            _crosshair.OnAim();
            _cmf.GetComponent<CameraMovement>().SetCameraMode(CameraMovement.CameraMode.Aim);
            Aim();
            if (Input.GetMouseButtonDown(0) && actualCd <= 0 && _chargerBullets > 0)
            {
                Shoot();
                _chargerBullets--;
                _weaponsHandler.RefreshData();
                _weaponFeedback.FireParticle();
            }
        }
        else
        {
            _crosshair.OffAim();
            _cmf.GetComponent<CameraMovement>().SetCameraMode(CameraMovement.CameraMode.Normal);
        }
    }

    public void Reload()
    {
        if(_actualReloadCd > 0 || _chargerBullets == bulletsPerCharge || _maxBullets == 0) return;

        var bulletsToCharge = bulletsPerCharge - _chargerBullets;
        _chargerBullets = bulletsPerCharge;
        _maxBullets -= bulletsToCharge;
        
        
        AmmoHandler.Instance.UpdateMaxAmount(_gunType,_maxBullets);
        
        _actualReloadCd = reloadTime;
        _weaponsHandler.RefreshData();
    }
    
    private void OnDisable()
    {
        model.SetActive(false);
        _actualBullets = _chargerBullets;
        _weaponsHandler.OnUpdateBulletUI -= ObtainedBullet;
    }

    private void OnEnable()
    {
        model.SetActive(true);
        _weaponsHandler = GetComponent<WeaponsHandler>();
        _weaponsHandler.OnUpdateBulletUI += ObtainedBullet;
        _chargerBullets = _actualBullets;
        _weaponsHandler.RefreshData();
    }

    public abstract void ObtainedBullet();
    
    protected abstract void Aim();

    protected abstract void Shoot();
}
