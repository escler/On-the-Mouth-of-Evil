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

    public void LerpCamera(float m0_Height, float m0_Radius, float m1_Height, float m1_Radius, float m2_Height,
        float m2_Radius, float m0_ScreenX, float m1_ScreenX, float m1_ScreenY, float mw_ScreenX, float t)
    {
        _cfl.m_Orbits[0].m_Height = Mathf.SmoothStep(_cfl.m_Orbits[0].m_Height,m0_Height,t);
        _cfl.m_Orbits[0].m_Radius = Mathf.SmoothStep(_cfl.m_Orbits[0].m_Radius,m0_Radius,t);

        _cfl.m_Orbits[1].m_Height = Mathf.SmoothStep(_cfl.m_Orbits[1].m_Height,m1_Height,t);
        _cfl.m_Orbits[1].m_Radius = Mathf.SmoothStep(_cfl.m_Orbits[1].m_Radius,m1_Radius,t);

        _cfl.m_Orbits[2].m_Height = Mathf.SmoothStep(_cfl.m_Orbits[2].m_Height,m2_Height,t);
        _cfl.m_Orbits[2].m_Radius = Mathf.SmoothStep(_cfl.m_Orbits[2].m_Radius,m2_Radius,t);

        _cfl.GetRig(0).GetCinemachineComponent<CinemachineComposer>().m_ScreenX = 
            Mathf.SmoothStep(_cfl.GetRig(0).GetCinemachineComponent<CinemachineComposer>().m_ScreenX,m0_ScreenX,t);
        _cfl.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_ScreenX = 
            Mathf.SmoothStep(_cfl.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_ScreenX,m1_ScreenX,t);
        _cfl.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_ScreenY = 
            Mathf.SmoothStep(_cfl.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_ScreenY,m1_ScreenY,t);
        _cfl.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_ScreenX =
            Mathf.SmoothStep(_cfl.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_ScreenX,mw_ScreenX,t);
    }
    public void SetCameraMode(CameraMode mode)
    {
        switch (mode)
        {
            case CameraMode.Normal:
                LerpCamera(2,1,
                    .5f,1.5f,
                    -1,1,
                    .3f,.3f,
                    .8f,.3f,
                    .2f);
                break;
            
            case CameraMode.Aim:
                LerpCamera(2.5f,1.2f,
                    .5f,1,
                    -1.2f,1,
                    0.2f,0.2f,
                    1,0.2f,
                    .2f);
                break;
        }
    }
}
