using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ShopData : DataBase
{
    public List<OfferID> offerBoughts = new List<OfferID>();
    const string strShop = "ShopData";
    public override void InitData()
    {
        SetStringKey(strShop);
        base.InitData();
    }
    public override void LoadData(string jsonData) {
        ShopData dataSave = JsonUtility.FromJson<ShopData>(jsonData);
        offerBoughts = dataSave.offerBoughts;
    }
    public override void CreateNewData()
    {
        offerBoughts = new List<OfferID>();
        base.CreateNewData();
    }
    public override void SaveData() {
        base.SaveData();
        Debug.Log("SaveShopData");
    }
    public bool CheckOfferBought(OfferID offerID)
    {
        foreach (OfferID id in offerBoughts)
        {
            if (id == offerID)
                return true;
        }
        return false;
    }
}
