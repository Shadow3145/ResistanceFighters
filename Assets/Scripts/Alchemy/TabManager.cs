using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TabManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> tabs;
    [SerializeField] private List<GameObject> inventories;
    
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color clickColor;

    
    private int selected;

    private void Start()
    {
        Select(0);
    }

    public void OnMouseHover(int i)
    {
        tabs[i].GetComponent<Image>().color = hoverColor;
    }

    public void OnMouseLeave(int i)
    {
        tabs[i].GetComponent<Image>().color = Color.white;
    }

    public void OnMouseClick(int i)
    {
        Unselect(selected);
        Select(i);     
    }

    private void Unselect(int index)
    {
        tabs[index].GetComponent<Image>().color = Color.white;
        tabs[index].GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        inventories[index].GetComponent<CanvasGroup>().interactable = false;
        inventories[index].GetComponent<CanvasGroup>().alpha = 0f;
        inventories[index].GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    private void Select(int index)
    {
        tabs[index].GetComponent<Image>().color = clickColor;
        tabs[index].GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
        inventories[index].GetComponent<CanvasGroup>().interactable = true;
        inventories[index].GetComponent<CanvasGroup>().alpha = 1f;
        inventories[index].GetComponent<CanvasGroup>().blocksRaycasts = true;
        selected = index;
    }
}
