using System;
using UnityEngine;

public class Shackles : MonoBehaviour
{
    public Vector2 scaleRange = new Vector2(1f, 2f); // Valores mínimos y máximos de la escala en XZ
    public float lerpDuration = 1f; // Duración del interpolado
    public Material rosaryMaterial; // Material del shader rosary
    public Vector2 edgeControlRange = new Vector2(0f, 1f); // Rango de valores para EdgeControl

    private float lerpTime = 0f;
    private bool isScaling = false;
    public bool toggleState = false; // Estado para alternar entre los valores
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

        // Inicialización de valores
        initialScale = transform.localScale;
        targetScale = new Vector3(scaleRange.y, transform.localScale.y, scaleRange.y);
        initialEdgeControl = edgeControlRange.x;
        targetEdgeControl = edgeControlRange.y;
    }

    private void Awake()
    {
        ChangeState();
    }

    public void ChangeState()
    {
        isScaling = true;
        lerpTime = 0f;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }

        if (isScaling)
        {
            lerpTime += Time.deltaTime / lerpDuration;

            // Interpolación de escala
            transform.localScale = Vector3.Lerp(initialScale, targetScale, lerpTime);

            // Interpolación de EdgeControl
            if (rosaryMaterial != null)
            {
                float edgeControlValue = Mathf.Lerp(initialEdgeControl, targetEdgeControl, lerpTime);
                rosaryMaterial.SetFloat("_EdgeControl", edgeControlValue);
            }

            if (lerpTime >= 1f)
            {
                isScaling = false; // Finalizar la interpolación
            }
        }
    }
}
