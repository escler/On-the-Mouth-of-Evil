using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class beadexplosion : MonoBehaviour
{
    public Animator beadAnimator;
    [SerializeField] List<VisualEffect> particles = new List<VisualEffect>();
    // Start is called before the first frame update
    void Awake()
    {
        beadAnimator = GetComponent<Animator>();
        beadAnimator.SetBool("used", false);
        foreach (VisualEffect item in particles)
        {
            item.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {


        }

    }
}
