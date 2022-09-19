using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class SkinDataFollowColor {
    public List<ModelSkinAbleObj> skinAbleObjs;
    public NameColor nameColor;
}
[System.Serializable]
public class BackPackFollow
{
    public List<ModelAbleObj> modelAbleObjs;
}
[System.Serializable]
public class ShovelFollow
{
    public List<ModelAbleObj> modelAbleObjs;
}
[System.Serializable]
public class SkinBotManager
{
    public List<SkinDataFollowColor> skinDataFollowColors = new List<SkinDataFollowColor>();
    public BackPackFollow backPackFollows;
    public ShovelFollow shovelFollow;

    public void InitData() {
        for (int i = 0; i < Enum.GetNames(typeof(NameColor)).Length; i++)
        {
            SkinDataFollowColor newSkinData = new SkinDataFollowColor();
            newSkinData.nameColor = (NameColor)i;
            List<ModelSkinAbleObj> newListSkin = new List<ModelSkinAbleObj>();
            newListSkin = ProfileManager.Instance.profileDataConfig.modelAbleObjData.GetModelByColor((NameColor)i);
            newSkinData.skinAbleObjs = newListSkin;
            skinDataFollowColors.Add(newSkinData);
        }
        backPackFollows.modelAbleObjs = ProfileManager.Instance.profileDataConfig.modelAbleObjData.GetModelsBackPacks();
        shovelFollow.modelAbleObjs = ProfileManager.Instance.profileDataConfig.modelAbleObjData.GetModelsShovels();
    }
    public ModelSkinAbleObj GetSkinRandom(NameColor nameColor) {
        foreach (SkinDataFollowColor listSkin in skinDataFollowColors)
        {
            if (listSkin.nameColor == nameColor)
            {
                int indexSkin = UnityEngine.Random.Range(0, listSkin.skinAbleObjs.Count);
                return listSkin.skinAbleObjs[indexSkin];
            }
        }
        return null;
    }
    public ModelAbleObj GetBackPackRandom()
    {
        if(backPackFollows.modelAbleObjs.Count > 0)
        {
            return backPackFollows.modelAbleObjs[UnityEngine.Random.Range(0, backPackFollows.modelAbleObjs.Count)];
        }
        return null;
    }
    public ModelAbleObj GetShovelRandom()
    {
        if (shovelFollow.modelAbleObjs.Count > 0)
        {
            return shovelFollow.modelAbleObjs[UnityEngine.Random.Range(0, shovelFollow.modelAbleObjs.Count)];
        }
        return null;
    }
}
