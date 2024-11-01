using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiblePaper : MonoBehaviour, IBurneable, IInteractable
{
    public float timeToBurn;
    public GameObject firePS;
    private MaterialPropertyBlock _burning;
    private float _valueBurn;
    public MeshRenderer[] meshesh;
    public bool paperOnRitual;
    
    private void Awake()
    {
        _burning = new MaterialPropertyBlock();
        foreach (var mesh in meshesh)
        {
            mesh.SetPropertyBlock(_burning);
        }
        _burning.SetFloat("_Edge2", 1);
        _valueBurn = _burning.GetFloat("_Edge2");
    }
    
    public void OnBurn()
    {
        GetComponent<BoxCollider>().enabled = false;
        if(Enemy.Instance != null && !paperOnRitual) Enemy.Instance.SetGoalPos(transform.position);
        if (HouseEnemy.Instance != null && paperOnRitual && RitualManager.Instance.candlesPlaced >= 3)
        {
            HouseEnemy.Instance.RitualReady(RitualManager.Instance.ritualNode);
        }
        StartCoroutine(BibleBurning());        
        StartCoroutine(BurnPaper());
        foreach (var mesh in meshesh)
        {
            mesh.SetPropertyBlock(_burning);
        }
    }

    IEnumerator BurnPaper()
    {
        while (timeToBurn > 0)
        {
            _valueBurn -= 0.2f / 10;
            foreach (var mesh in meshesh)
            {
                _burning.SetFloat("_Edge2", _valueBurn);
                mesh.SetPropertyBlock(_burning);
            }

            yield return new WaitForSeconds(0.1f);
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

        if(Enemy.Instance != null && !paperOnRitual) Enemy.Instance.bibleBurning = false;
        Destroy(gameObject);
    }

    public void OnInteractItem()
    {
    }

    public void OnInteract(bool hit, RaycastHit i)
    {
    }

    public void OnInteractWithObject()
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
