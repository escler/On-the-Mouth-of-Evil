using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayVFX : MonoBehaviour
{
    [SerializeField] VisualEffect vfx;
   
    void Start()
    {
        //playFX();
    }

    void Update()
    {
        
    }
    public void playFX()
    {        
        vfx.Play();
    }

}
