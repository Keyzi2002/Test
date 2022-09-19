using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ModelData", menuName = "ScriptAbleObjects/NewModelData")]
public class ModelAbleObject : ScriptableObject {
    public List<ModelAbleObj> modelSkin;
    public List<ModelAbleObj> modelShovel;
    public List<ModelAbleObj> modelBackPack;
    public Sprite iconSkinOff;
    public Sprite iconShovelOff;
    public Sprite iconBackPackOff;
    public List<ModelSkinAbleObj> GetModelByColor(NameColor nameColor) {
        List<ModelSkinAbleObj> newModelSkins = new List<ModelSkinAbleObj>();
        foreach (ModelSkinAbleObj model in modelSkin)
        {
            if (model.color == nameColor)
                newModelSkins.Add(model);
        }
        return newModelSkins;
    }
    public ModelAbleObj GetModelSkin(int modelID, Rarity rarity) {
        foreach (ModelAbleObj model in modelSkin)
        {
            if (model.modelID == modelID && model.rarity == rarity)
                return model;
        }
        return null;
    }
    public ModelAbleObj GetModelBackPack(int modelID, Rarity rarity) {
        foreach (ModelAbleObj model in modelBackPack)
        {
            if (model.modelID == modelID && model.rarity == rarity)
                return model;
        }
        return null;
    }
    public ModelAbleObj GetModelShovel(int modelID, Rarity rarity) {
        foreach (ModelAbleObj model in modelShovel)
        {
            if (model.modelID == modelID && model.rarity == rarity)
                return model;
        }
        return null;
    }
    public List<ModelAbleObj> GetModelsShovels()
    {
        return modelShovel;
    }
    public List<ModelAbleObj> GetModelsBackPacks()
    {
        return modelBackPack;
    }
    public Sprite GetSpriteOff(ModelType modelType) {
        switch (modelType)
        {
            case ModelType.Shovel:
                return iconShovelOff;
            case ModelType.Skin:
                return iconSkinOff;
            case ModelType.BackPack:
                return iconBackPackOff;
            default:
                return null;
        }
    }
}
