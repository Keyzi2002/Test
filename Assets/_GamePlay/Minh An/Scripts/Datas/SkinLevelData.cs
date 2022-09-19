using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LevelGetSkin {
    public int levelGet;
    public ModelAbleObj modelData;
    public LevelGetSkin(int levelGet, ModelAbleObj modelData) {
        this.levelGet = levelGet;
        this.modelData = modelData;
    }
}
[System.Serializable]
public class SkinLevelData 
{
    public List<LevelGetSkin> modelGetByLevel = new List<LevelGetSkin>();
    public void InitData() {
        ModelAbleObject customDataConfig = ProfileManager.Instance.profileDataConfig.modelAbleObjData;
        #region Skin
        for (int i = 0; i < customDataConfig.modelSkin.Count; i++)
        {
            if (customDataConfig.modelSkin[i].payment == Payment.LevelGet)
                AddItem(customDataConfig.modelSkin[i], ModelType.Skin);
        }
        #endregion

        #region Shovel
        for (int i = 0; i < customDataConfig.modelShovel.Count; i++)
        {
            if (customDataConfig.modelShovel[i].payment == Payment.LevelGet)
                AddItem(customDataConfig.modelShovel[i], ModelType.Shovel);
        }
        #endregion

        #region BackPack
        for (int i = 0; i < customDataConfig.modelBackPack.Count; i++)
        {
            if (customDataConfig.modelBackPack[i].payment == Payment.LevelGet)
                AddItem(customDataConfig.modelBackPack[i], ModelType.BackPack);
        }
        #endregion
    }
    public void AddItem(ModelAbleObj data, ModelType modelType)
    {
        ModelAbleObj modelData = data;
        LevelGetSkin newLevelGetSkin = new LevelGetSkin(data.priceAmount, modelData);
        modelGetByLevel.Add(newLevelGetSkin);
    }
    public LevelGetSkin GetLevelMinDistance(int level) {
        LevelGetSkin levelGetSkinReturn = null;
        int minDistance = modelGetByLevel[0].levelGet - level;
        for (int i = 0; i < modelGetByLevel.Count; i++)
        {
            int calculateResult = modelGetByLevel[i].levelGet - level;
            if (minDistance < 0)
            {
                levelGetSkinReturn = modelGetByLevel[i];
                minDistance = modelGetByLevel[i].levelGet - level;
                continue;
            }
            if (calculateResult <= minDistance && calculateResult >= 0)
            {
                levelGetSkinReturn = modelGetByLevel[i];
                minDistance = modelGetByLevel[i].levelGet - level;
            }
        }
        return levelGetSkinReturn;
    }
}
