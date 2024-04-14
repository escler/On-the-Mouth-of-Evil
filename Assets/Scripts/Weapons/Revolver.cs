using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            GameObject.Find("CM_Camera").GetComponent<CameraMovement>().SetCameraMode(CameraMovement.CameraMode.Aim);
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            GameObject.Find("CM_Camera").GetComponent<CameraMovement>().SetCameraMode(CameraMovement.CameraMode.Normal);
        }
        
    }
}
