using UnityEngine;
using UnityEngine.VFX;

public class handler : MonoBehaviour
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
    private Vector3 initialScale = new Vector3(0.14f, 0, 0.14f);
    private Vector3 targetScale = new Vector3(0.14f, 0.1f, 0.14f);

    // Duración de la interpolación
    public float lerpDuration;

    private bool isLerpingSizeSeal = false;
    private bool isLerpingSizeAndHeight = false;
    private bool isOpening = true; // Estado que indica si está "abriendo" o "cerrando"
    private float lerpTime = 0f;

    void Start()
    {
        // Establece los valores iniciales en el VFX y la escala inicial de los objetos
        ResetToInitialValues();
    }

    void Update()
    {
        // Inicia el proceso de apertura al presionar la barra espaciadora
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetToInitialValues();
            isLerpingSizeSeal = true; // Aseguramos que sizeSeal empiece inmediatamente
            isLerpingSizeAndHeight = false;
            isOpening = true; // Configura el efecto para abrirse
            lerpTime = 0f;
        }

        // Inicia el proceso de cierre al presionar la tecla E, solo si el efecto está abierto
        if (Input.GetKeyDown(KeyCode.E) && !isLerpingSizeSeal && !isLerpingSizeAndHeight)
        {
            isLerpingSizeSeal = false;
            isLerpingSizeAndHeight = true;
            isOpening = false; // Configura el efecto para cerrarse
            lerpTime = 0f;
        }

        // Interpolación de sizeSeal durante la apertura
        if (isLerpingSizeSeal)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / lerpDuration;

            // Interpolación lineal para sizeSeal
            float lerpedSizeSeal = Mathf.Lerp(initialSizeSeal, targetSizeSeal, t);
            SetFloatProperty(sizeSealPropertyName, lerpedSizeSeal);

            // Finaliza la interpolación de sizeSeal y activa la de size y topHeight
            if (lerpTime >= lerpDuration)
            {
                isLerpingSizeSeal = false;
                isLerpingSizeAndHeight = true;
                lerpTime = 0f; // Reinicia el tiempo para la segunda interpolación
            }
        }

        // Interpolación de size y topHeight, ya sea para abrir o cerrar
        if (isLerpingSizeAndHeight)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / lerpDuration;

            // Interpolación lineal para size y topHeight
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

            // Interpolación para la escala de los objetos en Y
            Vector3 lerpedScale = Vector3.Lerp(
                isOpening ? initialScale : targetScale,
                isOpening ? targetScale : initialScale,
                t
            );
            object1.localScale = lerpedScale;
            object2.localScale = lerpedScale;
            object3.localScale = lerpedScale;

            // Finaliza la interpolación de size y topHeight
            if (lerpTime >= lerpDuration)
            {
                isLerpingSizeAndHeight = false;

                if (!isOpening)
                {
                    // Después de terminar con size y topHeight, activa la interpolación de sizeSeal
                    isLerpingSizeSeal = true;
                    lerpTime = 0f; // Reinicia el tiempo para la interpolación de sizeSeal
                }
            }
        }

        // Interpolación de sizeSeal durante el cierre
        if (isLerpingSizeSeal && !isOpening)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / lerpDuration;

            // Interpolación lineal para sizeSeal
            float lerpedSizeSeal = Mathf.Lerp(targetSizeSeal, initialSizeSeal, t);
            SetFloatProperty(sizeSealPropertyName, lerpedSizeSeal);

            // Finaliza la interpolación de sizeSeal
            if (lerpTime >= lerpDuration)
            {
                isLerpingSizeSeal = false;
            }
        }
    }

    // Método para modificar una propiedad de tipo float en el VFX
    private void SetFloatProperty(string propertyName, float value)
    {
        if (vfx.HasFloat(propertyName))
        {
            vfx.SetFloat(propertyName, value);
        }
        else
        {
            Debug.LogWarning($"La propiedad {propertyName} no se encontró en el VFX.");
        }
    }

    // Método para restablecer los valores iniciales del VFX y de la escala de los objetos
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
