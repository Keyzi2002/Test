using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBag : MonoBehaviour
{
    public static string Name_SettingBag_ResetDataDefault = "Level 1";
    public Transform myTransform;
    private SettingBag settingBagCurrent;

    private void Awake()
    {
        if (myTransform == null) { myTransform = this.transform; }
        LoadData();
    }
    public void SaveData(SettingBag settingBag)
    {
        PlayerPrefs.SetString(NameData.ShovelCurrent, PathSettings.PathAssetPrefabBagSetting + settingBag.name);
    }
    public void LoadData()
    {
        if (PlayerPrefs.GetString(NameData.BagCurrent) == "")
        {
            PlayerPrefs.SetString(NameData.BagCurrent, PathSettings.PathAssetPrefabBagSetting + Name_SettingBag_ResetDataDefault);
        }
        settingBagCurrent = (SettingBag)Resources.Load(PlayerPrefs.GetString(NameData.BagCurrent), typeof(SettingBag));
    }
    public SettingBag GetSettingBag()
    {
        if (settingBagCurrent == null) { LoadData(); }
        return settingBagCurrent;
    }

}
