using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ChoiceUIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> choiceObjects;


    public void SetChoices(List<string> choices)
    {
        if (choices == null)
            return;
        for (int i = 0; i < choices.Count; i++)
        {
            choiceObjects[i].SetActive(true);
            string finalString = (i + 1).ToString() + ". " + choices[i];
            choiceObjects[i].GetComponent<TextMeshProUGUI>().text = finalString;
        }
    }

    public void SetInactive()
    {
        foreach (GameObject choice in choiceObjects)
            choice.SetActive(false);
        this.enabled = false;
    }
}
