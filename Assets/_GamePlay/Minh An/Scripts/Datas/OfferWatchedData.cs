using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ModelWatched {
    public ModelWatched(int modelID, Rarity rarity) {
        this.modelID = modelID;
        this.modelRarity = rarity;
        watchedCount = 0;
    }
    public int modelID;
    public Rarity modelRarity;
    public int watchedCount;
    public void IncreaseWatched() { watchedCount++; }
}
[System.Serializable]
public class OfferWatchedData : DataBase
{
    const string modelStr = "OfferWatchedData";
    public List<ModelWatched> modelWatcheds = new List<ModelWatched>();
    public override void InitData()
    {
        SetStringKey(modelStr);
        base.InitData();
    }
    public override void CreateNewData()
    {
        modelWatcheds = new List<ModelWatched>();
        base.CreateNewData();
    }
    public override void SaveData()
    {
        Debug.Log("Save OfferWatched data.");
        base.SaveData();
    }
    public override void LoadData(string jsonData)
    {
        OfferWatchedData data = JsonUtility.FromJson<OfferWatchedData>(jsonData);
        modelWatcheds = data.modelWatcheds;
        base.LoadData(jsonData);
    }
    public void RegisterOffer(ModelWatched offerWatched) { modelWatcheds.Add(offerWatched); }
    public ModelWatched GetOfferWatched(int modelID, Rarity rarity) {
        ModelWatched offerWatched = CheckOffer(modelID, rarity);
        
        if (offerWatched == null)
        {
            offerWatched = new ModelWatched(modelID, rarity);
            RegisterOffer(offerWatched);
        }
        
        return offerWatched;
    }
    ModelWatched CheckOffer(int modelID, Rarity rarity) {
        for (int i = 0; i < modelWatcheds.Count; i++)
        {
            if (modelID == modelWatcheds[i].modelID && rarity == modelWatcheds[i].modelRarity)
                return modelWatcheds[i];
        }
        return null;
    }
    public void IncreaseWatched(ModelWatched modelWatched) {
        for (int i = 0; i < modelWatcheds.Count; i++)
        {
            if (modelWatched == modelWatcheds[i])
                modelWatcheds[i].IncreaseWatched();
        }
        SaveData();
    }
}
