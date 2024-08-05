using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerView : MonoBehaviour
{
    [Header("Dash")]
    public float activeTime = 2f;
    public float meshRefreshRate = .1f;
    public float meshDestroyDelay = 3f;
    private bool _isTrailActivate;
    public Transform positionToSpawn;
    private SkinnedMeshRenderer[] skinnedMeshRendereres;
    public Material mat;
    public string shaderVarRef;
    public float shaderVarRate = 0.1f;
    public float shaderVarRefreshRate = 0.05f;
    
    private Vignette vignette;
    [Header("DamagePostProcess")]
    public Material damageMaterial;
    public VolumeProfile postProcess;
    public float hurthRate;
    public float hurthRefreshRate;
    private PlayerLifeHandler _life;
    private bool _isActive;

    private void Awake()
    {
        if (damageMaterial == null) return;
        damageMaterial.SetFloat("_Vignette_Darkness", 0);
        _life = GetComponent<PlayerLifeHandler>();
        _life.OnTakeDamage += DamageReceive;

    }

    private void OnDestroy()
    {
        _life.OnTakeDamage -= DamageReceive;
    }

    public void ActivateTrail()
    {
        _isTrailActivate = true;
        StartCoroutine(ActivateTrail(activeTime));
    }

    public void DeactivateTrail()
    {
        _isTrailActivate = false;

    }

    void DamageReceive()
    {
        if (_isActive) return;
        StartCoroutine(DamagePostProcess());
    }

    IEnumerator DamagePostProcess()
    {
        _isActive = true;

        var smooth = 1f;

        damageMaterial.SetFloat("_Vignette_Darkness", smooth);

        while (smooth > 0)
        {
            smooth -= hurthRate;
            damageMaterial.SetFloat("_Vignette_Darkness", smooth);
            yield return new WaitForSeconds(hurthRefreshRate);
        }
        _isActive = false;
    }

    IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive > 0 && _isTrailActivate)
        {
            timeActive -= meshRefreshRate;

            if (skinnedMeshRendereres == null) skinnedMeshRendereres = GetComponentsInChildren<SkinnedMeshRenderer>();

            for (int i = 0; i < skinnedMeshRendereres.Length; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(positionToSpawn.position,positionToSpawn.rotation);

                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRendereres[i].BakeMesh(mesh);

                mf.mesh = mesh;
                mr.material = mat;

                StartCoroutine(AnimateMaterialFloat(mr.material, 0,shaderVarRate, shaderVarRefreshRate));
                
                Destroy(gObj, meshDestroyDelay);
            }
            yield return new WaitForSeconds(meshRefreshRate);
        }
    }
    

    IEnumerator AnimateMaterialFloat(Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVarRef);

        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef,valueToAnimate);

            yield return new WaitForSeconds(refreshRate);
        }
    }
}
