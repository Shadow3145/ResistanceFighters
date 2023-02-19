using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
            choiceObjects[i].GetComponent<TextMeshProUGUI>().text = (i + 1).ToString() + ". " + choices[i];
        }
    }

    public void SetInactive()
    {
        foreach (GameObject choice in choiceObjects)
            choice.SetActive(false);
        this.enabled = false;
    }
}
