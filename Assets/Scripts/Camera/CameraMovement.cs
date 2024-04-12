using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    private CinemachineFreeLook _cfl;
    public enum CameraMode
    {
        Normal,
        Aim
    }
    
    // Start is called before the first frame update

    private void Awake()
    {
        _cfl = GetComponent<CinemachineFreeLook>();
    }

    private void FixedUpdate()
    {
        SetCameraMode(CameraMode.Normal);

    }

    private void SetCameraMode(CameraMode mode)
    {
        switch (mode)
        {
            case CameraMode.Normal:
                _cfl.m_Orbits[0].m_Height = 4;
                _cfl.m_Orbits[0].m_Radius = 1.75f;

                _cfl.m_Orbits[1].m_Radius = 3.37f;

                _cfl.m_Orbits[2].m_Height = -1f;


                
                break;
            
            case CameraMode.Aim:
                _cfl.m_Orbits[0].m_Height = 2.0f;
                _cfl.m_Orbits[0].m_Radius = 1.0f;

                _cfl.m_Orbits[1].m_Radius = 1.3f;
                
                _cfl.m_Orbits[2].m_Height = 1.2f;

                _cfl.GetRig(0).GetCinemachineComponent<CinemachineComposer>().m_ScreenX = 0.2f;
                _cfl.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_ScreenX = 0.2f;
                _cfl.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_ScreenX = 0.2f;
                
                break;
        }
    }
}
