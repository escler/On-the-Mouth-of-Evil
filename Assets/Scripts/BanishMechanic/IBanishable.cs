using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBanishable
{ 
    public bool canBanish { get; set; }
    public bool onBanishing { get; set; }
    public void StartBanish();
    public void FinishBanish();
}
