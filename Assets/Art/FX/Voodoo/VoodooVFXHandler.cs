using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class VoodooVFXHandler : MonoBehaviour
{
    // Asigna el VisualEffect en el Inspector de Unity
    public VisualEffect vfx;

    // Nombres de las propiedades que quieres modificar en el VFX
    public string sizePropertyName = "Size";
    public string sizeSealPropertyName = "SizeSeal";
    public string topHeightPropertyName = "TopHeight";

    // Objetos a modificar en la escena
    public Transform object1;
    public Transform object2;
    public Transform object3;

    // Valores iniciales y objetivos para cada propiedad del VFX
    public float initialSize;
    public float targetSize;

    public float initialSizeSeal;
    public float targetSizeSeal;

    public float initialTopHeight;
    public float targetTopHeight;

    // Objetivos de escala para los objetos en la escena
    private Vector3 initialScale = new Vector3(0.3f, 0, 0.3f);
    private Vector3 targetScale = new Vector3(0.5f, 0.4f, 0.5f);

    // Duraci�n de la interpolaci�n
    public float lerpDuration;

    private bool isLerpingSizeSeal = false;
    private bool isLerpingSizeAndHeight = false;
    private bool isOpening = true; // Estado que indica si est� "abriendo" o "cerrando"
    private float lerpTime = 0f;

    private bool closed;

    void Start()
    {
        // Establece los valores iniciales en el VFX y la escala inicial de los objetos
        ResetToInitialValues();
    }

    public void ClosePrison()
    {
        isLerpingSizeSeal = false;
        isLerpingSizeAndHeight = true;
        isOpening = false; // Configura el efecto para cerrarse
        lerpTime = 0f;
    }

    public void OpenPrison()
    {
        ResetToInitialValues();
        closed = false;
        isLerpingSizeSeal = true; // Aseguramos que sizeSeal empiece inmediatamente
        isLerpingSizeAndHeight = false;
        isOpening = true; // Configura el efecto para abrirse
        lerpTime = 0f;
    }
    
    void Update()
    {
        if (closed) return;
        // Interpolaci�n de sizeSeal durante la apertura
        if (isLerpingSizeSeal)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / lerpDuration;

            // Interpolaci�n lineal para sizeSeal
            float lerpedSizeSeal = Mathf.Lerp(initialSizeSeal, targetSizeSeal, t);
            SetFloatProperty(sizeSealPropertyName, lerpedSizeSeal);

            // Finaliza la interpolaci�n de sizeSeal y activa la de size y topHeight
            if (lerpTime > lerpDuration)
            {
                isLerpingSizeSeal = false;
                isLerpingSizeAndHeight = true;
                lerpTime = 0f; // Reinicia el tiempo para la segunda interpolaci�n
            }

            if (!isOpening)
            {
                lerpTime += Time.deltaTime;
                t = lerpTime / lerpDuration;

                // Interpolaci�n lineal para sizeSeal
                lerpedSizeSeal = Mathf.Lerp(targetSizeSeal, initialSizeSeal, t);
                SetFloatProperty(sizeSealPropertyName, lerpedSizeSeal);

                // Finaliza la interpolaci�n de sizeSeal
                if (lerpTime > lerpDuration)
                {
                    isLerpingSizeSeal = false;
                    closed = true;
                }
                return;
            }
        }

        // Interpolaci�n de size y topHeight, ya sea para abrir o cerrar
        if (isLerpingSizeAndHeight)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / lerpDuration;

            // Interpolaci�n lineal para size y topHeight
            float lerpedSize = Mathf.Lerp(
                isOpening ? initialSize : targetSize,
                isOpening ? targetSize : initialSize,
                t
            );
            float lerpedTopHeight = Mathf.Lerp(
                isOpening ? initialTopHeight : targetTopHeight,
                isOpening ? targetTopHeight : initialTopHeight,
                t
            );

            SetFloatProperty(sizePropertyName, lerpedSize);
            SetFloatProperty(topHeightPropertyName, lerpedTopHeight);

            // Interpolaci�n para la escala de los objetos en Y
            Vector3 lerpedScale = Vector3.Lerp(
                isOpening ? initialScale : targetScale,
                isOpening ? targetScale : initialScale,
                t
            );
            object1.localScale = lerpedScale;
            object2.localScale = lerpedScale;
            object3.localScale = lerpedScale;

            // Finaliza la interpolaci�n de size y topHeight
            if (lerpTime > lerpDuration)
            {
                isLerpingSizeAndHeight = false;

                if (!isOpening)
                {
                    // Despu�s de terminar con size y topHeight, activa la interpolaci�n de sizeSeal
                    isLerpingSizeSeal = true;
                    lerpTime = 0f; // Reinicia el tiempo para la interpolaci�n de sizeSeal
                }
            }
        }

        // Interpolaci�n de sizeSeal durante el cierre
        if (isLerpingSizeSeal && !isOpening)
        {

        }
    }
    
    

    // M�todo para modificar una propiedad de tipo float en el VFX
    private void SetFloatProperty(string propertyName, float value)
    {
        if (vfx.HasFloat(propertyName))
        {
            vfx.SetFloat(propertyName, value);
        }
        else
        {
            Debug.LogWarning($"La propiedad {propertyName} no se encontr� en el VFX.");
        }
    }

    // M�todo para restablecer los valores iniciales del VFX y de la escala de los objetos
    private void ResetToInitialValues()
    {
        SetFloatProperty(sizePropertyName, initialSize);
        SetFloatProperty(sizeSealPropertyName, initialSizeSeal);
        SetFloatProperty(topHeightPropertyName, initialTopHeight);

        // Restablece la escala de los objetos en Y a 0
        object1.localScale = initialScale;
        object2.localScale = initialScale;
        object3.localScale = initialScale;
    }
}
