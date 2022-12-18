using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingMessage : MonoBehaviour
{
    private Image icon;
    private TextMeshProUGUI message;
    private CanvasGroup canvasGroup;

    public static CraftingMessage instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponentsInChildren<Image>()[1];
        message = GetComponentInChildren<TextMeshProUGUI>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    public void Show(InventoryItem item)
    {
        canvasGroup.alpha = 1;
        icon.enabled = true;
        icon.sprite = item.GetItem().GetIcon();
        message.text = "You crafted: " + item.GetItem().GetName();
        Invoke(nameof(Hide), 3f);
    }

    public void Show()
    {
        canvasGroup.alpha = 1;
        icon.enabled = false;
        message.text = "Whatever you crafted... It would be safer not to touch it.";
        Invoke(nameof(Hide), 3f);
    }

    private void Hide()
    {
        canvasGroup.alpha = 0;
    }
}
