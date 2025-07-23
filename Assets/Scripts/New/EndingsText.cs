using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingsText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string[] goodPath;
    [SerializeField] private string[] badPath;
    [SerializeField] private GameObject logo;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float fadeLogoDuration = 2f;
    [SerializeField] private float timeForSkipDialog = 3f;
    [SerializeField] private Vector3 logoStartScale = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField] private Vector3 logoEndScale = Vector3.one;
    [SerializeField] private AudioSource goodEndingMusic, badEndingMusic;

    private CanvasGroup logoCanvasGroup;

    private void Awake()
    {
        // Aseguramos que el logo tenga CanvasGroup para controlar alpha
        logoCanvasGroup = logo.GetComponent<CanvasGroup>();
        if (logoCanvasGroup == null)
            logoCanvasGroup = logo.AddComponent<CanvasGroup>();

        logoCanvasGroup.alpha = 0;
        logo.SetActive(false);

        // Inicializamos escala y transparencia del logo
        logo.transform.localScale = logoStartScale;

        var textColor = text.color;
        textColor.a = 0;
        text.color = textColor;
        text.gameObject.SetActive(false);
    }

    public void ShowGoodEnding()
    {
        StartCoroutine(PlayEnding(goodPath));
        goodEndingMusic.Play();
    }

    public void ShowBadEnding()
    {
        StartCoroutine(PlayEnding(badPath));
        badEndingMusic.Play();
    }

    private IEnumerator PlayEnding(string[] lines)
    {
        text.gameObject.SetActive(true);
        logo.SetActive(false);

        foreach (var line in lines)
        {
            text.text = line;
            yield return StartCoroutine(FadeText(0, 1));
            yield return new WaitForSeconds(timeForSkipDialog);
            yield return StartCoroutine(FadeText(1, 0));
        }

        text.gameObject.SetActive(false);
        logo.SetActive(true);
        yield return StartCoroutine(FadeAndScaleLogo());
    }

    private IEnumerator FadeText(float from, float to)
    {
        float time = 0f;
        Color color = text.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / fadeDuration);
            color.a = Mathf.Lerp(from, to, t);
            text.color = color;
            yield return null;
        }
        color.a = to;
        text.color = color;
    }

    private IEnumerator FadeAndScaleLogo()
    {
        float time = 0f;

        while (time < fadeLogoDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / fadeLogoDuration);

            // Fade alpha
            logoCanvasGroup.alpha = Mathf.Lerp(0, 1, t);

            // Scale up
            logo.transform.localScale = Vector3.Lerp(logoStartScale, logoEndScale, t);

            yield return null;
        }

        logoCanvasGroup.alpha = 1;
        logo.transform.localScale = logoEndScale;

        yield return new WaitForSeconds(5f);
        GameManagerNew.Instance.LoadSceneWithDelay("Hub", 2f);
        logo.SetActive(false);
    }
}
