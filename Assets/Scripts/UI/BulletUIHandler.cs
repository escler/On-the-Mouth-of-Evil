using TMPro;
using UnityEngine;

public class BulletUIHandler : MonoBehaviour
{
    private TextMeshProUGUI _tmp;
    [SerializeField] private WeaponsHandler _actualWeapon;

    private void Start()
    {
        _tmp = GetComponentInChildren<TextMeshProUGUI>();
        _actualWeapon = Player.Instance.GetComponentInChildren<WeaponsHandler>();
        _tmp.text = _actualWeapon.ActualBullet.ToString();
        _actualWeapon.OnUpdateBulletUI += ChangeValue;
    }

    private void OnDisable()
    {
        _actualWeapon.OnUpdateBulletUI -= ChangeValue;
    }

    public void ChangeValue()
    {
        _tmp.text = _actualWeapon.ActualBullet.ToString();
    }

}
