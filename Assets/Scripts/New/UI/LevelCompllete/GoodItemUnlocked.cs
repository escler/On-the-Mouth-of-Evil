using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoodItemUnlocked : MonoBehaviour
{
    [SerializeField] public GameObject lockedChain, blackEffect;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Image _image;
    [SerializeField] private bool badItem;

    private void OnEnable()
    {
        CheckSpriteItem();
        CheckUnlocked();
    }

    private void CheckUnlocked()
    {
        if (badItem) CheckLogicBad();
        else CheckLogicGood();
    }

    private void CheckLogicGood()
    {
        string name = SceneManager.GetActiveScene().name;
        var unlocked = name == "HouseLevel"
            ? PlayerPrefs.GetInt("RosaryAvaible")
            : PlayerPrefs.GetInt("InciensoAvaible");

        if (unlocked == 1)
        {
            lockedChain.SetActive(false);
            blackEffect.SetActive(false);
        }
        
    }

    public void DoAnimation()
    {
        StartCoroutine(UnlockItem());
    }

    private IEnumerator UnlockItem()
    {
        yield return new WaitForSeconds(3f);
        lockedChain.SetActive(false);
    }

    private void CheckLogicBad()
    {
        
        string name = SceneManager.GetActiveScene().name;
        var unlocked = name == "HouseLevel"
            ? PlayerPrefs.GetInt("VoodooAvaible")
            : PlayerPrefs.GetInt("SwarmAvaible");

        if (unlocked == 1)
        {
            lockedChain.SetActive(false);
            blackEffect.SetActive(false);
        }
    }

    private void CheckSpriteItem()
    {
        string name = SceneManager.GetActiveScene().name;

        _image.sprite = name == "HouseLevel" ? _sprites[0] : _sprites[1];
    }
}
