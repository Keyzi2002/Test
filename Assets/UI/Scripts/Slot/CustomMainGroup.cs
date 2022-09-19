using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomMainGroup : MonoBehaviour
{
    [SerializeField] Transform contentMain;
    public UICustomSlot slotPrefab;
    public CustomMainType customMainType;
    public List<UICustomSlot> uICustomSlots;
    public UICustomSlot currentSelected;

    public Sprite bgCommon, bgRare, bgEpic, bgLegend, bgNone;
    public ModelType modelType;
    int currentModelID = 0;
    Rarity currentModelRarity = Rarity.Common;
    UICustomSlot currentSlotBuy;
    public SkinLoadData modelLoadData;
    public void InitData(List<ModelAbleObj> data, ModelType modelType, Sprite iconOff) {
        this.modelType = modelType;
        SetCurrentVariable();
        for (int i = 0; i < data.Count; i++)
        {
            switch (data[i].rarity)
            {
                case Rarity.Common:
                    CreateSlot(currentModelID, currentModelRarity, Rarity.Common, data[i], iconOff);
                    break;
                case Rarity.Rare:
                    CreateSlot(currentModelID, currentModelRarity, Rarity.Rare, data[i], iconOff);
                    break;
                case Rarity.Epic:
                    CreateSlot(currentModelID, currentModelRarity, Rarity.Epic, data[i], iconOff);
                    break;
                case Rarity.Legend:
                    CreateSlot(currentModelID, currentModelRarity, Rarity.Legend, data[i], iconOff);
                    break;
            }
        }
    }
    void CreateSlot(int currentID, Rarity currentRarity, Rarity rarity, ModelAbleObj modelData, Sprite iconOff) {
        Sprite sprBG = null;
        switch (rarity)
        {
            case Rarity.Common:
                sprBG = bgCommon;
                break;
            case Rarity.Rare:
                sprBG = bgRare;
                break;
            case Rarity.Epic:
                sprBG = bgEpic;
                break;
            case Rarity.Legend:
                sprBG = bgLegend;
                break;
        }
        UICustomSlot newUICustomSlot = Instantiate(slotPrefab, contentMain.position, Quaternion.identity, contentMain);
        newUICustomSlot.myCustomMainGroup = this;
        Sprite iconChange = null;
        bool able = CheckModelData(modelData.modelID, rarity);
        if (able)
            iconChange = modelData.iconOn;
        else
            iconChange = iconOff;
        newUICustomSlot.InitData(modelData, sprBG, iconChange, modelData.iconOn, able);
        uICustomSlots.Add(newUICustomSlot);
        if (modelData.modelID == currentID && currentRarity == rarity)
            OnSelectSlot(newUICustomSlot);
    }
    public void OnOpen() {
        gameObject.SetActive(true);
        switch (modelType)
        {
            case ModelType.Shovel:
                modelLoadData.backPackRoot.gameObject.SetActive(false);
                modelLoadData.skinRoot.gameObject.SetActive(false);
                modelLoadData.shovelRoot.gameObject.SetActive(true);
                break;
            case ModelType.Skin:
                modelLoadData.backPackRoot.gameObject.SetActive(false);
                modelLoadData.skinRoot.gameObject.SetActive(true);
                modelLoadData.shovelRoot.gameObject.SetActive(false);
                break;
            case ModelType.BackPack:
                modelLoadData.backPackRoot.gameObject.SetActive(true);
                modelLoadData.skinRoot.gameObject.SetActive(false);
                modelLoadData.shovelRoot.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
    public void OnClose() { gameObject.SetActive(false); }
    public void OnSelectSlot(UICustomSlot slot) {
        if (currentSelected == slot)
            return;
        currentSelected = slot;
        ResetSelect();
        currentSelected.OnSelect();
        CheckActivate(currentSelected.modelID, currentSelected.rarity);
        LoadModel();
    }
    void CheckActivate(int modelID, Rarity rarity) {
        int currentModelID = 0;
        Rarity currentRarity = Rarity.Common;
        bool checkAble = false;
        switch (modelType)
        {
            case ModelType.Shovel:
                currentModelID = ProfileManager.Instance.playerData.GetCurrentShovelID();
                currentRarity = ProfileManager.Instance.playerData.GetCurrentShovelRarity();
                checkAble = ProfileManager.Instance.playerData.CheckShovelID(modelID, rarity);
                break;
            case ModelType.Skin:
                currentModelID = ProfileManager.Instance.playerData.GetCurrentSkinID();
                currentRarity = ProfileManager.Instance.playerData.GetCurrentSkinRarity();
                checkAble = ProfileManager.Instance.playerData.CheckSkinID(modelID, rarity);
                break;
            case ModelType.BackPack:
                currentModelID = ProfileManager.Instance.playerData.GetCurrentBackPackID();
                currentRarity = ProfileManager.Instance.playerData.GetCurrentBackPackRarity();
                checkAble = ProfileManager.Instance.playerData.CheckBackPackID(modelID, rarity);
                break;
            default:
                break;
        }

        if ((currentModelID == modelID && currentRarity == rarity) || (!checkAble))
            EnventManager.TriggerEvent(EventName.DisableBtnSelect.ToString());
        else EnventManager.TriggerEvent(EventName.ActivateBtnSelect.ToString());
    }
    void LoadModel() {
        ModelAbleObj modelAbleObj;
        switch (modelType)
        {
            case ModelType.Shovel:
                modelAbleObj = ProfileManager.Instance.profileDataConfig.modelAbleObjData.GetModelShovel(currentSelected.modelID, currentSelected.rarity);
                modelLoadData.LoadShovelPreview(modelAbleObj);
                break;
            case ModelType.Skin:
                modelAbleObj = ProfileManager.Instance.profileDataConfig.modelAbleObjData.GetModelSkin(currentSelected.modelID, currentSelected.rarity);
                modelLoadData.LoadSkinPreview(modelAbleObj);
                //EnventManager.TriggerEvent(EventName.UpdateModelRootPreview.ToString());
                break;
            case ModelType.BackPack:
                modelAbleObj = ProfileManager.Instance.profileDataConfig.modelAbleObjData.GetModelBackPack(currentSelected.modelID, currentSelected.rarity);
                modelLoadData.LoadBackPackPreview(modelAbleObj);
                break;
            default:
                break;
        }
    }
    public void OnDeSelectSlot() {
        ResetSelect();
    }
    void ResetSelect() {
        foreach (UICustomSlot slot in uICustomSlots)
        {
            if (currentSelected != null && slot == currentSelected)
                continue;
            slot.OnDeSelect();
        }
    }
    void SetCurrentVariable()
    {
        switch (modelType)
        {
            case ModelType.Shovel:
                currentModelID = ProfileManager.Instance.playerData.GetCurrentShovelID();
                currentModelRarity = ProfileManager.Instance.playerData.GetCurrentShovelRarity();
                break;
            case ModelType.Skin:
                currentModelID = ProfileManager.Instance.playerData.GetCurrentSkinID();
                currentModelRarity = ProfileManager.Instance.playerData.GetCurrentSkinRarity();
                break;
            case ModelType.BackPack:
                currentModelID = ProfileManager.Instance.playerData.GetCurrentBackPackID();
                currentModelRarity = ProfileManager.Instance.playerData.GetCurrentBackPackRarity();
                break;
            default:
                break;
        }
    }
    public void ReloadData()
    {
        SetCurrentVariable();
        foreach (UICustomSlot slot in uICustomSlots)
            if (slot.modelID == currentModelID && currentModelRarity == slot.rarity)
            {
                OnSelectSlot(slot);
                slot.UnLock();
            }
    }
    bool CheckModelData(int modelID, Rarity rarity)
    {
        switch (modelType)
        {
            case ModelType.Shovel:
                return ProfileManager.Instance.playerData.CheckShovelID(modelID, rarity);
            case ModelType.Skin:
                return ProfileManager.Instance.playerData.CheckSkinID(modelID, rarity);
            case ModelType.BackPack:
                return ProfileManager.Instance.playerData.CheckBackPackID(modelID, rarity);
            default:
                break;
        }
        return false;
    }
    public void OnBuy(UICustomSlot slot) {
        currentSlotBuy = slot;
        BuyManager.Instance.CustomOutFitBuy(slot.modelID, slot.priceAmount, modelType, slot.rarity, BuySuccess, BuyFaild);
    }
    void BuySuccess() {
        OnSelectSlot(currentSlotBuy);
        //UICustomSlotData data = ProfileManager.Instance.profileDataConfig.customDataConfig.GetUICustomSlotData(modelType, currentSlotBuy.modelID, currentSlotBuy.rarity);
        currentSlotBuy.UnLock();
        UIManager.Instance.GetUIPanel(UIType.Custom).GetComponent<UICustomManager>().SelectModel();
        Debug.Log("Pls show Panel Conraturation");
    }
    void BuyFaild() {
        UIManager.Instance.ShowWarningPanel(WarningType.NotEnoughGem, currentSlotBuy.priceAmount);
        Debug.Log("Pls show Panel warning");
    }
}
