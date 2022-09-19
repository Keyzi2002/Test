using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopManager : UIPanel
{
    public static UIShopManager instance;
    public Button closeButton;
    public List<UIShopSlot> uiShopSlots;
    public Transform shopMain;
    public UIShopSlot slotPrefab;
    public override void Awake()
    {
        instance = this;
        uiType = UIType.Shop;
        base.Awake();
        closeButton.onClick.AddListener(OnClose);
    }
    private void Start()
    {
        InitData();
    }
    public override void InitData() {
        UIShopData shopData = ProfileManager.Instance.profileDataConfig.shopDataConfig;
        foreach (OfferData offerData in shopData.offerDatas)
        {
            UIShopSlot newUIShopSlot = Instantiate(slotPrefab, shopMain.position, Quaternion.identity, shopMain);
            uiShopSlots.Add(newUIShopSlot);
            newUIShopSlot.InitData(offerData);
        }
    }
    public override void OnClose()
    {
        UIManager.Instance.CloseShopPanel();
    }
}
