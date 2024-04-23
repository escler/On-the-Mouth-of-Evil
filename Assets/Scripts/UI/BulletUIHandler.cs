using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BulletUIHandler : MonoBehaviour
{
    private TextMeshProUGUI _tmp;
    [SerializeField] private RangedWeapon _actualWeapon;

    private void Start()
    {
        _tmp = GetComponentInChildren<TextMeshProUGUI>();
        _actualWeapon = Player.Instance.activeWeapon;
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
