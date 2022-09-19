using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataShovel : MonoBehaviour
{
    public static string Name_SettingShovel_ResetDataDefault = "Level 1";
    public Transform myTransform;
    private SettingShovel settingShovelCurrent;

    private void Awake()
    {
        if (myTransform == null) { myTransform = this.transform; }
        LoadData();
    }
    public void SaveData(SettingShovel settingShovel)
    {
        PlayerPrefs.SetString(NameData.ShovelCurrent, PathSettings.PathAssetPrefabShovelSetting_Normals + settingShovel.name);
    }
    public void LoadData()
    {
        if(PlayerPrefs.GetString(NameData.ShovelCurrent) == "")
        {
            PlayerPrefs.SetString(NameData.ShovelCurrent, PathSettings.PathAssetPrefabShovelSetting_Normals + Name_SettingShovel_ResetDataDefault);
        }
        settingShovelCurrent = (SettingShovel)Resources.Load(PlayerPrefs.GetString(NameData.ShovelCurrent), typeof(SettingShovel));
    }
    public SettingShovel GetSettingShovel()
    {
        if(settingShovelCurrent == null) { LoadData(); }
        return settingShovelCurrent;
    }

}
