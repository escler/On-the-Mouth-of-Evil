using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AbsorbsionShaderHandler : MonoBehaviour
{
    public static AbsorbsionShaderHandler Instance { get; private set; }
    
    public Material material; // Asigna el material aquí en el inspector

    // Rango de interpolación para Lerp
    public float startLerpValue = 0f;
    public float endLerpValue = 1f;
    public float lerpDuration = 2f; // Duración total de ida y vuelta para Lerp en segundos

    // Rango de interpolación para Intensity
    public float startIntensityValue = 0f;
    public float endIntensityValue = 5f;
    public float intensityDuration = 3f; // Duración total de ida y vuelta para Intensity en segundos

    private bool isLerping = false;

    private int count;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        count = 0;
        SceneManager.sceneLoaded += ResetParameters;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= ResetParameters;
        StopAllCoroutines();
        count = 0;
    }

    private void Update()
    {
        // Detecta si se presionó la barra espaciadora y no está en proceso de interpolación
        if (Input.GetKeyDown(KeyCode.Space) && !isLerping)
        {
            
        }
    }

    public void MakeShaderEffect()
    {
        StartCoroutine(LerpMaterialProperties());
    }

    void ResetParameters(Scene scene, LoadSceneMode loadSceneMode)
    {
        material.SetFloat("_Lerp", 0f);
        material.SetFloat("_Intensity", 100f);
        count = 0;
    }

    private IEnumerator LerpMaterialProperties()
    {
        isLerping = true;

        // Duración para ida y vuelta de cada propiedad
        float halfLerpDuration = lerpDuration / 2f;
        float halfIntensityDuration = intensityDuration / 2f;

        float elapsedLerpTime = 0f;
        float elapsedIntensityTime = 0f;

        // Interpolación de ida
        while (elapsedLerpTime < halfLerpDuration || elapsedIntensityTime < halfIntensityDuration)
        {
            if (elapsedLerpTime < halfLerpDuration)
            {
                float tLerp = elapsedLerpTime / halfLerpDuration;
                material.SetFloat("_Lerp", Mathf.Lerp(startLerpValue, endLerpValue, tLerp));
                elapsedLerpTime += Time.deltaTime;
            }

            if (elapsedIntensityTime < halfIntensityDuration)
            {
                float tIntensity = elapsedIntensityTime / halfIntensityDuration;
                material.SetFloat("_Intensity", Mathf.Lerp(startIntensityValue, endIntensityValue, tIntensity));
                elapsedIntensityTime += Time.deltaTime;
            }

            yield return null;
        }

        // Asegura que ambos valores lleguen al final
        material.SetFloat("_Lerp", endLerpValue);
        material.SetFloat("_Intensity", endIntensityValue);

        // Resetea los tiempos para el regreso
        elapsedLerpTime = 0f;
        elapsedIntensityTime = 0f;

        // Interpolación de vuelta
        while (elapsedLerpTime < halfLerpDuration || elapsedIntensityTime < halfIntensityDuration)
        {
            if (elapsedLerpTime < halfLerpDuration)
            {
                float tLerp = elapsedLerpTime / halfLerpDuration;
                material.SetFloat("_Lerp", Mathf.Lerp(endLerpValue, startLerpValue, tLerp));
                elapsedLerpTime += Time.deltaTime;
            }

            if (elapsedIntensityTime < halfIntensityDuration)
            {
                float tIntensity = elapsedIntensityTime / halfIntensityDuration;
                material.SetFloat("_Intensity", Mathf.Lerp(endIntensityValue, startIntensityValue, tIntensity));
                elapsedIntensityTime += Time.deltaTime;
            }

            yield return null;
        }

        // Reinicia los valores de Lerp e Intensity
        material.SetFloat("_Lerp", 0f);
        material.SetFloat("_Intensity", 100f);

        isLerping = false;

        count++;
        if (count < 3)
        {
            StartCoroutine(LerpMaterialProperties());
        }
    }
}
