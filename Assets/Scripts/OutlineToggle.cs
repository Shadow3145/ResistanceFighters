using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutlineToggle : MonoBehaviour
{
    private Outline[] outlines;
    private Image image;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color defaultColor;
    void Start()
    {
        image = GetComponentInParent<Image>();
        outlines = GetComponentsInParent<Outline>();
        foreach (Outline outline in outlines)
            outline.enabled = false;
    }

    public void OnMouseOver()
    {
        foreach (Outline outline in outlines)
            outline.enabled = true;
        image.color = highlightColor;
    }

    public void OnMouseExit()
    {
        foreach (Outline outline in outlines)
            outline.enabled = false;
        image.color = defaultColor;
    }
}
