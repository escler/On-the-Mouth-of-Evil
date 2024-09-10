using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }

    public Dictionary<string, GameObject> descriptions;
    public GameObject InventoryUI, InteractionText, InventoryNameSelect, missionLevelHouse, 
        descriptionLighter, descriptionCross, descriptionBible, descriptionSalt, puzzleSaltPaper;
    public CrosshairUI crossHairUI;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        descriptions = new Dictionary<string, GameObject>();
        descriptions.Add("Lighter", descriptionLighter);
        descriptions.Add("Cross", descriptionCross);
        descriptions.Add("Bible", descriptionBible);
        descriptions.Add("Salt", descriptionSalt);
    }

    public GameObject GetDescription(string nameItem)
    {
        if (!descriptions.ContainsKey(nameItem)) return null;
        return descriptions[nameItem];
    }
}
