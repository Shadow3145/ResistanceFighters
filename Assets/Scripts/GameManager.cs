using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject camp;
    [SerializeField] private GameObject lab;
    [SerializeField] private GameObject alchemyTable;
    [SerializeField] private GameObject settings;

   /* [Header("SFX")]
    [SerializeField] private SoundName resistanceLeader;
    [SerializeField] private SoundName entrance;
    [SerializeField] private SoundName alchemyTableSFX;
    [SerializeField] private SoundName addToPot;
    [SerializeField] private SoundName success;
    [SerializeField] private SoundName fail;
    [SerializeField] private SoundName hover;
    [SerializeField] private SoundName click;*/

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Settings();
    }

    public void GoToLab()
    {
        camp.SetActive(false);
        lab.SetActive(true);
    }

    public void GoToCamp()
    {
        camp.SetActive(true);
        lab.SetActive(false);
    }

    public void TalkToLeader()
    {
        Debug.Log("Open Dialogue");
    }

    public void StartAlchemy()
    {
        alchemyTable.SetActive(true);
        lab.SetActive(false);
    }

    public void LeaveAlchemy()
    {
        alchemyTable.SetActive(false);
        lab.SetActive(true);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Settings()
    {
        settings.SetActive(!settings.activeInHierarchy);
    }
}
