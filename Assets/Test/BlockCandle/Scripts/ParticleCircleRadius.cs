using UnityEngine;

public class ParticleCircleRadius : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public ParticleSystem nextParticleSystem;  // El segundo sistema de part�culas
    public float maxRadius = 5f;  // El radio m�ximo que quieres alcanzar
    public float growthSpeed = 1f; // Velocidad de crecimiento del radio

    private ParticleSystem.ShapeModule shapeModule;

    void Start()
    {
        // Accedemos al m�dulo Shape del sistema de part�culas
        shapeModule = particleSystem.shape;
    }

    void Update()
    {
        // Verificamos si el sistema de part�culas actual est� reproduci�ndose
        if (particleSystem.isPlaying)
        {
            // Aumentamos el radio con el tiempo hasta alcanzar el m�ximo
            if (shapeModule.radius < maxRadius)
            {
                shapeModule.radius += growthSpeed * Time.deltaTime;
            }
        }
        // Si el sistema de part�culas ha terminado (se ha detenido completamente)
        else if (particleSystem.isStopped && !nextParticleSystem.isPlaying)
        {
            // Reproducimos el siguiente sistema de part�culas
            nextParticleSystem.Play();
        }
    }
}