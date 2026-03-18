using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
[Serializable]
public class ItemData : ScriptableObject
{
    public string id;
    public string itemName;
    [TextArea(3, 5)]
    public string description;
    public Sprite icon;
    public int width = 1;
    public int height = 1;
    public ItemRarity rarity;
    public string iconPath;
}

public enum ItemRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}