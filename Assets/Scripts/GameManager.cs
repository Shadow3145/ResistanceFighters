using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject camp;
    [SerializeField] private GameObject lab;
    [SerializeField] private GameObject alchemyTable;

    public void Quit()
    {
        Application.Quit();   
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
        Debug.Log("Open Alchemy Table UI");
    }
}
