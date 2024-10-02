using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiblePaper : MonoBehaviour, IBurneable, IInteractable
{
    public float timeToBurn;
    public GameObject firePS;
    private MaterialPropertyBlock _burning;
    public MeshRenderer[] meshesh;
    
    private void Awake()
    {
        _burning = new MaterialPropertyBlock();
        foreach (var mesh in meshesh)
        {
            mesh.SetPropertyBlock(_burning);
        }
        _burning.SetInt("_BurningON", 0);
    }
    
    public void OnBurn()
    {
        GetComponent<BoxCollider>().enabled = false;
        if(Enemy.Instance != null) Enemy.Instance.SetGoalPos(transform.position);
        StartCoroutine(BibleBurning());        
        _burning.SetInt("_BurningON", 1);
        foreach (var mesh in meshesh)
        {
            mesh.SetPropertyBlock(_burning);
        }
    }
    
    
    IEnumerator BibleBurning()
    {
        firePS.SetActive(true);
        while (timeToBurn > 0)
        {
            timeToBurn -= 1;

            yield return new WaitForSeconds(1f);
        }

        if(Enemy.Instance != null) Enemy.Instance.bibleBurning = false;
        Destroy(gameObject);
    }

    public void OnInteractItem()
    {
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public string ShowText()
    {
        return "";
    }

    public bool CanShowText()
    {
        return false;
    }
}
