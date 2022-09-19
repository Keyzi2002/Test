using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class ModelData {

    public ModelIDData backPackIDs;
    public ModelIDData shovelIDs;
    public ModelIDData skinIDs;
    public ModelData()
    {
        backPackIDs = new ModelIDData();
        shovelIDs = new ModelIDData();
        skinIDs = new ModelIDData();
        AddBackPack(0, Rarity.Common);
        AddShovel(0, Rarity.Common);
        AddSkin(0, Rarity.Common);
    }
    public bool CheckBackPackID(int bpID, Rarity rarity)
    {
        return backPackIDs.CheckModelID(bpID, rarity);
    }
    public bool CheckSkinID(int skinID, Rarity rarity)
    {
        return skinIDs.CheckModelID(skinID, rarity);
    }
    public bool CheckShovelID(int shovelID, Rarity rarity) {
        return shovelIDs.CheckModelID(shovelID, rarity);
    }

    public int GetCurrentBackPackID() { return backPackIDs.GetCurrentModelID(); }
    public int GetCurrentShovelID() { return shovelIDs.GetCurrentModelID(); }
    public int GetCurrentSkinID() { return skinIDs.GetCurrentModelID(); }

    public Rarity GetCurrentBackPackRarity() { return backPackIDs.GetCurrentModelRariry(); }
    public Rarity GetCurrentShovelRarity() { return shovelIDs.GetCurrentModelRariry(); }
    public Rarity GetCurrentSkinRarity() { return skinIDs.GetCurrentModelRariry(); }

    public void AddBackPack(int backPackID, Rarity rarity) {
        switch (rarity)
        {
            case Rarity.Common:
                backPackIDs.AddCommonID(backPackID);
                break;
            case Rarity.Rare:
                backPackIDs.AddRareID(backPackID);
                break;
            case Rarity.Epic:
                backPackIDs.AddEpicID(backPackID);
                break;
            case Rarity.Legend:
                backPackIDs.AddLegendID(backPackID);
                break;
            default:
                break;
        }
    }
    public void AddShovel(int shovelID, Rarity rarity) {
        switch (rarity)
        {
            case Rarity.Common:
                shovelIDs.AddCommonID(shovelID);
                break;
            case Rarity.Rare:
                shovelIDs.AddRareID(shovelID);
                break;
            case Rarity.Epic:
                shovelIDs.AddEpicID(shovelID);
                break;
            case Rarity.Legend:
                shovelIDs.AddLegendID(shovelID);
                break;
            default:
                break;
        }
    }
    public void AddSkin(int skinID, Rarity rarity) {
        switch (rarity)
        {
            case Rarity.Common:
                skinIDs.AddCommonID(skinID);
                break;
            case Rarity.Rare:
                skinIDs.AddRareID(skinID);
                break;
            case Rarity.Epic:
                skinIDs.AddEpicID(skinID);
                break;
            case Rarity.Legend:
                skinIDs.AddLegendID(skinID);
                break;
            default:
                break;
        }
    }

    public void SetCurrentBackPackModel(int modelID, Rarity rarity) { backPackIDs.SetModel(modelID, rarity);  }
    public void SetCurrentShovelModel(int modelID, Rarity rarity) { shovelIDs.SetModel(modelID, rarity); }
    public void SetCurrentSkinModel(int modelID, Rarity rarity) { skinIDs.SetModel(modelID, rarity); }
}
public enum SpinWheelState { 
    FreeSpin,
    WatchSpin,
    CantSpin
}
[System.Serializable]
public class SpinWheelData
{
    public string endTimeSpin;
    public SpinWheelState wheelState;
    public SpinWheelState CheckCanSpinToday() {
        if (endTimeSpin == "")
        {
            SetEndTimeSpin();
            wheelState = SpinWheelState.FreeSpin;
            return SpinWheelState.FreeSpin;
        }

        DateTime timeCurrentCheck = DateTime.Now;
        TimeSpan timeSpan =  Convert.ToDateTime(endTimeSpin) - timeCurrentCheck;
        if (timeSpan <= TimeSpan.Zero)
        {
            wheelState = SpinWheelState.FreeSpin;
            return SpinWheelState.FreeSpin;
        }

        wheelState = SpinWheelState.CantSpin;
        return SpinWheelState.CantSpin;
    }
    public void SetEndTimeSpin() { endTimeSpin = DateTime.Now.AddDays(1).ToString(); }
    public SpinWheelState GetWheelState() { return wheelState; }
    public TimeSpan GetRemainingTime() {
        return Convert.ToDateTime(endTimeSpin) - DateTime.Now;
    }
    public string GetTimeReset() { return endTimeSpin; }
    public void ReduceEndtime(float timeReduce) {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeReduce);
        DateTime dateTime = Convert.ToDateTime(endTimeSpin).Subtract(timeSpan);
        endTimeSpin = dateTime.ToString();
    }
}
[System.Serializable]
public class PlayerData : DataBase
{
    public ModelData modelData;
    public SpinWheelData spinWheelData = new SpinWheelData();
    public int gem;
    const string strPlayerDataKey = "PlayerData";
    public override void InitData()
    {
        SetStringKey(strPlayerDataKey);
        base.InitData();
        EnventManager.AddListener(EventName.SaveTimeWatchVideo.ToString(), SetLastTimeSpin);
    }
    public override void LoadData(string jsonData) {
        PlayerData data = JsonUtility.FromJson<PlayerData>(jsonData);
        gem = data.gem;
        //levelDataSave = data.levelDataSave;
        modelData = data.modelData;
        spinWheelData = data.spinWheelData;
    }
    public override void CreateNewData()
    {
        gem = 1000000;
        modelData = new ModelData();
        base.CreateNewData();
    }
    public override void SaveData() {
        base.SaveData();
        Debug.Log("Save Player Data");
    }
    #region Spin
    public SpinWheelState CheckCanSpinWheel() { return spinWheelData.CheckCanSpinToday(); }
    public void SetLastTimeSpin() {
        spinWheelData.SetEndTimeSpin();
        SaveData();
    }
    public SpinWheelState GetWheelState() { return spinWheelData.GetWheelState(); }
    public TimeSpan GetSpinRemainingTime() { return spinWheelData.GetRemainingTime(); }
    public void ReduceEndTime(float timereduce) {
        spinWheelData.ReduceEndtime(timereduce);
        SaveData();
    }
    #endregion

    #region Model
    public bool CheckBackPackID(int backPackID, Rarity rarity) { return modelData.CheckBackPackID(backPackID, rarity); }
    public bool CheckShovelID(int shovelID, Rarity rarity) { return modelData.CheckShovelID(shovelID, rarity); }
    public bool CheckSkinID(int skinID, Rarity rarity) { return modelData.CheckSkinID(skinID, rarity); }

    public void AddBackPack(int backPackID, Rarity rarity) { modelData.AddBackPack(backPackID, rarity); SaveData(); }
    public void AddShovel(int shovelID, Rarity rarity) { 
        modelData.AddShovel(shovelID, rarity); 
        SaveData();
        EnventManager.TriggerEvent(EventName.ChangeSkinOnPlayer.ToString());
    }
    public void AddSkin(int skinID, Rarity rarity) { 
        modelData.AddSkin(skinID, rarity);
        SaveData();
        EnventManager.TriggerEvent(EventName.ChangeSkinOnPlayer.ToString());
    }

    public int GetCurrentBackPackID() { return modelData.GetCurrentBackPackID(); }
    public int GetCurrentSkinID() { return modelData.GetCurrentSkinID(); }
    public int GetCurrentShovelID() { return modelData.GetCurrentShovelID(); }

    public Rarity GetCurrentBackPackRarity() { return modelData.GetCurrentBackPackRarity(); }
    public Rarity GetCurrentSkinRarity() { return modelData.GetCurrentSkinRarity(); }
    public Rarity GetCurrentShovelRarity() { return modelData.GetCurrentShovelRarity(); }

    public void SetCurrentBackPackID(int modelID, Rarity rarity) {
        modelData.SetCurrentBackPackModel(modelID, rarity);
        SaveData();
    }
    public void SetCurrentSkinID(int modelID, Rarity rarity) {
        modelData.SetCurrentSkinModel(modelID, rarity);
        SaveData();
    }
    public void SetCurrentShovelID(int modelID, Rarity rarity) {
        modelData.SetCurrentShovelModel(modelID, rarity);
        SaveData();
    }
    #endregion

    #region Gem
    public int GetGem() { return gem; }
    public void ConsumeGem(int coinChange) { 
        gem -= coinChange; 
        SaveData();
        EnventManager.TriggerEvent(EventName.ChangeGem.ToString());
    }
    public void AddGem(int coinChange) { 
        gem += coinChange; 
        SaveData();
        EnventManager.TriggerEvent(EventName.ChangeGem.ToString());
    }
    public bool CheckEnoughGem(int coinPare) { return coinPare <= GetGem(); }
    #endregion
}
