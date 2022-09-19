using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WarningPopUpPanel : UIPanel
{
    [SerializeField] Text txtDescription;
    [SerializeField] Button btnWatchVideo;
    [SerializeField] Button btnClose;
    public float maxTimeReduce;
    public float minTimeReduce;
    float timeReduce;
    int amountReduce;
    WarningType currentWarningType;
    public override void Awake()
    {
        uiType = UIType.WarningPanel;
        base.Awake();
        btnWatchVideo.onClick.AddListener(WatchVideo);
        btnClose.onClick.AddListener(OnClose);
    }
    public void SettingPanel(WarningType warningType, int amount = 0) {
        currentWarningType = warningType;
        switch (warningType)
        {
            case WarningType.SpinWheelInCoolDown:
                txtDescription.text = "Time remaining: ";
                SettingWatchToReduceSpinTime();
                break;
            case WarningType.NotEnoughGem:
                txtDescription.text = "You don't have enough gems!\n";
                btnWatchVideo.gameObject.SetActive(false);
                //SettingWatchToGet(amount);
                break;
            case WarningType.WaittingToNextDay:
                txtDescription.text = "Wait until next day to get more gems!\n";
                SettingWatchToReduceResetGemTime();
                break;
            case WarningType.WaitGetReward:
                txtDescription.text = "Wait time end to get more gems!\n";
                SettingWatchToReduceGetGemTime();
                break;
            default:
                break;
        }
    }
    void SettingWatchToReduceGetGemTime() {
        btnWatchVideo.gameObject.SetActive(true);

        string timeGetReward = "";
        TimeSpan timeSpan = TimeSpan.Zero;
        float timeRemaining = 0f;
        timeGetReward = ProfileManager.Instance.coolDownData.GetTimeGetReward();

        timeSpan = Convert.ToDateTime(timeGetReward) - DateTime.Now;
        timeRemaining = (float)timeSpan.TotalSeconds;
        timeReduce = timeRemaining;
        txtDescription.text += " Watch to reduce " + TimeCustomManager.FormatTimeToString(timeReduce);
    }
    void SettingWatchToReduceResetGemTime() {
        btnWatchVideo.gameObject.SetActive(true);
        string timeGetReward = "";
        TimeSpan timeSpan = TimeSpan.Zero;
        float timeRemaining = 0f;
        timeGetReward = ProfileManager.Instance.coolDownData.GetResetTime();
        timeSpan = Convert.ToDateTime(timeGetReward) - DateTime.Now;
        timeRemaining = (float)timeSpan.TotalSeconds;
        timeReduce = GetRandomTime(timeRemaining);
        txtDescription.text += " Watch to reduce " + TimeCustomManager.FormatTimeToString(timeReduce);
    }
    void SettingWatchToReduceSpinTime() {
        btnWatchVideo.gameObject.SetActive(true);
        float timeRemaining = (float)ProfileManager.Instance.playerData.GetSpinRemainingTime().TotalSeconds;
        txtDescription.text += TimeCustomManager.FormatTimeToString(timeRemaining) + " to get free spin!\n";
        timeReduce = timeRemaining;
        txtDescription.text += " Watch to get one free spin!";
    }
    float GetRandomTime(float timeRemaining) {
        if (timeRemaining < 7200f)
            timeReduce = (float)timeRemaining;
        else timeReduce = 7200f;
        return timeReduce;
    }
    void SettingWatchToGet(int amount) {
        btnWatchVideo.gameObject.SetActive(true);
        amountReduce = (int)((amount - ProfileManager.Instance.playerData.GetGem()) * 0.01f);
        if (amountReduce < 30)
            amountReduce = 30;
        if (amountReduce > 0)
            txtDescription.text += " Watch to get " + amountReduce.ToString()+ " Gems"; 
        else btnWatchVideo.gameObject.SetActive(false);
    }
    void WatchVideo() {
        Debug.Log("Watch video");
        switch (currentWarningType)
        {
            case WarningType.NotEnoughGem:
                ProfileManager.Instance.playerData.AddGem(amountReduce);
                break;
            case WarningType.SpinWheelInCoolDown:
                ProfileManager.Instance.playerData.ReduceEndTime(timeReduce);
                break;
            case WarningType.WaittingToNextDay:
                ProfileManager.Instance.coolDownData.ReduceResetTime(timeReduce);
                break;
            case WarningType.WaitGetReward:
                ProfileManager.Instance.coolDownData.ReduceTimeGetReward(timeReduce);
                break;
            default:
                break;
        }
        OnClose();
    }
    public override void OnClose()
    {
        UIManager.Instance.CloseWarningPanel();
    }
}
