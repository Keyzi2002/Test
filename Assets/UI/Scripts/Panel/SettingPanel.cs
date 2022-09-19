using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : UIPanel
{
    [SerializeField] Button btnSound;
    [SerializeField] Button btnVibration;
    [SerializeField] Button btnMusic;
    [SerializeField] Button btnRestore;
    [SerializeField] Button btnExit;
    public Sprite sprOn, sprOff;
    bool mute;
    public override void Awake()
    {
        uiType = UIType.SettingPanel;
        base.Awake();
        btnSound.onClick.AddListener(ChangeSettingSound);
        btnVibration.onClick.AddListener(ChangeSettingVibration);
        btnMusic.onClick.AddListener(ChangeSettingMusic);
        btnRestore.onClick.AddListener(RestoreButton);
        btnExit.onClick.AddListener(OnClose);
    }
    void ChangeSettingSound() {
        SettingData settingData = ProfileManager.Instance.settingData;
        settingData.SetSound(!settingData.GetSound());
        if (settingData.GetSound())
            btnSound.image.sprite = sprOn;
        else btnSound.image.sprite = sprOff;
        ChangeActiavate(btnSound.transform);
    }
    void ChangeSettingVibration() {
        SettingData settingData = ProfileManager.Instance.settingData;
        settingData.SetVibrate(!settingData.GetVibrate());
        if (settingData.GetVibrate())
            btnVibration.image.sprite = sprOn;
        else btnVibration.image.sprite = sprOff;
        ChangeActiavate(btnVibration.transform);
    }
    void ChangeSettingMusic() {
        SettingData settingData = ProfileManager.Instance.settingData;
        settingData.SetMusic(!settingData.GetMusic());
        if (settingData.GetMusic())
            btnMusic.image.sprite = sprOn;
        else btnMusic.image.sprite = sprOff;
        ChangeActiavate(btnMusic.transform);
    }
    void RestoreButton() { }
    void ChangeActiavate(Transform btnChange) {
        for (int i = 0; i < btnChange.childCount; i++)
        {
            if (btnChange.GetChild(i).gameObject.activeSelf)
                btnChange.GetChild(i).gameObject.SetActive(false);
            else btnChange.GetChild(i).gameObject.SetActive(true);
        }
    }
    public override void OnClose() { UIManager.Instance.CloseSettingPanel(); }
}
