using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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

    private void StartTutorial()
    {
        if (_tutorialCompleted) return;
        hubBell.enabled = true;
        exitHub.GetComponent<BoxCollider>().enabled = false;
        pcHandler.GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(Tutorial());
        StartCoroutine(FlickOutline());

    }

    public void ChangeStep()
    {
        
    }

    IEnumerator Tutorial()
    {
        tutorialUI.ChangeText("Go To The Confessional and Interact with the Window (E)");
        AddOutline(bellWindows);
        yield return new WaitUntil(() => confessionaryReached);
        tutorialUI.CompleteTask();
        yield return new WaitForSeconds(.25f);
        
        tutorialUI.ChangeText("Grab The Mission (E)");
        RemoveOutline(bellWindows);
        paper = hubBell.PaperMission.GetComponentInChildren<MeshRenderer>();
        AddOutline(paper);
        yield return new WaitUntil(() => missionGrabbed);
        pcHandler.GetComponent<BoxCollider>().enabled = true;
        tutorialUI.CompleteTask();
        yield return new WaitForSeconds(.25f);
        
        tutorialUI.ChangeText("Change the inventory (TAB) and Inspect the Mission (F)");
        RemoveOutline(paper);
        yield return new WaitUntil(() => missionInspect);
        pcHandler.GetComponent<BoxCollider>().enabled = true;
        tutorialUI.CompleteTask();
        yield return new WaitForSeconds(.25f);

        tutorialUI.ChangeText("Sit At The PC (E)");
        AddOutline(pc);
        yield return new WaitUntil(() => pcOpen);
        tutorialUI.CompleteTask();
        yield return new WaitForSeconds(.25f);
        
        tutorialUI.ChangeText("Open the Good Shop and buy one item of each type");
        RemoveOutline(pc);
        yield return new WaitUntil(() => itemsBuyed);
        tutorialUI.CompleteTask();
        yield return new WaitForSeconds(.25f);
        
        tutorialUI.ChangeText("Pick items at the store");
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
        yield return new WaitUntil(() => exithub);
        RemoveOutline(doorExit);
        tutorialUI.CompleteTask();
        PlayerPrefs.SetInt("TutorialCompleted",1);
        tutorialUI.gameObject.SetActive(false);

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
}
