using UnityEngine;

public class ParticleShapeRadiusController : MonoBehaviour
{
    public ParticleSystem initialParticleSystem;  // El primer sistema de part�culas
    public ParticleSystem particleSystemWithGrowth;  // El segundo sistema de part�culas (con crecimiento radial)
    public float startRadius = 0.5f;  // Radio inicial
    public float maxRadius = 5f;  // Radio m�ximo
    public float growthSpeed = 1f;  // Velocidad del crecimiento

    private ParticleSystem.ShapeModule shapeModule;
    private bool isGrowing = false;  // Controla si el radio est� creciendo
    private bool hasStartedGrowth = false;  // Controla si ya se inici� el crecimiento

    void Start()
    {
        // Accedemos al m�dulo Shape del segundo sistema de part�culas
        shapeModule = particleSystemWithGrowth.shape;

        // Establecemos el radio inicial
        shapeModule.radius = startRadius;
    }

    void Update()
    {
        // Si el primer sistema de part�culas ha terminado y a�n no hemos comenzado el crecimiento
        if (initialParticleSystem.isStopped && !hasStartedGrowth)
        {
            // Inicia el segundo sistema de part�culas con crecimiento radial
            particleSystemWithGrowth.Play();
            isGrowing = true;
            hasStartedGrowth = true;  // Marcamos que ya hemos comenzado el crecimiento
        }

        // Si el radio est� creciendo, lo aumentamos progresivamente hasta el valor m�ximo
        if (isGrowing && shapeModule.radius < maxRadius)
        {
            shapeModule.radius += growthSpeed * Time.deltaTime;

            // Limitar el radio al valor m�ximo
            if (shapeModule.radius > maxRadius)
            {
                shapeModule.radius = maxRadius;
                isGrowing = false;  // Detenemos el crecimiento cuando alcanzamos el m�ximo
            }
        }
    }
}