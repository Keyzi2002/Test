using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class PanelStartGame : UIPanel
{

    [SerializeField] Button btnShowShopPanel;
    [SerializeField] Button btnShowCustomPanel;
    [SerializeField] Button btnShowSettingPanel;
    [SerializeField] Button btnStartGame;
    [SerializeField] Button btnCollectReward;
    [SerializeField] Button btnSpinWheel;
    [SerializeField] InputField ifName;
    [SerializeField] Text txtTimeCoolDown;
    [SerializeField] Text txtGem;
    [SerializeField] Text txtLevel;
    [SerializeField] Image imgIconCollectReward;
    [SerializeField] Animator animBtnReward;
    bool ableToGetReward;
    public override void Awake() {
        uiType = UIType.StartGamePanel;
        base.Awake();
    }
    private void Start()
    {
        InitData();
    }
    public override void InitData()
    {
        btnShowShopPanel.onClick.AddListener(UIManager.Instance.ShowShopPanel);
        btnShowCustomPanel.onClick.AddListener(UIManager.Instance.ShowCustomPanel);
        btnShowSettingPanel.onClick.AddListener(UIManager.Instance.ShowSettingPanel);
        btnCollectReward.onClick.AddListener(CollectReward);
        btnSpinWheel.onClick.AddListener(UIManager.Instance.ShowSpinWheelPanel);
        // ifName.onEndEdit.AddListener(LoadName);
        //btnStartGame.onClick.AddListener(StartGame);
        EnventManager.AddListener(EventName.OffTimePlayReward.ToString(), ShowTimeRewardAble);
        EnventManager.AddListener(EventName.ChangeGem.ToString(), UpdateGem);
        EnventManager.AddListener(EventName.ChangeLevel.ToString(), UpdateLevel);
        UpdateGem();
        UpdateLevel();
        animBtnReward = btnCollectReward.GetComponent<Animator>();
    }
    public void StartGame() {
        UIManager.Instance.CloseStartPanel();
#if UNITY_EDITOR
        Debug.Log("StartGame");
#endif
        GameManager.Instance.StartGame();
        //ActivateJoyStickAndGamePlay
    }
    public void LoadName()
    {
        EnventManager.TriggerEvent(EventName.LoadNamePlayer.ToString());
    }
    public string GetNamePlayer()
    {
        if (ifName.text == "")
        {
            ifName.text = nameof(Player);
        }
        return ifName.text;
    }
    private void Update()
    {
        float timeRemaining = GameManager.Instance.timeCoolDownManager.timeRemaining;
        if (timeRemaining > 0 && !ableToGetReward)
        {
            txtTimeCoolDown.text = TimeCustomManager.FormatTimeToString(timeRemaining);
            animBtnReward.SetBool("Shake", false);
        }
        else
        if (GameManager.Instance.timeCoolDownManager.CheckGetReward())
            ShowTimeRewardDisable();
        else WaitNextDay();
    }
    void UpdateGem() {
        txtGem.text = ProfileManager.Instance.playerData.GetGem().ToString();
    }
    void UpdateLevel() {
        txtLevel.text = "LEVEL " + (ProfileManager.Instance.levelData.GetLevel() + 1).ToString();
    }
    void ShowTimeRewardAble() {
        ableToGetReward = false;
    }
    void ShowTimeRewardDisable() {
        ableToGetReward = true;
        txtTimeCoolDown.text = "CLAIM";
        animBtnReward.SetBool("Shake", true);
    }
    void WaitNextDay() {
        ableToGetReward = false;
        string timeReset = ProfileManager.Instance.coolDownData.GetResetTime();
        TimeSpan timeSpan = Convert.ToDateTime(timeReset) - DateTime.Now;
        txtTimeCoolDown.text = TimeCustomManager.FormatTimeToString((float)timeSpan.TotalSeconds);
        animBtnReward.SetBool("Shake", false);
    }
    void CollectReward() {
        if (ableToGetReward)
            GameManager.Instance.timeCoolDownManager.GetReward();
        else if (GameManager.Instance.timeCoolDownManager.nextDay)
            UIManager.Instance.ShowWarningPanel(WarningType.WaittingToNextDay);
        else UIManager.Instance.ShowWarningPanel(WarningType.WaitGetReward);
    }
}
