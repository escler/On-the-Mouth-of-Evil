using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBurneable 
{
    Vector3 Position { get; set; }
    public void OnBurn();
}
