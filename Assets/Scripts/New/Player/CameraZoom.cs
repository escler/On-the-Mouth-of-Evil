using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [Header("Zoom Settings")]
    [SerializeField] private float zoomFOV = 30f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private KeyCode zoomKey = KeyCode.Mouse1;

    [Header("Opcional")]
    [SerializeField] private bool toggleMode = false;
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
        
        if (!PlayerHandler.Instance.playerCam.enabled || PlayerHandler.Instance.movement.ritualCinematic)
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

        // Si está forzado, se mantiene zoom aunque no esté presionado
        float targetFOV = _isZooming ? zoomFOV : normalFOV;
        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }

}

