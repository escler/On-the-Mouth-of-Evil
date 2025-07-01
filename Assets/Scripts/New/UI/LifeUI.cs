using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LifeUI : MonoBehaviour
{
    private PlayerLifeHandlerNew _lifeHandler;
    public Image lifeUI;
    public Sprite normalCoin, brokedCoin;
    private int actualLife;
    bool _corroutineActive = false;
    
    public float shakeMagnitude = 10f;

    private Vector3 originalPosition;
    private bool _shaking;

    private void CalculateShake()
    {
        float baseValue = 1920f;
        shakeMagnitude *= (Screen.width / baseValue);
    }
    private void Unshake()
    {
        _shaking = false;
    }
    
    private void Shake()
    {
        if (_corroutineActive) return;
        _shaking = true;
        StartCoroutine(ShakeUI());
    }

    private IEnumerator ShakeUI()
    {
        _corroutineActive = true;
        float time = 0f;

        while (_shaking)
        {
            float xOffset = Random.Range(-shakeMagnitude, shakeMagnitude);
            float yOffset = Random.Range(-shakeMagnitude, shakeMagnitude);
            Vector3 newPosition = new Vector3(xOffset, yOffset, 0f) + originalPosition;
            transform.GetChild(0).localPosition = newPosition;

            time += Time.deltaTime;
            yield return null;
        }

        _corroutineActive = false;
        transform.GetChild(0).localPosition = originalPosition;
    }

    private void Awake()
    {
        StartCoroutine(WaitCor()); 
        EnableUI(SceneManager.GetActiveScene(),LoadSceneMode.Single);
        originalPosition = transform.GetChild(0).localPosition;
        CalculateShake();
    }

    IEnumerator WaitCor()
    {
        yield return new WaitForSeconds(0.1f);
        _lifeHandler = PlayerLifeHandlerNew.Instance;
        _lifeHandler.OnLifeChange += ChangeUI;
        actualLife = _lifeHandler.ActualLife;
        SceneManager.sceneLoaded += ResetUI;
        SceneManager.sceneLoaded += EnableUI;
        PlayerHandler.Instance.OnPlayerInDanger += Shake;
        PlayerHandler.Instance.OnPlayerInDangerEnd += Unshake;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ResetUI;
        SceneManager.sceneLoaded -= EnableUI;
        if(_lifeHandler != null) _lifeHandler.OnLifeChange -= ChangeUI;
        PlayerHandler.Instance.OnPlayerInDanger -= Shake;
        PlayerHandler.Instance.OnPlayerInDangerEnd -= Unshake;
    }

    private void ChangeUI()
    {
        actualLife = _lifeHandler.ActualLife;
        
        lifeUI.sprite = brokedCoin;
        PlayerHandler.Instance.PlayerEndDanger();
    }

    private void ResetUI(Scene scene, LoadSceneMode loadSceneMode)
    {
        lifeUI.sprite = normalCoin;
    }

    private void EnableUI(Scene scene, LoadSceneMode loadSceneMode)
    {
        var inHubScene = scene.name == "Hub";

        lifeUI.enabled = !inHubScene;
    }
}
