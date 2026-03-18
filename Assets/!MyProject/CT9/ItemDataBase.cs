using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDataBase : ScriptableObject
{
    public List<ItemData> items;

    public ItemData GetItemById(string id)
    {
        return items.Find(item => item.id == id);
    }

    public ItemData GetRandomItem()
    {
        if (items.Count == 0) return null;
        return items[Random.Range(0, items.Count)];
    }
}