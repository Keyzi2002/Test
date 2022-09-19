using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RewardType { 
    Coin,
    Skin,
    NonADS
}
[System.Serializable]
public class OfferData
{
    public Sprite icon;
    public string productID;
    public string titleDeal;
    [TextArea(1,10)]
    public string descriptions;
    public float price;
    public OfferID offerID;
    //public List<RendererTextureData> rendererTextureDatas = new List<RendererTextureData>();
    public List<int> itemID;
}
