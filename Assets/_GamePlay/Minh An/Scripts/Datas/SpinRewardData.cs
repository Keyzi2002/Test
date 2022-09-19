using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SpinRewardData
{
    public List<Item> itemsCommon;
    public List<Item> itemsRare;
    public List<Item> itemsEpic;
    public List<Item> itemsLegend;

    public void InitData() {
        ModelAbleObject customDataConfig = ProfileManager.Instance.profileDataConfig.modelAbleObjData;
        #region Skin
        for (int i = 0; i < customDataConfig.modelSkin.Count; i++)
        {
            if (customDataConfig.modelSkin[i].payment == Payment.Spin)
                AddItemFromModelData(customDataConfig.modelSkin[i], ModelType.Skin);
        }
        #endregion

        #region Shovel
        for (int i = 0; i < customDataConfig.modelShovel.Count; i++)
        {
            if (customDataConfig.modelShovel[i].payment == Payment.Spin)
                AddItemFromModelData(customDataConfig.modelShovel[i], ModelType.Shovel);
        }
        #endregion

        #region BackPack
        for (int i = 0; i < customDataConfig.modelBackPack.Count; i++)
        {
            if (customDataConfig.modelBackPack[i].payment == Payment.Spin)
                AddItemFromModelData(customDataConfig.modelBackPack[i], ModelType.BackPack);
        }
        #endregion

        #region AddDataConfig
        RewardData rewardData = ProfileManager.Instance.profileDataConfig.rewardData;
        for (int i = 0; i < rewardData.itemsCommon.Count; i++)
            AddItemFromDataConfig(rewardData.itemsCommon[i]);

        for (int i = 0; i < rewardData.itemsRare.Count; i++)
            AddItemFromDataConfig(rewardData.itemsRare[i]);

        for (int i = 0; i < rewardData.itemsEpic.Count; i++)
            AddItemFromDataConfig(rewardData.itemsEpic[i]);

        for (int i = 0; i < rewardData.itemsLegend.Count; i++)
            AddItemFromDataConfig(rewardData.itemsLegend[i]);
        #endregion
    }
    public void AddItemFromDataConfig(Item item) {
        switch (item.itemRarity)
        {
            case Rarity.Common:
                itemsCommon.Add(item);
                break;
            case Rarity.Rare:
                itemsRare.Add(item);
                break;
            case Rarity.Epic:
                itemsEpic.Add(item);
                break;
            case Rarity.Legend:
                itemsLegend.Add(item);
                break;
            default:
                break;
        }
    }
    public void AddItemFromModelData(ModelAbleObj data, ModelType modelType)
    {
        int newID = itemsCommon.Count + itemsRare.Count + itemsEpic.Count + itemsLegend.Count;
        string itemName = modelType.ToString();
        ItemType itemType = ItemType.Skin;
        Sprite sprite = data.iconOn;
        int modelID = data.modelID;
        ModelItem newItem = new ModelItem(itemName, newID, itemType, data.rarity, sprite, modelID, 0, modelType);
        switch (data.rarity)
        {
            case Rarity.Common:
                itemsCommon.Add(newItem);
                break;
            case Rarity.Rare:
                itemsRare.Add(newItem);
                break;
            case Rarity.Epic:
                itemsEpic.Add(newItem);
                break;
            case Rarity.Legend:
                itemsLegend.Add(newItem);
                break;
            default:
                break;
        }
    }
    public Item GetRandomItem(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return GetRandomItemCommon();
            case Rarity.Rare:
                return GetRandomItemRare();
            case Rarity.Epic:
                return GetRandomItemEpic();
            case Rarity.Legend:
                return GetRandomItemLegend();
            default:
                break;
        }
        return null;
    }
    #region GetItemRandom
    public Item GetRandomItemCommon()
    {
        int indexRand = Random.Range(0, itemsCommon.Count);
        return itemsCommon[indexRand];
    }
    public Item GetRandomItemRare()
    {
        int indexRand = Random.Range(0, itemsRare.Count);
        return itemsRare[indexRand];
    }
    public Item GetRandomItemEpic()
    {
        int indexRand = Random.Range(0, itemsEpic.Count);
        return itemsEpic[indexRand];
    }
    public Item GetRandomItemLegend()
    {
        int indexRand = Random.Range(0, itemsLegend.Count);
        return itemsLegend[indexRand];
    }
    #endregion
    public void GetReward(Item itemReward, bool doubleIt = false)
    {
        int amount = itemReward.amount;
        if (doubleIt)
            amount *= 2;
        switch (itemReward.itemType)
        {
            case ItemType.Gem:
                ProfileManager.Instance.playerData.AddGem(amount);
                break;
            case ItemType.Skin:
                if (CheckModel(itemReward as ModelItem))
                    ProfileManager.Instance.playerData.AddGem(amount);
                else AddModel(itemReward as ModelItem);
                break;
            default:
                break;
        }
    }
    void AddModel(ModelItem item) {
        switch (item.modelType)
        {
            case ModelType.Shovel:
                ProfileManager.Instance.playerData.AddShovel(item.modelID, item.itemRarity);
                break;
            case ModelType.Skin:
                ProfileManager.Instance.playerData.AddSkin(item.modelID, item.itemRarity);
                break;
            case ModelType.BackPack:
                ProfileManager.Instance.playerData.AddBackPack(item.modelID, item.itemRarity);
                break;
            default:
                break;
        }
    }
    bool CheckModel(ModelItem item) {
        switch (item.modelType)
        {
            case ModelType.Shovel:
                return ProfileManager.Instance.playerData.modelData.CheckShovelID(item.modelID, item.itemRarity);
            case ModelType.Skin:
                return ProfileManager.Instance.playerData.modelData.CheckSkinID(item.modelID, item.itemRarity);
            case ModelType.BackPack:
                return ProfileManager.Instance.playerData.modelData.CheckBackPackID(item.modelID, item.itemRarity);
            default:
                break;
        }
        return false;
    }
}
