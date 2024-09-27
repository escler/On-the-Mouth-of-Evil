using UnityEngine;

public class ParticleCircleRadius : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public ParticleSystem nextParticleSystem;  // El segundo sistema de partículas
    public float maxRadius = 5f;  // El radio máximo que quieres alcanzar
    public float growthSpeed = 1f; // Velocidad de crecimiento del radio

    private ParticleSystem.ShapeModule shapeModule;

    void Start()
    {
        // Accedemos al módulo Shape del sistema de partículas
        shapeModule = particleSystem.shape;
    }

    void Update()
    {
        // Verificamos si el sistema de partículas actual está reproduciéndose
        if (particleSystem.isPlaying)
        {
            // Aumentamos el radio con el tiempo hasta alcanzar el máximo
            if (shapeModule.radius < maxRadius)
            {
                shapeModule.radius += growthSpeed * Time.deltaTime;
            }
        }
        // Si el sistema de partículas ha terminado (se ha detenido completamente)
        else if (particleSystem.isStopped && !nextParticleSystem.isPlaying)
        {
            // Reproducimos el siguiente sistema de partículas
            nextParticleSystem.Play();
        }
    }
}