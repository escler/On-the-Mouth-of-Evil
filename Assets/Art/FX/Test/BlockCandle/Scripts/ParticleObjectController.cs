using UnityEngine;

public class ParticleObjectController : MonoBehaviour
{
    public ParticleSystem particleSystem;  // El sistema de partículas
    public GameObject[] objectsToControl;  // Array de los objetos a prender/apagar

    void Update()
    {
        // Verificamos si la partícula está reproduciéndose
        if (particleSystem.isPlaying)
        {
            // Encendemos los objetos
            SetObjectsActive(true);
        }
        // Si el sistema de partículas se ha detenido completamente
        else if (particleSystem.isStopped)
        {
            // Apagamos los objetos
            SetObjectsActive(false);
        }
    }

    // Método para prender o apagar los objetos
    void SetObjectsActive(bool isActive)
    {
        foreach (GameObject obj in objectsToControl)
        {
            obj.SetActive(isActive);  // Prendemos o apagamos el objeto
        }
    }
}