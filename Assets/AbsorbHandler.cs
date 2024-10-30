using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AbsorbHandler : MonoBehaviour
{
    [SerializeField] VisualEffect vfxAbsob;
    [SerializeField] VisualEffect vfxMagnetic;
    private bool _isPlaying;
    [SerializeField] float minPower, maxPower, minPos, maxPos;
    [SerializeField] float lerpSpeed;
    [SerializeField] Material _cabritaMat;
    [SerializeField] float initialPower;
    [SerializeField] float initialPosition;
    [SerializeField] Animator enemyAnimator;
    private float _lerpTime; // Tiempo acumulado para la interpolación
    public float duration; // Duración de la interpolación en segundos

    void Awake()
    {
        _isPlaying = false;
        vfxAbsob.Stop();
        vfxMagnetic.Stop();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (_cabritaMat.HasProperty("_Power") && _cabritaMat.HasProperty("_Position"))
            {
                _cabritaMat.SetFloat("_Power", initialPower);
                _cabritaMat.SetFloat("_Position", initialPosition);
                _lerpTime = 0f; // Reiniciar el tiempo de lerp
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_isPlaying && _cabritaMat.HasProperty("_Power") && _cabritaMat.HasProperty("_Position"))
            {
                vfxAbsob.Play();
                vfxMagnetic.Play();
                _isPlaying = true;
                _lerpTime = 0f; // Reiniciar el tiempo de lerp al iniciar

                // Activar el parámetro booleano del Animator
                enemyAnimator.SetBool("Absorb", true);
            }
            else if (_isPlaying)
            {
                vfxAbsob.Stop();
                vfxMagnetic.Stop();
                _isPlaying = false;
                _lerpTime = 0f; // Reiniciar el tiempo de lerp al detener

                // Desactivar el parámetro booleano del Animator
                enemyAnimator.SetBool("Absorb", false);
            }
            else
            {
                Debug.LogError("El material no tiene la propiedad '_Power' o '_Position'");
            }
        }

        if (_isPlaying)
        {
            _lerpTime += Time.deltaTime;

            // Normalizar el tiempo para que la interpolación tome exactamente 3 segundos
            float t = Mathf.Clamp01(_lerpTime / duration);

            // Interpolación entre minPower y maxPower en 3 segundos
            float powerValue = Mathf.Lerp(minPower, maxPower, t);
            _cabritaMat.SetFloat("_Power", powerValue);

            // Interpolación entre minPos y maxPos en 3 segundos
            float posValue = Mathf.Lerp(minPos, maxPos, t);
            _cabritaMat.SetFloat("_Position", posValue);

            // Detener la interpolación cuando se completa el ciclo
            if (t >= 1f)
            {
                _isPlaying = false;
                vfxAbsob.Stop();
                vfxMagnetic.Stop();

                // Desactivar el parámetro booleano del Animator
                enemyAnimator.SetBool("Absorb", false);
            }
        }
    }
}
