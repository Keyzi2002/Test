using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class TimeCoolDownData : DataBase
{
    const string timeCoolDownData = "TimeCoolDownData";
    public int currentIndex;
    public string timeResetSpan;
    public string timeGetReward;
    public override void InitData()
    {
        SetStringKey(timeCoolDownData);
        base.InitData();
    }
    public override void LoadData(string jsonData)
    {
        TimeCoolDownData data = JsonUtility.FromJson<TimeCoolDownData>(jsonData);
        currentIndex = data.currentIndex;
        timeResetSpan = data.timeResetSpan;
        timeGetReward = data.timeGetReward;
    }
    public override void CreateNewData()
    {
        currentIndex = 0;
        timeResetSpan = DateTime.Now.AddDays(1).ToString();
        timeGetReward = DateTime.Now.AddMinutes(10).ToString();
        base.CreateNewData();
    }
    public override void SaveData()
    {
        base.SaveData();
        Debug.Log("Save TimeCoolDown Data");
    }
    public int GetCurrentIndex() { return currentIndex; }
    public string GetResetTime() { return timeResetSpan; }
    public string GetTimeGetReward() { return timeGetReward; }
    public void SetCurrentIndex(int indexChange) { currentIndex = indexChange; SaveData(); }
    public void SetResetTime(string timeResetChange) { timeResetSpan = timeResetChange; SaveData(); }
    public void SetTimeGetReward(string timeGetRewardChange) { timeGetReward = timeGetRewardChange; SaveData(); }
    public void ReduceResetTime(float timeReduce) {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeReduce);
        DateTime dateTime = Convert.ToDateTime(timeResetSpan).Subtract(timeSpan);
        timeResetSpan = dateTime.ToString();
        GameManager.Instance.timeCoolDownManager.InitData();
        SaveData();
    }
    public void ReduceTimeGetReward(float timeReduce) {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeReduce);
        DateTime dateTime = Convert.ToDateTime(timeGetReward).Subtract(timeSpan);
        timeGetReward = dateTime.ToString();
        GameManager.Instance.timeCoolDownManager.InitData();
        SaveData();
    }
}
