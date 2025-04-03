using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    private bool _cantRotate;
    private int actualRotation;
    public Vector3[] rotations;
    public float speedRotation;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) StartCoroutine(RotateSocket()); 
    }
    
    IEnumerator RotateSocket()
    {
        _cantRotate = true;
        actualRotation++;
        if (actualRotation >= rotations.Length) actualRotation = 0;
        while (Mathf.Abs(transform.localEulerAngles.z - rotations[actualRotation].z) > 1)
        {
            print(Mathf.Abs(transform.localEulerAngles.z - rotations[actualRotation].z));
            transform.Rotate(0,0,speedRotation,Space.Self);
            yield return new WaitForSeconds(0.01f);
        }

        transform.localEulerAngles = rotations[actualRotation];
        
        _cantRotate = false;
    }
}
