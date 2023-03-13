using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settings;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenSettings()
    {
        this.gameObject.SetActive(false);
        settings.SetActive(true);
    }

    public void CloseSettings()
    {
        this.gameObject.SetActive(true);
        settings.SetActive(false);
    }
}
