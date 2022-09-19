using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Item
{
    public string itemName;
    public int itemID;
    public ItemType itemType;
    public Rarity itemRarity;
    public Sprite itemSprite;
    public int amount;
}
[System.Serializable]
public class ModelItem : Item {
    public int modelID;
    public ModelType modelType;
    public ModelItem(string itemName = "Skin", int itemID = 0, ItemType itemType = ItemType.Gem, Rarity rarity = Rarity.Common, Sprite itemSprite = null, int modelID = 0, int amount = 0, ModelType modelType = ModelType.Skin)
    {
        this.itemType = itemType;
        this.itemName = itemName;
        this.itemID = itemID;
        this.itemRarity = rarity;
        this.itemSprite = itemSprite;
        this.modelID = modelID;
        this.amount = amount;
        this.modelType = modelType;
    }
}
