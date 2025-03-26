using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    public Button playBTN, quitBTN, optionsBTN, backOptionsBTN, deleteProgressBTN, yesProgressBTN, noProgressBTN;
    public GameObject mainButtons, options, progressConfirm;

    private void Awake()
    {
        playBTN.onClick.AddListener(LoadHubScene);
        quitBTN.onClick.AddListener(QuitGame);
        optionsBTN.onClick.AddListener(OpenOptions);
        backOptionsBTN.onClick.AddListener(CloseOptions);
        deleteProgressBTN.onClick.AddListener(OpenConfirmProgress);
        yesProgressBTN.onClick.AddListener(ClearProgress);
        noProgressBTN.onClick.AddListener(CloseConfirmProgress);
    }

    private void OnDestroy()
    {
        playBTN.onClick.RemoveAllListeners();
        quitBTN.onClick.RemoveAllListeners();
        optionsBTN.onClick.RemoveAllListeners();
        backOptionsBTN.onClick.RemoveAllListeners();
        deleteProgressBTN.onClick.RemoveAllListeners();
        yesProgressBTN.onClick.RemoveAllListeners();
        noProgressBTN.onClick.RemoveAllListeners();
    }

    private void LoadHubScene()
    {
        GameManagerNew.Instance.LoadSceneWithDelay("Hub", 5f);
    }

    private void OpenOptions()
    {
        mainButtons.SetActive(false);
        options.SetActive(true);
    }

    private void CloseOptions()
    {
        options.SetActive(false);
        mainButtons.SetActive(true);
        progressConfirm.SetActive(false);
    }

    private void QuitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }

    private void OpenConfirmProgress()
    {
        progressConfirm.SetActive(true);
        deleteProgressBTN.gameObject.SetActive(false);
    }

    private void CloseConfirmProgress()
    {
        progressConfirm.SetActive(false);
        deleteProgressBTN.gameObject.SetActive(true);
    }

    private void ClearProgress()
    {
        ProgressManager.Instance.ResetPrefs();
        CloseConfirmProgress();
    }
}
