using System.Collections;
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

    }

    public void ChangeStep()
    {
        
    }

    IEnumerator Tutorial()
    {
        tutorialUI.ChangeText("Go To The Confessional and Interact with Window");
        yield return new WaitUntil(() => confessionaryReached);
        tutorialUI.CompleteTask();
        yield return new WaitForSeconds(.25f);
        
        tutorialUI.ChangeText("Interact and grab Mission");
        yield return new WaitUntil(() => missionGrabbed);
        pcHandler.GetComponent<BoxCollider>().enabled = true;
        tutorialUI.CompleteTask();
        yield return new WaitForSeconds(.25f);
        
        tutorialUI.ChangeText("Press TAB to change inventory and F to Inspect Mission");
        yield return new WaitUntil(() => missionInspect);
        pcHandler.GetComponent<BoxCollider>().enabled = true;
        tutorialUI.CompleteTask();
        yield return new WaitForSeconds(1f);

        tutorialUI.ChangeText("Sit At The PC");
        yield return new WaitUntil(() => pcOpen);
        tutorialUI.CompleteTask();
        yield return new WaitForSeconds(1f);
        
        tutorialUI.ChangeText("Open the Good Shop and buy one item of each type");
        yield return new WaitUntil(() => itemsBuyed);
        tutorialUI.CompleteTask();
        yield return new WaitForSeconds(1f);
        
        tutorialUI.ChangeText("Pick items at the store");
        yield return new WaitUntil(() => itemsGrabbed);
        tutorialUI.CompleteTask();
        yield return new WaitForSeconds(1f);
        
        exitHub.GetComponent<BoxCollider>().enabled = true;
        tutorialUI.ChangeText("Go to the door to start mission");
        yield return new WaitUntil(() => exithub);
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
}
