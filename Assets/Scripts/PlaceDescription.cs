using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PlaceDescription : MonoBehaviour
{
    [TextArea(3, 10)]
    [SerializeField] private List<string> descriptions;
    private int index = -1;

    private TextMeshProUGUI textMeshPro;

    private Tween writerTween;
    private const float writingSpeed = 15f;

    private void Start()
    {
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        Next();
    }

    private void WriteText()
    {
        string text = "";
        writerTween = DOTween.To(() => text, x => text = x, descriptions[index], descriptions[index].Length / writingSpeed).OnUpdate(() =>
        {
            textMeshPro.text = text;
        });        
    }

    public void Next()
    {
        index++;
        if (index >= descriptions.Count)
        {
            Close();
            return;
        }
        WriteText();
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}
