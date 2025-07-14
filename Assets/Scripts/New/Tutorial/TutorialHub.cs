using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TutorialHub : MonoBehaviour
{
    public static TutorialHub Instance { get; private set; }
    private bool _tutorialCompleted;
    public bool TutorialCompleted => _tutorialCompleted;
    [SerializeField] HubBell hubBell;
    [SerializeField] private PcInteractable pcHandler;
    [SerializeField] ExitHub exitHub;
    [SerializeField] private TutorialUI tutorialUI;
    public bool missionInspect;
    public bool confessionaryReached;
    public bool missionGrabbed;
    public bool pcOpen;
    public bool itemsBuyed;
    public bool itemsGrabbed;
    public int countItemBuy;
    public int countItemGrabbed;
    public bool exithub;
    public Material outlineMaterial;
    public MeshRenderer pc, bellWindows, doorExit, paper;
    public GameObject salt, lighter, bible, cross;
    public GameObject holyMarketGlow, fGlow, tabGlow;
    public GameObject[] priceGlows;
    public Color colorGlow;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        var tutorialPref = PlayerPrefs.HasKey("TutorialCompleted") ? PlayerPrefs.GetInt("TutorialCompleted") : 0;
        
        _tutorialCompleted = tutorialPref == 1;
        StartTutorial();
        tutorialUI.gameObject.SetActive(!_tutorialCompleted);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void StopCoroutines(Scene scene, LoadSceneMode loadSceneMode)
    {
        //StopAllCoroutines();
    }

    private void StartTutorial()
    {
        if (_tutorialCompleted) return;

        hubBell.enabled = true;
        exitHub.GetComponent<BoxCollider>().enabled = false;
        pcHandler.GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(Tutorial());
        StartCoroutine(FlickOutline());
        StartCoroutine(FlickUIOutline());
        ApplyColor();

    }

    private void ApplyColor()
    {
        if (tabGlow == null) tabGlow = CanvasManager.Instance.tabGlow;
        if (fGlow == null) fGlow = CanvasManager.Instance.fGlow;
        holyMarketGlow.GetComponent<Image>().color = colorGlow;
        fGlow.GetComponent<Image>().color = colorGlow;
        tabGlow.GetComponent<Image>().color = colorGlow;
        foreach (var p in priceGlows)
        {
            p.GetComponent<Image>().color = colorGlow;
        }
    }

    IEnumerator Tutorial()
    {
        tutorialUI.ChangeText("Go To The Confessional and Interact with the Window (E)");
        tutorialUI.ChangeDescription("In the confessional, you’ll receive missions from people in need of your help.\nAs you complete assignments, you’ll gain more respect unlocking new locations where you can hunt and fight demons.");
        AddOutline(bellWindows);
        yield return new WaitUntil(() => confessionaryReached);
        tutorialUI.CompleteTask();
        yield return new WaitForSeconds(.25f);
        
        tutorialUI.ChangeText("Grab The Mission (E)");
        tutorialUI.ChangeDescription("You can pick up items by pressing E to store them in your inventory, and use them later throughout the levels.\n" +
                                     "Each mission will contain information about the location, the demon, and its fatal attacks.");
        RemoveOutline(bellWindows);
        paper = hubBell.PaperMission.GetComponentInChildren<MeshRenderer>();
        AddOutline(paper);
        yield return new WaitUntil(() => missionGrabbed);
        pcHandler.GetComponent<BoxCollider>().enabled = true;
        tutorialUI.CompleteTask();
        yield return new WaitForSeconds(.25f);
        
        tutorialUI.ChangeText("Change the inventory (TAB) and Inspect the Mission (F)");
        tutorialUI.ChangeDescription("You have two inventories:\n– One for the items you bring from the confessional.\n– Another for items you find during the mission.\n\n" +
                                     "Some items can be inspected by pressing F. These inspectable items will be marked with a symbol displayed above your inventory.");
        StartCoroutine(DisableF());
        StartCoroutine(DisableTab());
        tabGlow.SetActive(true);
        fGlow.SetActive(true);
        RemoveOutline(paper);
        yield return new WaitUntil(() => missionInspect);
        tabGlow.SetActive(false);
        fGlow.SetActive(false);
        pcHandler.GetComponent<BoxCollider>().enabled = true;
        tutorialUI.CompleteTask();
        yield return new WaitForSeconds(.25f);

        tutorialUI.ChangeText("Sit At The PC (E)");
        tutorialUI.ChangeDescription("From the PC, you’ll have access to different shops where you can purchase items that will help you during your missions.\n" +
                                     "As you complete missions, new items will unlock, and you’ll earn money to improve your equipment, depending on the decisions you make.");
        AddOutline(pc);
        yield return new WaitUntil(() => pcOpen);
        tutorialUI.CompleteTask();
        yield return new WaitForSeconds(.25f);
        
        tutorialUI.ChangeText("Open the Good Shop and buy one item of each type");
        tutorialUI.ChangeDescription("Open the “Good Shop” and buy one item of each type.\n" +
                                     "In each store, you can preview how an item works by hovering over its name to see a tutorial before purchasing.");
        holyMarketGlow.SetActive(true);
        foreach (var p in priceGlows)
        {
            p.SetActive(true);
        }
        RemoveOutline(pc);
        yield return new WaitUntil(() => itemsBuyed);
        tutorialUI.CompleteTask();
        yield return new WaitForSeconds(.25f);
        
        tutorialUI.ChangeText("Pick items at the store");
        tutorialUI.ChangeDescription("Once you’ve bought your items, they will be available in your storage. Be careful—if you fail a mission, any items you took with you will be lost.");
        salt = SaltHandler.Instance.salts.First();
        lighter = LighterHandler.Instance.lighters.First();
        bible = BibleHandler.Instance.bibles.First();
        cross = CrossHandler.Instance.crosses.First();
        var saltMats = salt.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var mat in saltMats)
        {
            AddOutline(mat);
        }
        
        var lighterMats = lighter.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var mat in lighterMats)
        {
            AddOutline(mat);
        }
        
        var bibleMats = bible.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var mat in bibleMats)
        {
            AddOutline(mat);
        }
        
        var crossMat = cross.GetComponentInChildren<MeshRenderer>();
        AddOutline(crossMat);

        yield return new WaitUntil(() => itemsGrabbed);

        foreach (var mat in saltMats)
        {
            RemoveOutline(mat);
        }
        
        foreach (var mat in lighterMats)
        {
            RemoveOutline(mat);
        }
        
        foreach (var mat in bibleMats)
        {
            RemoveOutline(mat);
        }
        
        RemoveOutline(crossMat);
        tutorialUI.CompleteTask();
        yield return new WaitForSeconds(.25f);
        
        exitHub.GetComponent<BoxCollider>().enabled = true;
        AddOutline(doorExit);
        tutorialUI.ChangeText("Go to the door to start mission");
        tutorialUI.ChangeDescription("When you’re ready, head to the exit door to begin your first mission.");
        yield return new WaitUntil(() => exithub);
        RemoveOutline(doorExit);
        tutorialUI.CompleteTask();
        PlayerPrefs.SetInt("TutorialCompleted",1);
        PlayerPrefs.Save();
        tutorialUI.gameObject.SetActive(false);

    }

    IEnumerator DisableTab()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Tab));
        tabGlow.SetActive(false);
    }

    IEnumerator DisableF()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));
        fGlow.SetActive(false);
    }

    public void CheckStoreBuy()
    {
        itemsBuyed = countItemBuy == 4;
    }

    public void CheckGrabbedItems()
    {
        itemsGrabbed = countItemGrabbed == 4;
    }

    private void AddOutline(MeshRenderer rend)
    {
        var existingMat = rend.sharedMaterials;
        Material[] newMats = new Material[existingMat.Length + 1];
        for (int i = 0; i < existingMat.Length; i++)
        {
            newMats[i] = existingMat[i];
        }
        newMats[existingMat.Length] = outlineMaterial;
        rend.materials = newMats;
    }

    private void RemoveOutline(MeshRenderer rend)
    {
        var existingMat = rend.sharedMaterials;
        Material[] newMats = new Material[existingMat.Length - 1];
        for (int i = 0; i < newMats.Length; i++)
        {
            newMats[i] = existingMat[i];
        }
        rend.materials = newMats;
    }
    
    private void AddOutline(SkinnedMeshRenderer rend)
    {
        var existingMat = rend.sharedMaterials;
        Material[] newMats = new Material[existingMat.Length + 1];
        for (int i = 0; i < existingMat.Length; i++)
        {
            newMats[i] = existingMat[i];
        }
        newMats[existingMat.Length] = outlineMaterial;
        rend.materials = newMats;
    }

    private void RemoveOutline(SkinnedMeshRenderer rend)
    {
        var existingMat = rend.sharedMaterials;
        Material[] newMats = new Material[existingMat.Length - 1];
        for (int i = 0; i < newMats.Length; i++)
        {
            newMats[i] = existingMat[i];
        }
        rend.materials = newMats;
    }

    IEnumerator FlickOutline()
    {
        float amount = 1;
        float condition = -1;
        bool increased = false;
        while (!exithub)
        {
            if(amount >= 1f) increased = false;
            else if(amount <= 0f) increased = true;

            float delta = Time.deltaTime;
            amount += increased ? delta : -delta;
            amount = Mathf.Clamp01(amount);
            outlineMaterial.SetFloat("_Alpha", amount);
            yield return null;
        }
    }

    IEnumerator FlickUIOutline()
    {
        float amount = 1;
        float condition = -1;
        bool increased = false;
        while (!exithub)
        {
            if(amount >= 1f) increased = false;
            else if(amount <= 0f) increased = true;

            float delta = Time.deltaTime;
            amount += increased ? delta : -delta;
            amount = Mathf.Clamp01(amount);
            colorGlow.a = amount;
            ApplyColor();
            yield return null;
        }
    }
}
