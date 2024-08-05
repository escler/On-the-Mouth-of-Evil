using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonDirection : MonoBehaviour
{
    public Vector4 _MoonPos;
    // Start is called before the first frame update
    void Start()
    {
        Shader.SetGlobalVector("_MoonDirection", _MoonPos);
    }

}
