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
        SetCameraMode(CameraMode.Normal);
    }

    public void SetCameraMode(CameraMode mode)
    {
        switch (mode)
        {
            case CameraMode.Normal:
                _cfl.m_Orbits[0].m_Height = 2;
                _cfl.m_Orbits[0].m_Radius = 1.75f;

                _cfl.m_Orbits[1].m_Radius = 2.3f;

                _cfl.m_Orbits[2].m_Height = -1f;

                _cfl.GetRig(0).GetCinemachineComponent<CinemachineComposer>().m_ScreenX = 0.3f;
                _cfl.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_ScreenX = 0.3f;
                _cfl.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_ScreenX = 0.3f;

                
                break;
            
            case CameraMode.Aim:
                _cfl.m_Orbits[0].m_Height = 2.5f;
                _cfl.m_Orbits[0].m_Radius = 1.2f;

                _cfl.m_Orbits[1].m_Height = .2f;
                _cfl.m_Orbits[1].m_Radius = 1.5f;
                
                _cfl.m_Orbits[2].m_Height = -1.2f;

                _cfl.GetRig(0).GetCinemachineComponent<CinemachineComposer>().m_ScreenX = 0.2f;
                _cfl.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_ScreenX = 0.2f;
                _cfl.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_ScreenX = 0.2f;
                
                break;
        }
    }
}
