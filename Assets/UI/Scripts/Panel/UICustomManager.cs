using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICustomManager : UIPanel
{
    public override void Awake()
    {
        uiType = UIType.Custom;
        base.Awake();
    }
    [SerializeField] Transform outFitMain;
    [SerializeField] GameObject outFitTabGroup;
    [SerializeField] Button btnClose;
    [SerializeField] Button btnSelectModel;
    [SerializeField] RuntimeAnimatorController previewController;
    [SerializeField] Button btnActivateAnim;
    [SerializeField] Text gemText;
    public bool initDataDone = false;
    private void Start()
    {
        btnClose.onClick.AddListener(OnClose);
        btnSelectModel.onClick.AddListener(SelectModel);
        btnActivateAnim.onClick.AddListener(() => {
            EnventManager.TriggerEvent(EventName.ChangeAnimModel.ToString());
        });
        EnventManager.AddListener(EventName.DisableBtnSelect.ToString(), DisableBtnSelect);
        EnventManager.AddListener(EventName.ActivateBtnSelect.ToString(), ActivateBtnSelect);
        EnventManager.AddListener(EventName.UpdateModelRootPreview.ToString(), UpdateRootModel);
        EnventManager.AddListener(EventName.ChangeGem.ToString(), UpdateGemText);
        InitData();
        UpdateGemText();

        RectTransform rect = btnActivateAnim.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.rect.width);
    }
    public CustomMainGroup customMainPref;
    public List<CustomMainGroup> groupMains;
    public UITabGroup tabGroup;
    CustomMainGroup currentMainGroup;
    public SkinLoadData modelLoadData;
    public override void InitData()
    {
        modelLoadData.SetControllerPreview(previewController);
        ModelAbleObject modelData = ProfileManager.Instance.profileDataConfig.modelAbleObjData;

        CustomMainGroup skin = Instantiate(customMainPref, outFitMain.position, Quaternion.identity, outFitMain);
        skin.modelLoadData = modelLoadData;
        skin.InitData(modelData.modelSkin, ModelType.Skin, modelData.GetSpriteOff(ModelType.Skin));
        skin.customMainType = CustomMainType.Skin;
        skin.OnClose();

        CustomMainGroup shovel = Instantiate(customMainPref, outFitMain.position, Quaternion.identity, outFitMain);
        shovel.modelLoadData = modelLoadData;
        shovel.InitData(modelData.modelShovel, ModelType.Shovel, modelData.GetSpriteOff(ModelType.Shovel));
        shovel.customMainType = CustomMainType.Shovel;
        shovel.OnClose();

        CustomMainGroup bag = Instantiate(customMainPref, outFitMain.position, Quaternion.identity, outFitMain);
        bag.modelLoadData = modelLoadData;
        bag.InitData(modelData.modelBackPack, ModelType.BackPack, modelData.GetSpriteOff(ModelType.BackPack));
        bag.customMainType = CustomMainType.Bag;
        bag.OnClose();

        groupMains.Add(skin);
        groupMains.Add(shovel);
        groupMains.Add(bag);
        //TabGroup;
        tabGroup.InitData();
        tabGroup.OnSelect(tabGroup.GetTabButton(CustomMainType.Skin));
        initDataDone = true;
    }
    void UpdateGemText() {
        gemText.text = ProfileManager.Instance.playerData.GetGem().ToString();
    }
    void DisableBtnSelect() {
        btnSelectModel.gameObject.SetActive(false);
    }
    void ActivateBtnSelect() {
        btnSelectModel.gameObject.SetActive(true);
    }
    public CustomMainGroup GetMainGroup(CustomMainType mainType) {
        foreach (CustomMainGroup group in groupMains)
        {
            if (group.customMainType == mainType)
                return group;
        }
        return null;
    }
    public void ChangeMainGroup(CustomMainType mainType) {
        if (currentMainGroup != null) currentMainGroup.OnClose();
        currentMainGroup = GetMainGroup(mainType);
        currentMainGroup.OnOpen();
    }
    public void ReloadData() {
        foreach (CustomMainGroup group in groupMains)
            group.ReloadData();
    }
    public override void OnClose()
    {
        UIManager.Instance.CloseCustomPanel();
    }
    public void SelectModel()
    {
        switch (currentMainGroup.modelType)
        {
            case ModelType.Shovel:
                ProfileManager.Instance.playerData.SetCurrentShovelID(currentMainGroup.currentSelected.modelID, currentMainGroup.currentSelected.rarity);
                EnventManager.TriggerEvent(EventName.ChangeShovelOnPlayer.ToString());
                break;
            case ModelType.Skin:
                ProfileManager.Instance.playerData.SetCurrentSkinID(currentMainGroup.currentSelected.modelID, currentMainGroup.currentSelected.rarity);
                EnventManager.TriggerEvent(EventName.ChangeSkinOnPlayer.ToString());
                break;
            case ModelType.BackPack:
                ProfileManager.Instance.playerData.SetCurrentBackPackID(currentMainGroup.currentSelected.modelID, currentMainGroup.currentSelected.rarity);
                EnventManager.TriggerEvent(EventName.ChangeBackPackPlayer.ToString());
                break;
            default:
                break;
        }
        DisableBtnSelect();
    }
    public void UpdateRootModel()
    {
        ModelSkinRoot modelSkinRoot = GetComponentInChildren<ModelSkinRoot>();
        modelLoadData.backPackRoot = modelSkinRoot.bagPoint;
        //modelLoadData.shovelRoot = modelSkinRoot.handPoint;

        ///Reload model
        int currentID = 0;
        Rarity currentRarity = Rarity.Common;

        currentID = ProfileManager.Instance.playerData.GetCurrentBackPackID();
        currentRarity = ProfileManager.Instance.playerData.GetCurrentBackPackRarity();
        ModelAbleObj backPack = ProfileManager.Instance.profileDataConfig.modelAbleObjData.GetModelBackPack(currentID, currentRarity);
        modelLoadData.LoadBackPackPreview(backPack);

        //currentID = ProfileManager.Instance.playerData.GetCurrentShovelID();
        //currentRarity = ProfileManager.Instance.playerData.GetCurrentShovelRarity();
        //ModelAbleObj shovel = ProfileManager.Instance.profileDataConfig.modelAbleObjData.GetModelShovel(currentID, currentRarity);
        //modelLoadData.LoadShovelPreview(shovel);
    }
}
