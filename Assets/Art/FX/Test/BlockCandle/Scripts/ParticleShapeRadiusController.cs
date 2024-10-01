using UnityEngine;

public class ParticleShapeRadiusController : MonoBehaviour
{
    public ParticleSystem initialParticleSystem;  // El primer sistema de partículas
    public ParticleSystem particleSystemWithGrowth;  // El segundo sistema de partículas (con crecimiento radial)
    public float startRadius = 0.5f;  // Radio inicial
    public float maxRadius = 5f;  // Radio máximo
    public float growthSpeed = 1f;  // Velocidad del crecimiento

    private ParticleSystem.ShapeModule shapeModule;
    private bool isGrowing = false;  // Controla si el radio está creciendo
    private bool hasStartedGrowth = false;  // Controla si ya se inició el crecimiento

    void Start()
    {
        // Accedemos al módulo Shape del segundo sistema de partículas
        shapeModule = particleSystemWithGrowth.shape;

        // Establecemos el radio inicial
        shapeModule.radius = startRadius;
    }

    void Update()
    {
        // Si el primer sistema de partículas ha terminado y aún no hemos comenzado el crecimiento
        if (initialParticleSystem.isStopped && !hasStartedGrowth)
        {
            // Inicia el segundo sistema de partículas con crecimiento radial
            particleSystemWithGrowth.Play();
            isGrowing = true;
            hasStartedGrowth = true;  // Marcamos que ya hemos comenzado el crecimiento
        }

        // Si el radio está creciendo, lo aumentamos progresivamente hasta el valor máximo
        if (isGrowing && shapeModule.radius < maxRadius)
        {
            shapeModule.radius += growthSpeed * Time.deltaTime;

            // Limitar el radio al valor máximo
            if (shapeModule.radius > maxRadius)
            {
                shapeModule.radius = maxRadius;
                isGrowing = false;  // Detenemos el crecimiento cuando alcanzamos el máximo
            }
        }
    }
}