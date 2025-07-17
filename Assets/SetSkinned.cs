using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SetSkinned : MonoBehaviour
{
    public VisualEffect vfx;
    public SkinnedMeshRenderer skinned;

    void Start()
    {
        vfx.SetSkinnedMeshRenderer("New SkinnedMeshRenderer", skinned);
    }
}
