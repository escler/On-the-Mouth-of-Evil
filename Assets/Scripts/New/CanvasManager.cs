using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }

    public GameObject InventoryUI, InteractionText, InventoryNameSelect, missionLevelHouse, 
        descriptionLighter, descriptionCross, descriptionBible, descriptionSalt;
    public CrosshairUI crossHairUI;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
