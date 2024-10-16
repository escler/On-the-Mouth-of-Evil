using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System.Collections;

public class HypnosisEffectControllerHDRP : MonoBehaviour
{
    public static HypnosisEffectControllerHDRP Instance { get; private set; }
    public Material hypnosisMaterialK; // Material para blink con K

    private Material originalSkybox;

    public float lerpDuration = 1.0f;
    private float currentLerpTimeK = 0.0f; // Lerp para K
    private bool isLerpingK = false;

    private float startValue = 1.0f;
    private float endValue = 0.2f;
    private bool hasCompletedCycleK = false;

    private GameObject[] pointLights;
    private GameObject spotLight;

    private bool skyboxIsOn = true; // Indica el estado actual del Skybox

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);

        hypnosisMaterialK.SetFloat("_Blink", startValue);

        pointLights = GameObject.FindGameObjectsWithTag("PointLight");
        spotLight = GameObject.FindGameObjectWithTag("DemonLight");

        DeactivateDemonLight();
        originalSkybox = RenderSettings.skybox;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!isLerpingK)
            {
                isLerpingK = true;
                currentLerpTimeK = 0.0f;
            }
        }

        if (isLerpingK)
        {
            PerformLerp(ref isLerpingK, ref currentLerpTimeK, hypnosisMaterialK, ref hasCompletedCycleK);
        }
    }

    private void PerformLerp(ref bool isLerping, ref float currentLerpTime, Material material, ref bool hasCompletedCycle)
    {
        currentLerpTime += Time.deltaTime;
        float t = currentLerpTime / lerpDuration;
        float blinkValue = Mathf.Lerp(startValue, endValue, t);
        material.SetFloat("_Blink", blinkValue);

        if (blinkValue <= 0.2f && !hasCompletedCycle)
        {
            StartCoroutine(ToggleLights());
            hasCompletedCycle = true;
        }

        // Si hemos alcanzado el tiempo de duración, reiniciamos el ciclo
        if (currentLerpTime >= lerpDuration)
        {
            currentLerpTime = 0.0f;

            // Intercambiamos los valores de startValue y endValue para invertir el lerp
            if (startValue == 1.0f && endValue == 0.2f)
            {
                startValue = 0.2f;
                endValue = 1.0f;
            }
            else
            {
                startValue = 1.0f;
                endValue = 0.2f;
                isLerping = false;
                hasCompletedCycle = false;
            }
        }
    }

    private IEnumerator ToggleLights()
    {
        if (skyboxIsOn)
        {
            // Apaga Skybox, apaga PointLights, enciende DemonLight
            yield return StartCoroutine(TransitionSkybox(false));
            DeactivateLights();
            ActivateDemonLight();
        }
        else
        {
            // Enciende Skybox, enciende PointLights, apaga DemonLight
            yield return StartCoroutine(TransitionSkybox(true));
            ActivateLights();
            DeactivateDemonLight();
        }

        // Cambia el estado del Skybox
        skyboxIsOn = !skyboxIsOn;
    }

    private IEnumerator TransitionSkybox(bool toOn)
    {
        if (toOn)
        {
            EncenderSkybox();
        }
        else
        {
            ApagarSkybox();
        }

        // Asegúrate de que los cambios de iluminación se apliquen
        yield return new WaitForEndOfFrame(); // Espera un frame para permitir que se rendericen los cambios
        DynamicGI.UpdateEnvironment(); // Actualiza la iluminación global
    }

    private void DeactivateLights()
    {
        foreach (var light in pointLights)
        {
            light.SetActive(false);
        }
    }

    private void DeactivateDemonLight()
    {
        if (spotLight == null)
        {
            spotLight = GameObject.FindGameObjectWithTag("DemonLight");
        }

        spotLight.SetActive(false);
    }

    private void ActivateLights()
    {
        foreach (var light in pointLights)
        {
            light.SetActive(true);
        }
    }

    private void ActivateDemonLight()
    {
        if (spotLight == null)
        {
            spotLight = GameObject.FindGameObjectWithTag("DemonLight");
        }

        spotLight.SetActive(true);
    }

    public void ApagarSkybox()
    {
        RenderSettings.skybox = null;
        DynamicGI.UpdateEnvironment();
    }

    public void EncenderSkybox()
    {
        if (originalSkybox != null)
        {
            RenderSettings.skybox = originalSkybox;
        }
        DynamicGI.UpdateEnvironment();
    }
}