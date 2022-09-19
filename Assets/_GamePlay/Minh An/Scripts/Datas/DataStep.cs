using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStep : MonoBehaviour
{
    public static string Name_SettingStep_ResetDataDefault = "Level 1";
    public Transform myTransform;
    private SettingStep settingStepCurrent;

    private void Awake()
    {
        if (myTransform == null) { myTransform = this.transform; }
        LoadData();
    }
    public void SaveData(DataStep dataStep)
    {
        PlayerPrefs.SetString(NameData.StepCurrent, PathSettings.PathAssetPrefabStepSetting + dataStep.name);
    }
    public void LoadData()
    {
        if (PlayerPrefs.GetString(NameData.StepCurrent) == "")
        {
            PlayerPrefs.SetString(NameData.StepCurrent, PathSettings.PathAssetPrefabStepSetting + Name_SettingStep_ResetDataDefault);
        }
        settingStepCurrent = (SettingStep)Resources.Load(PlayerPrefs.GetString(NameData.StepCurrent), typeof(SettingStep));
    }
    public SettingStep GetSettingStep()
    {
        if(settingStepCurrent == null) { LoadData(); }
        return settingStepCurrent;
    }
}
