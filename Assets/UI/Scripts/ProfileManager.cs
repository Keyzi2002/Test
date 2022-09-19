using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : GenerticSingleton<ProfileManager>
{
    public override void Awake()
    {
        InitData();
    }
    private void Start()
    {
        EnventManager.TriggerEvent(EventName.ChangeSkinOnPlayer.ToString());
    }
    void InitData() {
        spinReward.InitData();
        modelLevelData.InitData();
        playerData.InitData();
        shopData.InitData();
        levelData.InitData();
        coolDownData.InitData();
        skinBotManager.InitData();
        offerWatchedData.InitData();
        settingData.InitData();
        GameManager.Instance.timeCoolDownManager.InitData();
        EnventManager.TriggerEvent(EventName.ChangeSkinOnPlayer.ToString());
    }
    public ProfileDataConfig profileDataConfig;
    public PlayerData playerData = new PlayerData();
    public ShopData shopData = new ShopData();
    public LevelSaveData levelData = new LevelSaveData();
    public TimeCoolDownData coolDownData = new TimeCoolDownData();
    public SkinBotManager skinBotManager = new SkinBotManager();
    public OfferWatchedData offerWatchedData = new OfferWatchedData();
    public SpinRewardData spinReward = new SpinRewardData();
    public SkinLevelData modelLevelData = new SkinLevelData();
    public SettingData settingData = new SettingData();
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            playerData.ClearData();
            shopData.ClearData();
            levelData.ClearData();
            coolDownData.ClearData();
            offerWatchedData.ClearData();
            settingData.ClearData(); 
            Debug.Log("Clear All Data");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            playerData.SaveData();
            shopData.SaveData();
            levelData.SaveData();
            coolDownData.SaveData();
            offerWatchedData.SaveData();
            settingData.SaveData();
            Debug.Log("Save All Data");
        }
    }
}
