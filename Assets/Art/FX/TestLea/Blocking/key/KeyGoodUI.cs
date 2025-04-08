using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyGoodUI : ItemUI
{
    [SerializeField] private Sprite[] sprites;
    private void OnEnable()
    {
        _image = GetComponent<Image>();
    }

    public void ChangeUI(int i)
    {
        _image.sprite = sprites[i];
    }
}
