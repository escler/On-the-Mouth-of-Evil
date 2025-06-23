using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sangre : MonoBehaviour
{
    public Vector3 escalaOriginal = new Vector3(2.91087556f, 1.73157787f, 2.90893531f);
    public Vector3 escalaObjetivo = new Vector3(1f, 4f, 1f);
    public Vector3 escalaFinal = new Vector3(3f, 1f, 3f);

    public Vector3 posicionOriginal;
    public Vector3 posicionObjetivo;
    public Vector3 posicionFinal;

    public float duracionTransicion = 2f;

    private bool animando = false;
    private float tiempoTranscurrido = 0f;

    private int estadoActual = 0;

    // Shader properties
    private Material materialSangre;

    // Twirl (progresivo) y Rotate (inmediato)
    public float twirlPowerObjetivo = 1f;
    public float rotatePowerObjetivo = 1f;

    private float twirlPowerOriginal = 0f;
    private float rotatePowerOriginal = 0f;

    void Start()
    {
        posicionOriginal = transform.position;

        // Obtiene el material (asegurate que el objeto tenga un Renderer)
        materialSangre = GetComponent<Renderer>().material;

        // Inicializa propiedades del shader
        materialSangre.SetFloat("_twirl_power", twirlPowerOriginal);
        materialSangre.SetFloat("_rotate_power", rotatePowerOriginal);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && estadoActual == 0)
        {
            animando = true;
            tiempoTranscurrido = 0f;
            estadoActual = 1;

            // Cambiar rotatePower de forma inmediata al comenzar la fase 1
            materialSangre.SetFloat("_rotate_power", rotatePowerObjetivo);
        }

        if (animando)
        {
            tiempoTranscurrido += Time.deltaTime;
            float t = tiempoTranscurrido / duracionTransicion;

            if (estadoActual == 1)
            {
                transform.localScale = Vector3.Lerp(escalaOriginal, escalaObjetivo, t);
                transform.position = Vector3.Lerp(posicionOriginal, posicionObjetivo, t);

                if (t >= 1f)
                {
                    transform.localScale = escalaObjetivo;
                    transform.position = posicionObjetivo;

                    tiempoTranscurrido = 0f;
                    estadoActual = 2;
                }
            }
            else if (estadoActual == 2)
            {
                transform.localScale = Vector3.Lerp(escalaObjetivo, escalaFinal, t);
                transform.position = Vector3.Lerp(posicionObjetivo, posicionFinal, t);

                // Cambiar twirlPower de forma progresiva
                float twirlActual = Mathf.Lerp(twirlPowerOriginal, twirlPowerObjetivo, t);
                materialSangre.SetFloat("_twirl_power", twirlActual);

                if (t >= 1f)
                {
                    transform.localScale = escalaFinal;
                    transform.position = posicionFinal;

                    // Asegurarse que el valor final se aplique exacto
                    materialSangre.SetFloat("_twirl_power", twirlPowerObjetivo);

                    animando = false;
                    estadoActual = 3; // Animación completa
                }
            }
        }
    }
}
