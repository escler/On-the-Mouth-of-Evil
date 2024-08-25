using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public void OnPressButton()
    {
        Application.Quit();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("Level");
    }
}
