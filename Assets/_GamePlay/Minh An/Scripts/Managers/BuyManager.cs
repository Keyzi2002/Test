using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuyManager : GenerticSingleton<BuyManager>
{
    public void CustomOutFitBuy(int modelID, int priceAmount, ModelType modelType, Rarity rarity, UnityAction buySucess, UnityAction buyFaild) {
        ProfileManager profileManager = ProfileManager.Instance;
        if (profileManager.playerData.CheckEnoughGem(priceAmount))
        {
            switch (modelType)
            {
                case ModelType.Shovel:
                    profileManager.playerData.AddShovel(modelID, rarity);
                    break;
                case ModelType.Skin:
                    profileManager.playerData.AddSkin(modelID, rarity);
                    break;
                case ModelType.BackPack:
                    profileManager.playerData.AddBackPack(modelID, rarity);
                    break;
                default:
                    break;
            }
            profileManager.playerData.ConsumeGem(priceAmount);
            buySucess();
        }
        else buyFaild();
    }
}
