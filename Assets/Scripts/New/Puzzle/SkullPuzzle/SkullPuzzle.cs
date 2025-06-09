using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullPuzzle : MonoBehaviour
{
    public SkullPuzzleSlot[] slots;
    private int count;
    public GameObject ps;
    [SerializeField] Animator freezerAnimator;
    public static SkullPuzzle Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void CheckPuzzleState()
    {
        count = 0;
        foreach (var slot in slots)
        {
            if (slot.currentSkull == null) break;
            if (slot.numberSlot != slot.currentSkull.numberSlot) break;
            if (slot.correctRotation != slot.actualRotation) break;
            count++;
        }
        
        if(count < slots.Length) return;
        
        //DecisionsHandler.Instance.BadChoiceTaked();

        foreach (var slot in slots)
        {
            slot.DisableSlot();
        }

        freezerAnimator.SetBool("Open", true);
        ps.SetActive(true);
        //RitualManager.Instance.AltarCompleted();
    }
}
