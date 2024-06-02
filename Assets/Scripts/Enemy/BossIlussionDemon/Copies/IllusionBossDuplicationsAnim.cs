using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionBossDuplicationsAnim : MonoBehaviour
{
    private Animator _anim;

    public Animator Animator => _anim;

    public bool moving, attack, run;
    public float yAxis;
    void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        _anim.SetBool("Moving", moving);
        _anim.SetBool("Attack", attack);
        _anim.SetBool("Run", run);
        _anim.SetFloat("yAxis", yAxis);
    }
}
