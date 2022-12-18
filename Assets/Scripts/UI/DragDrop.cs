using UnityEngine;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour
{
    private InventoryItem dragItem = null;
    [SerializeField] private Sprite defaultImage;

    public void SetItem(InventoryItem item)
    {
        dragItem = item;
        GetComponent<Image>().sprite = ItemManager.instance.GetItem(dragItem).GetIcon();
        GetComponent<CanvasGroup>().alpha = 1;
    }

    public void ResetItem()
    {
        dragItem = null;
        GetComponent<Image>().sprite = defaultImage;
        GetComponent<CanvasGroup>().alpha = 0;
    }
}
