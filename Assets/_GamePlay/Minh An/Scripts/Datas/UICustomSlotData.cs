using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Payment { 
    Gem,
    Watch,
    Offer,
    Spin,
    LevelGet
}
[System.Serializable]
public class UICustomSlotData
{
    public int modelID;
    public Sprite iconOn;
    public Rarity modelRarity;
    public Payment payment;
    public int priceAmount;
}
                                                                                                        