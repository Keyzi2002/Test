using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LevelSaveData : DataBase
{
    public int currentLevel;
    //public List<int> stars;
    const string strLevelKey = "LevelData";
    public override void InitData()
    {
        SetStringKey(strLevelKey);
        base.InitData();
    }
    public override void LoadData(string jsonData)
    {
        LevelSaveData data = JsonUtility.FromJson<LevelSaveData>(jsonData);
        currentLevel = data.currentLevel;
        //stars = data.stars;
    }
    public override void CreateNewData()
    {
        currentLevel = 0;
        //stars = new List<int>();
        base.CreateNewData();
    }
    public override void SaveData()
    {
        base.SaveData();
        Debug.Log("Save Level Data");
    }
    public void LevelUp() {
        int levelMax = ProfileManager.Instance.profileDataConfig.levelDataConfig.levels.Count;
        if (currentLevel + 1 < levelMax)
        { 
            currentLevel++;
            EnventManager.TriggerEvent(EventName.ChangeLevel.ToString());
            SaveData();
        }
    }
    public int GetLevel() { return currentLevel; }
    public void SetStar(int level, int starChange)
    {
        //stars[level] = starChange;
    }
    public int GetStar(int level)
    {
        //if (level < stars.Count)
        //    return stars[level];
        return -1;
    }
}
