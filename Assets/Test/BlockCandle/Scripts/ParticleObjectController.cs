using UnityEngine;

public class ParticleObjectController : MonoBehaviour
{
    public ParticleSystem particleSystem;  // El sistema de part�culas
    public GameObject[] objectsToControl;  // Array de los objetos a prender/apagar

    void Update()
    {
        // Verificamos si la part�cula est� reproduci�ndose
        if (particleSystem.isPlaying)
        {
            // Encendemos los objetos
            SetObjectsActive(true);
        }
        // Si el sistema de part�culas se ha detenido completamente
        else if (particleSystem.isStopped)
        {
            // Apagamos los objetos
            SetObjectsActive(false);
        }
    }

    // M�todo para prender o apagar los objetos
    void SetObjectsActive(bool isActive)
    {
        foreach (GameObject obj in objectsToControl)
        {
            obj.SetActive(isActive);  // Prendemos o apagamos el objeto
        }
    }
}