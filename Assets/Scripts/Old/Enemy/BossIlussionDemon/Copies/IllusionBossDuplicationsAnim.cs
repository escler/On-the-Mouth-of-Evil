using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionBossDuplicationsAnim : MonoBehaviour
{
    private Animator _anim;

    private IllusionDuplication _i;
    public Animator Animator => _anim;

    public bool moving, attack, run;
    public float yAxis;
    void Awake()
    {
        _anim = GetComponent<Animator>();
        _i = GetComponentInParent<IllusionDuplication>();
    }

    void Update()
    {
        _anim.SetBool("Moving", moving);
        _anim.SetBool("Attack", attack);
        _anim.SetBool("Run", run);
        _anim.SetFloat("yAxis", yAxis);
    }
    
    public void EnableHitBox()
    {
        _i.spawnHitbox.SetActive(true);
    }
    
    public void DisableHitBox()
    {
        _i.spawnHitbox.SetActive(false);
    }
    
    public void AdjustPosition()
    {
        var modelPos = _i._model.transform.position;
        _i.transform.position = new Vector3(modelPos.x, _i.transform.position.y, modelPos.z);
        _i._model.transform.localPosition = Vector3.zero;
    }

}
