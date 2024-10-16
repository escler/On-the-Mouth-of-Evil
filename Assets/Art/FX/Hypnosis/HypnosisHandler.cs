using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class HypnosisEffectControllerHDRP : MonoBehaviour
{
    public static HypnosisEffectControllerHDRP Instance { get; private set; }
    public Material hypnosisMaterial;

    public float lerpDuration = 1.0f;
    private float currentLerpTime = 0.0f;
    private bool isLerping = false;

    private float startValue = 1.0f;
    private float endValue = 0.2f;
    private bool hasCompletedCycle = false;

    private GameObject[] pointLights;
    private GameObject spotLight;


    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
        hypnosisMaterial.SetFloat("_Blink", startValue);
        isLerping = false;
        // Descomenta la siguiente línea si quieres activar las luces en el inicio
         pointLights = GameObject.FindGameObjectsWithTag("PointLight");
         spotLight = GameObject.FindGameObjectWithTag("DemonLight");
        DeactivateDemonLight();


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (hasCompletedCycle)
            {
                ResetCycle();
            }

            if (!isLerping)
            {
                isLerping = true;
                currentLerpTime = 0.0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            ActivateLights();
            DeactivateDemonLight();

        }

        if (isLerping)
        {
            PerformLerp();
        }
    }

    private void PerformLerp()
    {
        currentLerpTime += Time.deltaTime;

        float t = currentLerpTime / lerpDuration;
        float blinkValue = Mathf.Lerp(startValue, endValue, t);
        hypnosisMaterial.SetFloat("_Blink", blinkValue);

        if (blinkValue <= 0.5f && !hasCompletedCycle)
        {
            DeactivateLights();
            ActivateDemonLight();
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
                // Restablecer el ciclo
                startValue = 1.0f;
                endValue = 0.2f;
                isLerping = false;
                hasCompletedCycle = true;
            }
        }
    }

    private void DeactivateLights()
    {
        if (pointLights == null)
        {
            pointLights = GameObject.FindGameObjectsWithTag("PointLight");
        }

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
        if (pointLights == null)
        {
            pointLights = GameObject.FindGameObjectsWithTag("PointLight");
        }

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

    public void ResetCycle()
    {
        hasCompletedCycle = false;
        startValue = 1.0f;
        endValue = 0.2f;
    }
}

