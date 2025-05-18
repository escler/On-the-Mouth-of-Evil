using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemUIUnlocked : MonoBehaviour
{
    [SerializeField] private GameObject lockedChain, blackEffect;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Image _image;

    private void OnEnable()
    {
        CheckSpriteItem();
    }

    public void UnlockItem()
    {
        blackEffect.SetActive(true);
        lockedChain.SetActive(true);
        StartCoroutine(UnlockItemCor());
    }

    private IEnumerator UnlockItemCor()
    {
        yield return new WaitForSeconds(.5f);
        float timer = 0f;
        Vector3 originalScale = transform.localScale;
        Vector3 finalScale = originalScale * 1.5f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * 5f;
            transform.localScale = Vector3.Lerp(originalScale, finalScale, timer);
            yield return null;
        }
        blackEffect.SetActive(false);
        lockedChain.SetActive(false);
        timer = 0;
        while (timer < 1f)
        {
            timer += Time.deltaTime * 3f;
            transform.localScale = Vector3.Lerp(finalScale, originalScale, timer);
            yield return null;
        }
    }


    private void CheckSpriteItem()
    {
        string name = SceneManager.GetActiveScene().name;

        _image.sprite = name == "HouseLevel" ? _sprites[0] : _sprites[1];
    }

    public void UnlockedItem(bool state)
    {
        lockedChain.SetActive(!state);
        blackEffect.SetActive(!state);
    }
}
