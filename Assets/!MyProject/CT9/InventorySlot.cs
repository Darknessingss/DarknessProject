using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public int x;
    public int y;

    private InventoryManager inventoryManager;
    private InventoryItem occupyingItem;
    private UnityEngine.UI.Image slotImage;

    public bool IsOccupied => occupyingItem != null;

    void Start()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        slotImage = GetComponent<UnityEngine.UI.Image>();

        if (slotImage != null)
        {
            Color color = slotImage.color;
            color.a = 0.3f;
            slotImage.color = color;
        }
    }

    public void Initialize(int gridX, int gridY, InventoryManager manager)
    {
        x = gridX;
        y = gridY;
        inventoryManager = manager;
    }

    public void SetOccupied(InventoryItem item)
    {
        occupyingItem = item;
    }

    public InventoryItem GetOccupyingItem()
    {
        return occupyingItem;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (occupyingItem != null)
            {
                inventoryManager.RemoveItem(occupyingItem);
            }
        }
    }
}