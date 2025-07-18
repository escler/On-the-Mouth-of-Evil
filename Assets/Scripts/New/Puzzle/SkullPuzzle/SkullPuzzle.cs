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
    [SerializeField] private AudioSource fridgeOpen;
    public static SkullPuzzle Instance { get; private set; }
    
    public BoxCollider freezerCollider;
    public BoxCollider[] freezerColliders;

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

        var skulls = 0;
        foreach (var s in slots)
        {
            if (s.currentSkull != null) skulls++;
        }

        if (skulls >= slots.Length && count < slots.Length)
            DialogHandler.Instance.ChangeText(
                "These skulls don’t seem to be in the right position… maybe I should take a closer look.");
        
        if(count < slots.Length) return;
        
        //DecisionsHandler.Instance.BadChoiceTaked();

        foreach (var slot in slots)
        {
            slot.DisableSlot();
        }

        freezerAnimator.SetBool("Open", true);
        fridgeOpen.Play();
        ps.SetActive(true);
        //RitualManager.Instance.AltarCompleted();
    }
}
