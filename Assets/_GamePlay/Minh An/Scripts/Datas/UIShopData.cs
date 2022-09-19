using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "New Shop Data", menuName = "ScriptAbleObjects/NewShopData")]
public class UIShopData : ScriptableObject
{
    public Sprite[] sprConfig;
    public List<OfferData> offerDatas = new List<OfferData>();

    public void OnEnable()
    {
        //LoadOffer();
    }
    void LoadOffer() {
        offerDatas.Clear();
        OfferData offer1 = new OfferData();
        offer1.titleDeal = "SUPER PACK";
        offer1.productID = "SUPERPACK";
        offer1.price = 1.99f;
        offer1.icon = GetSpriteByName("Character_Sample01_s");
        offer1.descriptions = "Remove all ads + Exclusive skin + Pet (x2 crytals)";
        offer1.offerID = OfferID.SupperPack;

        //offer1.itemRewards = new List<ItemReward>() {
        //    new ItemReward(){ type=ItemType.Gem, amount = 100 },
        //    new ItemReward(){ type=ItemType.Cuon, amount = 100 }
        //};

        OfferData offer2 = new OfferData();
        offer2.titleDeal = "KING OUTFIT";
        offer2.productID = "KINGOUTFIT";
        offer2.price = 2.99f;
        offer2.icon = GetSpriteByName("Character_Sample05_m");
        //offer2.offerID = OfferID.KingOutFit;
        offer2.descriptions = "Unlock exclusive skin";

        //offer2.itemRewards = new List<ItemReward>() {
        //    new ItemReward(){ type=ItemType.Gem, amount = 100 },
        //    new ItemReward(){ type=ItemType.Cuon, amount = 100 }
        //};

        OfferData offer3 = new OfferData();
        offer3.titleDeal = "BONUS + PET";
        offer3.productID = "BONUSANDPET";
        offer3.price = 3.99f;
        offer3.icon = GetSpriteByName("Character_Sample06_s");
        //offer3.offerID = OfferID.BonusAndPet;
        offer3.descriptions = "x2 crytals collect";

        //offer3.itemRewards = new List<ItemReward>() {
        //    new ItemReward(){ type=ItemType.Gem, amount = 100 },
        //    new ItemReward(){ type=ItemType.Cuon, amount = 100 }
        //};

        OfferData offer4 = new OfferData();
        offer4.titleDeal = "NO ADS";
        offer4.productID = "NOADS";
        offer4.price = 4.99f;
        offer4.icon = GetSpriteByName("Character_Sample12");
        //offer4.offerID = OfferID.NoADS;
        offer4.descriptions = "Play without any ads";
        //offer4.itemRewards = new List<ItemReward>() {
        //    new ItemReward(){ type=ItemType.Gem, amount = 100 },
        //    new ItemReward(){ type=ItemType.Cuon, amount = 100 }
        //};

        OfferData offer5 = new OfferData();
        offer5.titleDeal = "Spiral king";
        offer5.productID = "SPIRALKING";
        offer5.price = 4.99f;
        offer5.icon = GetSpriteByName("Character_Sample12");
        //offer5.offerID = OfferID.NoADS;
        offer5.descriptions = "Always start battle withs 5 extra spirals";
        //offer4.itemRewards = new List<ItemReward>() {
        //    new ItemReward(){ type=ItemType.Gem, amount = 100 },
        //    new ItemReward(){ type=ItemType.Cuon, amount = 100 }
        //};

        offerDatas.Add(offer4);
        offerDatas.Add(offer1);
        offerDatas.Add(offer2);
        offerDatas.Add(offer3);
        offerDatas.Add(offer5);
    }
    Sprite GetSpriteByName(string nameSpr) {
        foreach (Sprite spr in sprConfig)
        {
            if (spr.name == nameSpr)
                return spr;
        }
        return null;
    }
    public OfferData GetOfferDataByID(OfferID offerIDGet) {
        for (int i = 0; i < offerDatas.Count; i++)
        {
            if (offerDatas[i].offerID == offerIDGet)
                return offerDatas[i];
        }
        return null;
    }
}
