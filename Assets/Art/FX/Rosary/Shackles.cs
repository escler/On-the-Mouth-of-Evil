using UnityEngine;

public class Shackles : MonoBehaviour
{
    public Vector2 scaleRange = new Vector2(1f, 2f); // Valores m�nimos y m�ximos de la escala en XZ
    public float lerpDuration = 1f; // Duraci�n del interpolado
    public Material rosaryMaterial; // Material del shader rosary
    public Vector2 edgeControlRange = new Vector2(0f, 1f); // Rango de valores para EdgeControl

    private float lerpTime = 0f;
    private bool isScaling = false;
    private bool toggleState = false; // Estado para alternar entre los valores
    private Vector3 initialScale;
    private Vector3 targetScale;
    private float initialEdgeControl;
    private float targetEdgeControl;

    void Start()
    {
        if (rosaryMaterial == null)
        {
            Debug.LogError("Rosary material is not assigned!");
        }

        // Inicializaci�n de valores
        initialScale = transform.localScale;
        targetScale = new Vector3(scaleRange.y, transform.localScale.y, scaleRange.y);
        initialEdgeControl = edgeControlRange.x;
        targetEdgeControl = edgeControlRange.y;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isScaling = true;
            lerpTime = 0f;

            // Alternar el estado para intercambiar valores
            toggleState = !toggleState;

            initialScale = transform.localScale;
            initialEdgeControl = rosaryMaterial.GetFloat("_EdgeControl");

            if (toggleState)
            {
                targetScale = new Vector3(scaleRange.y, transform.localScale.y, scaleRange.y);
                targetEdgeControl = edgeControlRange.y;
            }
            else
            {
                targetScale = new Vector3(scaleRange.x, transform.localScale.y, scaleRange.x);
                targetEdgeControl = edgeControlRange.x;
            }
        }

        if (isScaling)
        {
            lerpTime += Time.deltaTime / lerpDuration;

            // Interpolaci�n de escala
            transform.localScale = Vector3.Lerp(initialScale, targetScale, lerpTime);

            // Interpolaci�n de EdgeControl
            if (rosaryMaterial != null)
            {
                float edgeControlValue = Mathf.Lerp(initialEdgeControl, targetEdgeControl, lerpTime);
                rosaryMaterial.SetFloat("_EdgeControl", edgeControlValue);
            }

            if (lerpTime >= 1f)
            {
                isScaling = false; // Finalizar la interpolaci�n
            }
        }
    }
}
