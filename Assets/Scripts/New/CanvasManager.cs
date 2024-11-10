using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance { get; private set; }

    public Dictionary<string, GameObject> descriptions;
    public GameObject InventoryUI, InteractionText, InventoryNameSelect, missionLevelHouse, 
        descriptionLighter, descriptionCross, descriptionBible, descriptionSalt, puzzleSaltPaper,
        rotateInfo, moveObjectUI, inspectImage, menu, descriptionTextContent, fps, loadingScreen;
    public CrosshairUI crossHairUI;

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
        SceneManager.sceneLoaded += DisableInfo;
    }

    private void DisableInfo(Scene scene, LoadSceneMode loadSceneMode)
    {
        rotateInfo.SetActive(false);
        moveObjectUI.SetActive(false);
        InteractionText.SetActive(false);
        descriptionTextContent.SetActive(false);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= DisableInfo;
    }

    public GameObject GetDescription(string nameItem)
    {
        if (!descriptions.ContainsKey(nameItem)) return null;
        return descriptions[nameItem];
    }
}
