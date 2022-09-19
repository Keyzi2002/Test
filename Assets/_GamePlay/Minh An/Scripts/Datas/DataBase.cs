using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DataBase
{
    string strKey = "";
    public virtual void InitData() {
        string jsonData = GetData();
        if (!string.IsNullOrEmpty(jsonData))
        {
            LoadData(jsonData);
        }
        else
        {
            CreateNewData();
        }
    }
    public virtual void SetStringKey(string keyChange) { strKey = keyChange; }
    public virtual void SaveData() { PlayerPrefs.SetString(strKey, JsonUtility.ToJson(this).ToString()); }
    public virtual string GetData() { return PlayerPrefs.GetString(strKey); }
    public virtual void LoadData(string jsonData) { }
    public virtual void CreateNewData() { SaveData(); }
    public virtual void ClearData() { PlayerPrefs.DeleteKey(strKey); }
}
