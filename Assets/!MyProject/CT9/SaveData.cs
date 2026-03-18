using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<SerializableItem> items = new List<SerializableItem>();
    public int gridWidth = 8;
    public int gridHeight = 6;
}

[Serializable]
public class SerializableItem
{
    public string instanceId;
    public string itemId;
    public int gridX;
    public int gridY;
    public bool isRotated;
    public string itemName;
    public string description;
    public ItemRarity rarity;
    public string iconPath;
}