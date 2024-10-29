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
        SceneManager.LoadScene(nameLevel);
        CanvasManager.Instance.menu.SetActive(false);
    }
}
