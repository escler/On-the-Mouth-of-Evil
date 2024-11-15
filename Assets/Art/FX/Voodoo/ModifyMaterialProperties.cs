using UnityEngine;
using System.Collections;

public class ModifyMaterialProperties : MonoBehaviour
{
    public Material material; // Asigna el material aqu� en el inspector

    // Rango de interpolaci�n para Lerp
    public float startLerpValue = 0f;
    public float endLerpValue = 1f;
    public float lerpDuration = 2f; // Duraci�n total de ida y vuelta para Lerp en segundos

    // Rango de interpolaci�n para Intensity
    public float startIntensityValue = 0f;
    public float endIntensityValue = 5f;
    public float intensityDuration = 3f; // Duraci�n total de ida y vuelta para Intensity en segundos

    private bool isLerping = false;

    private void Update()
    {
        // Detecta si se presion� la barra espaciadora y no est� en proceso de interpolaci�n
        if (Input.GetKeyDown(KeyCode.Space) && !isLerping)
        {
            StartCoroutine(LerpMaterialProperties());
        }
    }

    private IEnumerator LerpMaterialProperties()
    {
        isLerping = true;

        // Duraci�n para ida y vuelta de cada propiedad
        float halfLerpDuration = lerpDuration / 2f;
        float halfIntensityDuration = intensityDuration / 2f;

        float elapsedLerpTime = 0f;
        float elapsedIntensityTime = 0f;

        // Interpolaci�n de ida
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

        // Interpolaci�n de vuelta
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
    }
}
