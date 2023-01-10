using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI description;

    public static Tooltip instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void ShowItemTooltip(InventoryItem item)
    {
        header.text = item.GetItem().GetName();
        SetIcon(item.GetItem().GetIcon());
        description.text = item.GetItem().GetDescription();
    }

    public void HideItemTooltip()
    {
        header.text = "";
        icon.color = new Color(1f, 1f, 1f, 0f);
        description.text = "";
    }

    private void SetIcon(Sprite icon)
    {
        this.icon.sprite = icon;
        this.icon.color = new Color(1f, 1f, 1f, 1f);
    }
}
