using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [Header("Zoom Settings")]
    [SerializeField] private float zoomFOV = 30f;          // FOV al hacer zoom
    [SerializeField] private float zoomSpeed = 5f;         // Velocidad de transición
    [SerializeField] private float normalFOV = 60f;        // FOV normal de la cámara
    [SerializeField] private KeyCode zoomKey = KeyCode.Mouse1; // Botón derecho del mouse

    [Header("Opcional")]
    [SerializeField] private bool toggleMode = false;      // True = click para activar/desactivar
    [SerializeField] private bool disableDuringCinematic = true;

    private Camera _cam;
    private bool _isZooming = false;

    private void Start()
    {
        _cam = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (_cam == null) return;

        if (disableDuringCinematic && PlayerHandler.Instance.movement.ritualCinematic)
        {
            _isZooming = false;
        }
        else
        {
            if (toggleMode)
            {
                if (Input.GetKeyDown(zoomKey))
                    _isZooming = !_isZooming;
            }
            else
            {
                _isZooming = Input.GetKey(zoomKey);
            }
        }

        float targetFOV = _isZooming ? zoomFOV : normalFOV;
        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }
}
