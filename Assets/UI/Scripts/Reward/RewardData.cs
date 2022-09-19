using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RewardData", menuName = "ScriptAbleObjects/New Reward Data")]
public class RewardData : ScriptableObject
{
    public List<Item> itemsCommon;
    public List<Item> itemsRare;
    public List<Item> itemsEpic;
    public List<Item> itemsLegend;
    public List<DecorItem> decorItem;

    #region Get item by ID
    public Item GetItemCommon(int itemID) {
        foreach (Item item in itemsCommon)
        {
            if (item.itemID == itemID)
                return item;
        }
        return null;
    }
    public Item GetItemRare(int itemID) {
        foreach (Item item in itemsRare)
        {
            if (item.itemID == itemID)
                return item;
        }
        return null;
    }
    public Item GetItemEpic(int itemID) {
        foreach (Item item in itemsEpic)
        {
            if (item.itemID == itemID)
                return item;
        }
        return null;
    }
    public Item GetItemLegend(int itemID) {
        foreach (Item item in itemsLegend)
        {
            if (item.itemID == itemID)
                return item;
        }
        return null;
    }
    #endregion
   

    public Item GetItemByID(int itemID, Rarity rarity) {
        switch (rarity)
        {
            case Rarity.Common:
                return GetItemCommon(itemID);
            case Rarity.Rare:
                return GetItemRare(itemID);
            case Rarity.Epic:
                return GetItemEpic(itemID);
            case Rarity.Legend:
                return GetItemLegend(itemID);
            default:
                break;
        }
        return null;
    }
    public DecorItem GetDecore(Rarity rarity) {
        foreach (DecorItem decorItem in decorItem)
        {
            if (decorItem.rarity == rarity)
                return decorItem;
        }
        return null;
    }
    
    
}
 