using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using UnityEngine.SceneManagement;
public class HypnosisEffectControllerHDRP : MonoBehaviour
{
    public static HypnosisEffectControllerHDRP Instance { get; private set; }
    public Material hypnosisMaterialK; // Material para blink con K

    public float lerpDuration = 1.0f;
    public float skyboxSpeedMultiplier = 1.0f; // Controla la velocidad desde el editor

    public float currentLerpTimeK = 0.0f; // Lerp para K
    public bool isLerpingK = false;

    private float startValue = 1.0f;
    private float endValue = 0.2f;
    private bool hasCompletedCycleK = false;

    private GameObject[] pointLights;
    private GameObject spotLight;

    public bool skyboxIsOn = true; // Indica el estado actual del Skybox
    private float skyboxIntensityStart = 1.0f;
    private float skyboxIntensityEnd = 0.0f;

    private bool skyboxNextState;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
        Instance = this;

        hypnosisMaterialK.SetFloat("_Blink", startValue);
        SceneManager.sceneLoaded += GetLights;

        GetLights(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        ActivateLights();
        DeactivateDemonLight();
        RenderSettings.ambientIntensity = .53f;
        //RenderSettings.skybox.SetFloat("_Exposure", skyboxIntensityStart); // Aseg�rate de usar "_Exposure" o el nombre correcto
        DynamicGI.UpdateEnvironment();

        // Originalmente la skybox deber�a estar visible.
    }

    private void Update()
    {
        if(isLerpingK) PerformLerp(ref isLerpingK, ref currentLerpTimeK, hypnosisMaterialK, ref hasCompletedCycleK, skyboxNextState);
    }

    public void StartLerpShader(string name)
    {
        print("Start " + name);
        skyboxNextState = false;
        hasCompletedCycleK = false;
        isLerpingK = true;
        currentLerpTimeK = 0.0f;
    }

    public void EndLerpShader(string name)
    {
        print("End " + name);
        skyboxNextState = true;
        isLerpingK = true;
        hasCompletedCycleK = false;
    }

    private void PerformLerp(ref bool isLerping, ref float currentLerpTime, Material material, ref bool hasCompletedCycle, bool skyBox)
    {
        currentLerpTime += Time.deltaTime;
        float t = currentLerpTime / lerpDuration;
        float blinkValue = Mathf.Lerp(startValue, endValue, t);
        material.SetFloat("_Blink", blinkValue);

        if (blinkValue <= 0.2f && !hasCompletedCycle)
        {
            StartCoroutine(ToggleLights(skyboxNextState));
            hasCompletedCycle = true;
        }

        // Ajuste del multiplicador de intensidad
        if (blinkValue <= 0.2f)
        {
            // Reducir multiplicador de intensidad a 0 al apagarse las luces
            material.SetFloat("_IntensityMultiplier", 0.0f);
        }
        else if (blinkValue >= 1.0f)
        {
            // Restablecer multiplicador de intensidad a 1 al encenderse las luces
            material.SetFloat("_IntensityMultiplier", 1.0f);
        }

        if (currentLerpTime >= lerpDuration)
        {
            currentLerpTime = 0.0f;

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

    private void GetLights(Scene scene, LoadSceneMode loadSceneMode)
    {
        pointLights = GameObject.FindGameObjectsWithTag("PointLight");
        spotLight = GameObject.FindGameObjectWithTag("DemonLight");
        ActivateLights();
        DeactivateDemonLight();
        RenderSettings.ambientIntensity = scene.name == "Menu" ? 0.0f : 0.53f;
        DynamicGI.UpdateEnvironment();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= GetLights;
    }
    
    private IEnumerator ToggleLights(bool skyBoxState)
    {
        if (!skyBoxState)
        {
            yield return StartCoroutine(ChangeSkyboxIntensity(.53f, 0.0f));
            DeactivateLights();
            ActivateDemonLight();
        }
        else
        {
            yield return StartCoroutine(ChangeSkyboxIntensity(0.0f, .53f));
            ActivateLights();
            DeactivateDemonLight();
        }
        skyboxIsOn = skyBoxState;
    }

    private IEnumerator ChangeSkyboxIntensity(float startIntensity, float endIntensity)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime * skyboxSpeedMultiplier; // Ajusta la velocidad con el multiplicador
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);
            float currentIntensity = Mathf.Lerp(startIntensity, endIntensity, t);
            
            RenderSettings.ambientIntensity = currentIntensity;
            DynamicGI.UpdateEnvironment();
            yield return null;
        }
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

        if (spotLight == null) return;
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

        if (spotLight == null) return;

        spotLight.SetActive(true);
    }
}
