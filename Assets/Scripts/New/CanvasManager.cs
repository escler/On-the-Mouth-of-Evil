using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }

    public Dictionary<string, GameObject> descriptions;
    public GameObject InventoryUI, InteractionText, InventoryNameSelect, missionLevelHouse, 
        descriptionLighter, descriptionCross, descriptionBible, descriptionSalt, descriptionVoodoo, descriptionRosary, puzzleSaltPaper,
        rotateInfo, moveObjectUI, inspectImage, menu, descriptionMissionContent, descrptionMissionConten2, descriptionPuzzleContent, fps;
    public CrosshairUI crossHairUI;

    public Image crossHair;
    public GameObject fGlow, tabGlow;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
        descriptions = new Dictionary<string, GameObject>();
        descriptions.Add("Lighter", descriptionLighter);
        descriptions.Add("Cross", descriptionCross);
        descriptions.Add("Bible", descriptionBible);
        descriptions.Add("Salt", descriptionSalt);
        descriptions.Add("Voodoo Doll", descriptionVoodoo);
        descriptions.Add("Rosary", descriptionRosary);
        SceneManager.sceneLoaded += DisableInfo;
        SceneManager.sceneLoaded += DestroyCanvas;
    }

    private void DestroyCanvas(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name != "Menu") return;
        Destroy(gameObject);
    }
    private void DisableInfo(Scene scene, LoadSceneMode loadSceneMode)
    {
        crossHair.enabled = true;
        rotateInfo.SetActive(false);
        moveObjectUI.SetActive(false);
        InteractionText.SetActive(false);
        descriptionMissionContent.SetActive(false);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= DisableInfo;
        SceneManager.sceneLoaded -= DestroyCanvas;
    }

    public GameObject GetDescription(string nameItem)
    {
        if (!descriptions.ContainsKey(nameItem)) return null;
        return descriptions[nameItem];
    }
}
