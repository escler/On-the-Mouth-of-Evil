using UnityEngine;

public class SandFallingAnimation : MonoBehaviour
{
    public Material sandMaterial; 
    public float animationSpeed = 1.0f; 
    private float time;

    void Start()
    {
        if (sandMaterial == null)
        {
            Debug.LogError("Material no asignado al script.");
        }
    }

    void Update()
    {
        // Aumenta el tiempo acumulado en funci�n del tiempo real y la velocidad de la animaci�n
        time += Time.deltaTime * animationSpeed;

        // Actualiza la propiedad _TimeScale en el material
        sandMaterial.SetFloat("_TimeScale", time);

        // Opcional: tambi�n puedes animar otras propiedades, como _Cutoff
        float cutoffValue = Mathf.PingPong(Time.time, 1.0f);
        sandMaterial.SetFloat("_Cutoff", cutoffValue);
    }
}
