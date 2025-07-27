using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System.Collections;

public class EndingsText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string[] goodPath;
    [SerializeField] private string[] badPath;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float timeForSkipDialog = 3f;
    [SerializeField] private AudioSource goodEndingMusic, badEndingMusic;

    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage videoDisplay;
    [SerializeField] private VideoClip finalClip;

    private void Awake()
    {
        var textColor = text.color;
        textColor.a = 0;
        text.color = textColor;
        text.gameObject.SetActive(false);

        if (videoDisplay != null)
            videoDisplay.gameObject.SetActive(false);
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

        foreach (var line in lines)
        {
            text.text = line;
            yield return StartCoroutine(FadeText(0, 1));
            yield return new WaitForSeconds(timeForSkipDialog);
            yield return StartCoroutine(FadeText(1, 0));
        }

        text.gameObject.SetActive(false);

        PlayFinalVideo();
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

    private void PlayFinalVideo()
    {
        if (videoPlayer == null || videoDisplay == null || finalClip == null)
        {
            Debug.LogError("Falta VideoPlayer, RawImage o VideoClip.");
            return;
        }

        videoDisplay.gameObject.SetActive(true);

        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = finalClip;

        RenderTexture renderTex = new RenderTexture(Screen.width, Screen.height, 0);
        videoPlayer.targetTexture = renderTex;
        videoDisplay.texture = renderTex;

        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        vp.loopPointReached -= OnVideoFinished;
        GameManagerNew.Instance.LoadSceneWithDelay("Hub", 2f);
        videoDisplay.gameObject.SetActive(false);
    }
}
