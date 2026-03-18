using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemView : MonoBehaviour,
    IPointerClickHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    private InventoryItem item;
    private InventoryManager inventoryManager;
    private Image itemImage;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        itemImage = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void Initialize(InventoryItem inventoryItem, InventoryManager manager)
    {
        item = inventoryItem;
        inventoryManager = manager;

        if (item?.itemData?.icon != null && itemImage != null)
        {
            itemImage.sprite = item.itemData.icon;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            inventoryManager?.ShowItemInfo(item);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        inventoryManager?.StartDrag(item, eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        inventoryManager?.OnDrag(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        inventoryManager?.EndDrag(eventData.position);
    }
}