using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Button chestButton;
    public Button clearAllButton;

    void Start()
    {
        if (chestButton != null)
            chestButton.onClick.AddListener(OnChestClicked);

        if (clearAllButton != null)
            clearAllButton.onClick.AddListener(OnClearAllClicked);
    }

    void OnChestClicked()
    {
        inventoryManager?.SpawnRandomItem();
    }

    void OnClearAllClicked()
    {
        inventoryManager?.ClearAllItems();
    }
}