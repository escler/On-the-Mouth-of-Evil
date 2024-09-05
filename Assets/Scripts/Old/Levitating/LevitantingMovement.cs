using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitantingMovement : MonoBehaviour
{
    public float maxHeight;
    public float velocityLevitation;
    private Vector3 initialPos;
    bool boina;

  

    void Start()
    {
        initialPos = transform.position;
    }
    void Update()
    {
        
        
        transform.position = initialPos + new Vector3(0, 
            Mathf.Sin(Time.time * velocityLevitation) * maxHeight, 0);
       
        
    }

    

}
