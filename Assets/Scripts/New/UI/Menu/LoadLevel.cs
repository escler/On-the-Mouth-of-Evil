using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public string nameLevel;
    public void LoadScene()
    {
        GameManagerNew.Instance.LoadSceneWithDelay(nameLevel, 2f);
        GetComponentInParent<MenuHandler>().SwitchMenu();
        PlayerHandler.Instance.playerCam.CameraLock = false;
    }
}
