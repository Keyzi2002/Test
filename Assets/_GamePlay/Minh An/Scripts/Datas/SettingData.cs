using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SettingData : DataBase
{
    public bool music;
    public bool vibaration;
    public bool sound;
    const string strSetting = "SettingData";
    public override void InitData()
    {
        SetStringKey(strSetting);
        base.InitData();
    }
    public override void LoadData(string jsonData)
    {
        SettingData settingData = JsonUtility.FromJson<SettingData>(jsonData);
        music = settingData.music;
        vibaration = settingData.vibaration;
        sound = settingData.sound;
    }
    public override void CreateNewData()
    {
        music = true;
        sound = true;
        vibaration = true;
        base.CreateNewData();
    }
    public override void SaveData()
    {
        base.SaveData();
        Debug.Log("Save setting data.");
    }
    public bool GetMusic() { return music; }
    public bool GetSound() { return sound; }
    public bool GetVibrate() { return vibaration; }
    public void SetSound(bool setting) { 
        sound = setting;
        SaveData();
    }
    public void SetMusic(bool setting) { 
        music = setting;
        AudioManager.Instance.SetMuteMusic(setting);
        SaveData();
    }
    public void SetVibrate(bool setting) { 
        vibaration = setting;
        SaveData();
    }
}
