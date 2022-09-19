using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class TimeCoolDownManager
{
    #region Play Time Reward
    int currentIndex;
    TimeSpan timeResetSpan;
    public float timeRemaining;
    public List<float> timeRemainingSetting;
    public List<Item> gemReward;
    TimeCoolDownData coolDownData;
    public bool nextDay;
    #endregion
    public void InitData() {
        #region PLayTimeReward
        coolDownData = ProfileManager.Instance.coolDownData;
        timeResetSpan = Convert.ToDateTime(coolDownData.GetResetTime()) - DateTime.Now;
        if (timeResetSpan <= TimeSpan.Zero)
            coolDownData.CreateNewData();

        currentIndex = coolDownData.GetCurrentIndex();
        if (currentIndex < timeRemainingSetting.Count)
        {
            TimeSpan timeGetReward = Convert.ToDateTime(coolDownData.GetTimeGetReward()) - DateTime.Now;
            if (timeGetReward <= TimeSpan.Zero)
                timeRemaining = 0f;
            else
                timeRemaining = (float)timeGetReward.TotalSeconds;
            nextDay = false;
        }
        else
        {
            TimeSpan timeGetReward = Convert.ToDateTime(coolDownData.GetResetTime()) - DateTime.Now;
            timeRemaining = (float)timeGetReward.TotalSeconds;
            nextDay = true;
        }
        #endregion
    }
    public void Update() {
        if (currentIndex >= timeRemainingSetting.Count)
            nextDay = true;
        if (timeRemaining > 0)
            timeRemaining -= Time.deltaTime;
        else if (nextDay && timeRemaining <= 0f)
        {
            InitData();
        }
    }
    void GetRewardDone() {
        currentIndex++;
        coolDownData.SetCurrentIndex(currentIndex);
        if (currentIndex >= timeRemainingSetting.Count)
        {
            coolDownData.SetResetTime(DateTime.Now.AddDays(1).ToString());
            return;
        }
        timeRemaining = timeRemainingSetting[currentIndex];
        coolDownData.SetTimeGetReward(DateTime.Now.AddSeconds(timeRemainingSetting[currentIndex]).ToString());
        EnventManager.TriggerEvent(EventName.OffTimePlayReward.ToString());
    }
    public void GetReward() {
        if (currentIndex >= timeRemainingSetting.Count)
            return;
        UIManager.Instance.ShowRewardPanel(gemReward[currentIndex], GetRewardDone);
    }
    public bool CheckGetReward() {
        if (currentIndex >= timeRemainingSetting.Count)
            return false;
        return true;
    }
}
