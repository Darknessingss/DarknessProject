using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public string instanceId;
    public ItemData itemData;
    public int gridX;
    public int gridY;
    public bool isRotated;

    public InventoryItem(ItemData data, int x, int y)
    {
        instanceId = Guid.NewGuid().ToString();
        itemData = data;
        gridX = x;
        gridY = y;
        isRotated = false;
    }
}