using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutlineToggle : MonoBehaviour
{
    private Image image;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color defaultColor;
    void Start()
    {
        image = GetComponentInParent<Image>();
    }

    public void OnMouseOver()
    {
        image.color = highlightColor;
    }

    public void OnMouseExit()
    {
        image.color = defaultColor;
    }
}
