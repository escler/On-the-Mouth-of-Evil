using UnityEngine;

public class LightController: MonoBehaviour
{
    public Light targetLight;  // La luz cuya intensidad quieres controlar.
    public float minIntensity;  // Intensidad mínima.
    public float maxIntensity;  // Intensidad máxima.
    public float speed;  // Velocidad de cambio.

    private bool increasing = true;

    void Update()
    {
        if (increasing)
        {
            targetLight.intensity += speed * Time.deltaTime;
            if (targetLight.intensity >= maxIntensity)
            {
                targetLight.intensity = maxIntensity;
                increasing = false;
            }
        }
        else
        {
            targetLight.intensity -= speed * Time.deltaTime;
            if (targetLight.intensity <= minIntensity)
            {
                targetLight.intensity = minIntensity;
                increasing = true;
            }
        }
    }
}