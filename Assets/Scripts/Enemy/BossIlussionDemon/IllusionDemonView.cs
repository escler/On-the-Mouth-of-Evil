using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionDemonView : MonoBehaviour
{
    public TrailRenderer _trailSword;

    public void StartTrail()
    {
        _trailSword.emitting = true;
    }

    public void StopTrail()
    {
        _trailSword.emitting = false;
    }
}
