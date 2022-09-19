using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ModelIDData
{
   
    public List<int> commonID;
    public List<int> rareID;
    public List<int> epicID;
    public List<int> legendID;
    public int currentModelID;
    public Rarity currentModelRarity;
    public ModelIDData()
    {
        commonID = new List<int>();
        rareID = new List<int>();
        epicID = new List<int>();
        legendID = new List<int>();
    }
    public void AddCommonID(int modelID) {
        commonID.Add(modelID);
        currentModelID = modelID;
        currentModelRarity = Rarity.Common;
    }
    public void AddRareID(int modelID)
    {
        rareID.Add(modelID);
        currentModelID = modelID;
        currentModelRarity = Rarity.Rare;
    }
    public void AddEpicID(int modelID)
    {
        epicID.Add(modelID);
        currentModelID = modelID;
        currentModelRarity = Rarity.Epic;
    }
    public void AddLegendID(int modelID)
    {
        legendID.Add(modelID);
        currentModelID = modelID;
        currentModelRarity = Rarity.Legend;
    }
    public int GetCurrentModelID() { return currentModelID; }
    public Rarity GetCurrentModelRariry() { return currentModelRarity; }
    public void SetModel(int modelID, Rarity modelRarity) {
        currentModelID = modelID;
        currentModelRarity = modelRarity;
    }
    public bool CheckModelID(int modelID, Rarity rarity) {
        switch (rarity)
        {
            case Rarity.Common:
                foreach (int id in commonID)
                {
                    if (modelID == id)
                        return true;
                }
                break;
            case Rarity.Rare:
                foreach (int id in rareID)
                {
                    if (modelID == id)
                        return true;
                }
                break;
            case Rarity.Epic:
                foreach (int id in epicID)
                {
                    if (modelID == id)
                        return true;
                }
                break;
            case Rarity.Legend:
                foreach (int id in legendID)
                {
                    if (modelID == id)
                        return true;
                }
                break;
            default:
                break;
        }
        return false;
    }
}
