using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
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
    
    public void ActivateTrail()
    {
        _isTrailActivate = true;
        StartCoroutine(ActivateTrail(activeTime));
    }

    public void DeactivateTrail()
    {
        _isTrailActivate = false;

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
