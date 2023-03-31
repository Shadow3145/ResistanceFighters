using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ChoiceUIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> choiceObjects;

    private Tween writerTween;
    private const float writingSpeed = 15f;
    public void SetChoices(List<string> choices)
    {
        if (choices == null)
            return;
        for (int i = 0; i < choices.Count; i++)
        {
            choiceObjects[i].SetActive(true);
            string text = "";
            string finalString = (i + 1).ToString() + ". " + choices[i];
            writerTween = DOTween.To(() => text, x => text = x, finalString, finalString.Length / writingSpeed).OnUpdate(() =>
            {
                choiceObjects[i].GetComponent<TextMeshProUGUI>().text = text;
            });
        }
    }

    public void SetInactive()
    {
        foreach (GameObject choice in choiceObjects)
            choice.SetActive(false);
        this.enabled = false;
    }
}
