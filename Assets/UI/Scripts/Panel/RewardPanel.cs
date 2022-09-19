using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RewardPanel : UIPanel
{
    [SerializeField] Image imgIcon;
    [SerializeField] Button btnGetReward;
    [SerializeField] Button btnGetRewardDouble;
    [SerializeField] Button btnClose;
    [SerializeField] Text txtRewardName;
    [SerializeField] Text txtRewardDescription;
    [SerializeField] GameObject panelNormal;
    [SerializeField] GameObject panelSkin;
    [SerializeField] ShowSkinPanel showSkinPanel;
    UnityAction getRewardDone;
    public Item itemReward;
    bool doubleIt;
    public override void Awake()
    {
        uiType = UIType.RewardPanel;
        base.Awake();
    }
    private void Start()
    {
        btnGetReward.onClick.AddListener(GetReward);
        btnGetRewardDouble.onClick.AddListener(GetDouble);
        btnClose.onClick.AddListener(OnClose);
    }
    public void InitData(Item item, UnityAction action = null) {
        itemReward = item;
        ProfileManager.Instance.spinReward.GetReward(itemReward);
        txtRewardName.text = item.itemName;
        imgIcon.sprite = item.itemSprite;
        if (item.itemType == ItemType.Skin)
        {
            ModelItem modelItem = item as ModelItem;
            ShowPanel(modelItem.modelType == ModelType.Skin, modelItem);
            txtRewardDescription.text = "x1";
            btnGetRewardDouble.gameObject.SetActive(false);
            return;
        }
        ShowPanel(false);
        txtRewardDescription.text = "x" + item.amount.ToString();
        btnGetRewardDouble.gameObject.SetActive(true);
        getRewardDone = action;
        btnGetRewardDouble.gameObject.SetActive(true);
    }
    void ShowPanel(bool skinPanel, ModelItem modelItem = null) {
        panelNormal.SetActive(!skinPanel);
        panelSkin.SetActive(skinPanel);
        if (skinPanel)
            showSkinPanel.InitData(modelItem);
    }
    void GetReward() {
        OnClose();
    }
    void GetDouble() {
        doubleIt = true;
        btnGetRewardDouble.gameObject.SetActive(false);
        GetReward();
    }
    public override void OnClose()
    {
        doubleIt = false;
        if (getRewardDone != null)
        {
            getRewardDone();
            getRewardDone = null;
        }
        UIManager.Instance.CloseRewardPanel();
    }
}
