using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject slotPrefab;
    public Transform slotsContainer;
    public GameObject itemPrefab;
    public Transform itemsContainer;

    [Header("Info Panel")]
    public GameObject infoPanel;
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI itemRarityText;

    [Header("Settings")]
    public int gridWidth = 8;
    public int gridHeight = 6;
    public float cellSize = 100f;

    [Header("Items Database")]
    public List<ItemData> availableItems;

    private InventorySlot[,] slots;
    private Dictionary<string, InventoryItem> items = new Dictionary<string, InventoryItem>();
    private Dictionary<string, GameObject> itemObjects = new Dictionary<string, GameObject>();
    private InventoryItem draggedItem;
    private Vector2 dragOffset;
    private string savePath;
    private InventoryItem selectedItem;
    private GridLayoutGroup gridLayout;

    void Start()
    {
        savePath = Path.Combine(Application.persistentDataPath, "inventory_save.json");
        Debug.Log("Save path: " + savePath);

        gridLayout = slotsContainer.GetComponent<GridLayoutGroup>();
        if (gridLayout != null)
        {
            cellSize = gridLayout.cellSize.x;
        }

        InitializeGrid();
        LoadInventory();
    }

    void InitializeGrid()
    {
        slots = new InventorySlot[gridWidth, gridHeight];

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                GameObject slotObj = Instantiate(slotPrefab, slotsContainer);

                if (gridLayout == null)
                {
                    RectTransform rect = slotObj.GetComponent<RectTransform>();
                    rect.anchoredPosition = new Vector2(x * cellSize, -y * cellSize);
                }

                InventorySlot slot = slotObj.GetComponent<InventorySlot>();
                slot.Initialize(x, y, this);
                slots[x, y] = slot;
            }
        }

        if (gridLayout == null)
        {
            RectTransform containerRect = slotsContainer.GetComponent<RectTransform>();
            containerRect.sizeDelta = new Vector2(gridWidth * cellSize, gridHeight * cellSize);
        }
    }

    public void SaveInventory()
    {
        SaveData saveData = new SaveData
        {
            gridWidth = gridWidth,
            gridHeight = gridHeight
        };

        foreach (var item in items.Values)
        {
            SerializableItem serializableItem = new SerializableItem
            {
                instanceId = item.instanceId,
                itemId = item.itemData.id,
                gridX = item.gridX,
                gridY = item.gridY,
                isRotated = item.isRotated,
                itemName = item.itemData.itemName,
                description = item.itemData.description,
                rarity = item.itemData.rarity,
                iconPath = item.itemData.iconPath
            };
            saveData.items.Add(serializableItem);
        }

        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        File.WriteAllText(savePath, json);
        Debug.Log("Inventory saved!");
    }

    public void LoadInventory()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No save file found");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);

        ClearInventoryObjects();

        foreach (var serializableItem in saveData.items)
        {
            ItemData itemData = FindItemById(serializableItem.itemId);
            if (itemData == null)
            {
                itemData = ScriptableObject.CreateInstance<ItemData>();
                itemData.id = serializableItem.itemId;
                itemData.itemName = serializableItem.itemName;
                itemData.description = serializableItem.description;
                itemData.rarity = serializableItem.rarity;
                itemData.iconPath = serializableItem.iconPath;

                if (!string.IsNullOrEmpty(serializableItem.iconPath))
                {
                    itemData.icon = Resources.Load<Sprite>(serializableItem.iconPath);
                }
            }

            InventoryItem item = new InventoryItem(itemData, serializableItem.gridX, serializableItem.gridY);
            item.instanceId = serializableItem.instanceId;
            item.isRotated = serializableItem.isRotated;

            PlaceItemInGrid(item, serializableItem.gridX, serializableItem.gridY);
        }
    }

    ItemData FindItemById(string id)
    {
        return availableItems.Find(item => item.id == id);
    }

    void ClearInventoryObjects()
    {
        items.Clear();
        foreach (var itemObj in itemObjects.Values)
        {
            Destroy(itemObj);
        }
        itemObjects.Clear();
    }

    void PlaceItemInGrid(InventoryItem item, int x, int y)
    {
        if (!CanPlaceItem(item, x, y)) return;

        item.gridX = x;
        item.gridY = y;

        GameObject itemObj;

        if (itemObjects.ContainsKey(item.instanceId))
        {
            itemObj = itemObjects[item.instanceId];
        }
        else
        {
            itemObj = Instantiate(itemPrefab, itemsContainer);
            itemObjects[item.instanceId] = itemObj;

            InventoryItemView itemView = itemObj.GetComponent<InventoryItemView>();
            itemView.Initialize(item, this);
        }

        RectTransform rect = itemObj.GetComponent<RectTransform>();

        float itemWidth = item.itemData.width * cellSize;
        float itemHeight = item.itemData.height * cellSize;

        float centerX = (x + (item.itemData.width - 1) * 0.5f) * cellSize;
        float centerY = -(y + (item.itemData.height - 1) * 0.5f) * cellSize;

        rect.anchoredPosition = new Vector2(centerX, centerY);
        rect.sizeDelta = new Vector2(itemWidth, itemHeight);

        for (int ix = 0; ix < item.itemData.width; ix++)
        {
            for (int iy = 0; iy < item.itemData.height; iy++)
            {
                slots[x + ix, y + iy].SetOccupied(item);
            }
        }

        items[item.instanceId] = item;

        SaveInventory();
    }

    bool CanPlaceItem(InventoryItem item, int x, int y)
    {
        if (x < 0 || y < 0 ||
            x + item.itemData.width > gridWidth ||
            y + item.itemData.height > gridHeight)
            return false;

        for (int ix = 0; ix < item.itemData.width; ix++)
        {
            for (int iy = 0; iy < item.itemData.height; iy++)
            {
                if (slots[x + ix, y + iy].IsOccupied)
                    return false;
            }
        }

        return true;
    }

    public void SpawnRandomItem()
    {
        if (availableItems.Count == 0) return;

        ItemData randomItemData = availableItems[Random.Range(0, availableItems.Count)];

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                InventoryItem newItem = new InventoryItem(randomItemData, x, y);
                if (CanPlaceItem(newItem, x, y))
                {
                    PlaceItemInGrid(newItem, x, y);
                    return;
                }
            }
        }

        Debug.Log("No free space!");
    }

    public void RemoveItem(InventoryItem item)
    {
        if (item == null) return;

        for (int ix = 0; ix < item.itemData.width; ix++)
        {
            for (int iy = 0; iy < item.itemData.height; iy++)
            {
                slots[item.gridX + ix, item.gridY + iy].SetOccupied(null);
            }
        }

        if (itemObjects.ContainsKey(item.instanceId))
        {
            Destroy(itemObjects[item.instanceId]);
            itemObjects.Remove(item.instanceId);
        }

        items.Remove(item.instanceId);

        if (selectedItem == item)
        {
            HideItemInfo();
        }

        SaveInventory();
    }

    public void ClearAllItems()
    {
        List<InventoryItem> itemsToRemove = new List<InventoryItem>(items.Values);
        foreach (var item in itemsToRemove)
        {
            RemoveItem(item);
        }
    }

    public void ShowItemInfo(InventoryItem item)
    {
        selectedItem = item;
        infoPanel.SetActive(true);

        itemIcon.sprite = item.itemData.icon;
        itemNameText.text = item.itemData.itemName;
        itemDescriptionText.text = item.itemData.description;
        itemRarityText.text = item.itemData.rarity.ToString();

        switch (item.itemData.rarity)
        {
            case ItemRarity.Common:
                itemRarityText.color = Color.white;
                break;
            case ItemRarity.Rare:
                itemRarityText.color = Color.blue;
                break;
            case ItemRarity.Epic:
                itemRarityText.color = new Color(0.5f, 0, 0.5f);
                break;
            case ItemRarity.Legendary:
                itemRarityText.color = new Color(1, 0.5f, 0);
                break;
        }
    }

    void HideItemInfo()
    {
        infoPanel.SetActive(false);
        selectedItem = null;
    }

    public void StartDrag(InventoryItem item, Vector2 position)
    {
        draggedItem = item;

        RectTransform itemRect = itemObjects[item.instanceId].GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            itemsContainer.GetComponent<RectTransform>(),
            position,
            null,
            out localPoint
        );

        dragOffset = localPoint - itemRect.anchoredPosition;
        itemObjects[item.instanceId].transform.SetAsLastSibling();
    }

    public void OnDrag(Vector2 position)
    {
        if (draggedItem == null) return;

        RectTransform itemRect = itemObjects[draggedItem.instanceId].GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            itemsContainer.GetComponent<RectTransform>(),
            position,
            null,
            out localPoint
        );

        itemRect.anchoredPosition = localPoint - dragOffset;
    }

    public void EndDrag(Vector2 position)
    {
        if (draggedItem == null) return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            itemsContainer.GetComponent<RectTransform>(),
            position,
            null,
            out localPoint
        );

        int targetX = Mathf.RoundToInt(localPoint.x / cellSize - (draggedItem.itemData.width - 1) * 0.5f);
        int targetY = Mathf.RoundToInt(-localPoint.y / cellSize - (draggedItem.itemData.height - 1) * 0.5f);

        targetX = Mathf.Clamp(targetX, 0, gridWidth - draggedItem.itemData.width);
        targetY = Mathf.Clamp(targetY, 0, gridHeight - draggedItem.itemData.height);

        TryMoveItem(draggedItem, targetX, targetY);

        draggedItem = null;
    }

    void TryMoveItem(InventoryItem item, int newX, int newY)
    {
        if (CanPlaceItem(item, newX, newY))
        {
            for (int ix = 0; ix < item.itemData.width; ix++)
            {
                for (int iy = 0; iy < item.itemData.height; iy++)
                {
                    slots[item.gridX + ix, item.gridY + iy].SetOccupied(null);
                }
            }

            PlaceItemInGrid(item, newX, newY);
        }
        else
        {
            InventoryItem occupyingItem = GetOccupyingItem(newX, newY, item.itemData.width, item.itemData.height);

            if (occupyingItem != null)
            {
                SwapItems(item, occupyingItem);
            }
            else
            {
                UpdateItemPosition(item);
            }
        }
    }

    InventoryItem GetOccupyingItem(int x, int y, int width, int height)
    {
        HashSet<InventoryItem> itemsInArea = new HashSet<InventoryItem>();

        for (int ix = 0; ix < width; ix++)
        {
            for (int iy = 0; iy < height; iy++)
            {
                if (x + ix < gridWidth && y + iy < gridHeight)
                {
                    InventoryItem item = slots[x + ix, y + iy].GetOccupyingItem();
                    if (item != null)
                        itemsInArea.Add(item);
                }
            }
        }

        return itemsInArea.Count == 1 ? itemsInArea.FirstOrDefault() : null;
    }

    void SwapItems(InventoryItem item1, InventoryItem item2)
    {
        int x1 = item1.gridX;
        int y1 = item1.gridY;
        int x2 = item2.gridX;
        int y2 = item2.gridY;

        for (int ix = 0; ix < item1.itemData.width; ix++)
        {
            for (int iy = 0; iy < item1.itemData.height; iy++)
            {
                slots[x1 + ix, y1 + iy].SetOccupied(null);
            }
        }

        for (int ix = 0; ix < item2.itemData.width; ix++)
        {
            for (int iy = 0; iy < item2.itemData.height; iy++)
            {
                slots[x2 + ix, y2 + iy].SetOccupied(null);
            }
        }

        item1.gridX = x2;
        item1.gridY = y2;
        item2.gridX = x1;
        item2.gridY = y1;

        UpdateItemPosition(item1);
        UpdateItemPosition(item2);

        for (int ix = 0; ix < item1.itemData.width; ix++)
        {
            for (int iy = 0; iy < item1.itemData.height; iy++)
            {
                slots[x2 + ix, y2 + iy].SetOccupied(item1);
            }
        }

        for (int ix = 0; ix < item2.itemData.width; ix++)
        {
            for (int iy = 0; iy < item2.itemData.height; iy++)
            {
                slots[x1 + ix, y1 + iy].SetOccupied(item2);
            }
        }

        SaveInventory();
    }

    void UpdateItemPosition(InventoryItem item)
    {
        if (!itemObjects.ContainsKey(item.instanceId)) return;

        GameObject itemObj = itemObjects[item.instanceId];
        RectTransform rect = itemObj.GetComponent<RectTransform>();

        float centerX = (item.gridX + (item.itemData.width - 1) * 0.5f) * cellSize;
        float centerY = -(item.gridY + (item.itemData.height - 1) * 0.5f) * cellSize;

        rect.anchoredPosition = new Vector2(centerX, centerY);
    }
}