using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIManager : GenerticSingleton<UIManager>
{
    Dictionary<UIType, GameObject> listPanel = new Dictionary<UIType, GameObject>();
    public bool popUpOnScene = false;
    public Transform parentTransform;
    private void Start()
    {
        parentTransform = GetComponent<Transform>();
        ShowStartPanel();
    }

    //int countTest;
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    ShowSpiralPanel();
        //    countTest = 0;
        //}
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    UIManager.Instance.ShowSpiralPanel();
        //}
        //if (Input.GetKey(KeyCode.B))
        //{
        //    countTest += 1;
        //    GetUIPanel(UIType.SpiralCountPanel).GetComponent<SpiralCountPanel>().CountChange(countTest);
        //}
        //if (Input.GetKeyUp(KeyCode.B))
        //{
        //    GetUIPanel(UIType.SpiralCountPanel).GetComponent<SpiralCountPanel>().EndCount();
        //}
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    ShowRevivalPanel();
        //}
        if (Input.GetKeyDown(KeyCode.T))
        {
            ShowMapPanel();
        }
    }
    public void RegisterPanel(UIType type, GameObject obj)
    {
        GameObject go = null;
        if (!listPanel.TryGetValue(type, out go))
        {
            //Debug.Log("RegisterPanel " + type.ToString());
            listPanel.Add(type, obj);
        }
        obj.SetActive(false);
    }
    public void ShowStartPanel() {
        popUpOnScene = true;
        GetUIPanel(UIType.StartGamePanel).SetActive(true);
    }
    public void CloseStartPanel()
    {
        popUpOnScene = false;
        GetUIPanel(UIType.StartGamePanel).SetActive(false);
    }
    public void ShowRewardPanel(Item itemShow, UnityAction actionDone = null)
    {
        popUpOnScene = true;
        GameObject rewardPanelObj = GetUIPanel(UIType.RewardPanel);
        rewardPanelObj.SetActive(true);
        RewardPanel rewardPanel = rewardPanelObj.GetComponent<RewardPanel>();
        rewardPanel.InitData(itemShow, actionDone);
        rewardPanelObj.transform.SetAsLastSibling();
    }
    public void CloseRewardPanel()
    {
        popUpOnScene = false;
        GetUIPanel(UIType.RewardPanel).SetActive(false);
    }
    public void ShowRevivalPanel()
    {
        popUpOnScene = true;
        GameObject revivalPanel = GetUIPanel(UIType.RevivalPanel);
        revivalPanel.SetActive(true);
        PanelRevival panel = revivalPanel.GetComponent<PanelRevival>();
        panel.SettingTime();
    }
    public void CloseRevivalPanel()
    {
        popUpOnScene = false;
        GetUIPanel(UIType.RevivalPanel).SetActive(false);
    }
    public void ShowLosePanel()
    {
        popUpOnScene = true;
        GetUIPanel(UIType.LoseGamePanel).SetActive(true);
    }
    public void CloseLosePanel()
    {
        popUpOnScene = false;
        GetUIPanel(UIType.LoseGamePanel).SetActive(false);
    }
    public void ShowMapPanel()
    {
        popUpOnScene = true;
        SelectMapPanel uIMapSlot = GetUIPanel(UIType.SelectMapPanel).GetComponent<SelectMapPanel>();
        uIMapSlot.gameObject.SetActive(true);
        if (uIMapSlot.initDone) uIMapSlot.ReloadData();
    }
    public void CloseMapPanel()
    {
        popUpOnScene = false;
        GetUIPanel(UIType.SelectMapPanel).SetActive(false);
    }
    public void ShowOpenChessPanel()
    {
        popUpOnScene = true;
        GetUIPanel(UIType.OpenChessPanel).SetActive(true);
    }
    public void CloseOpenChessPanel()
    {
        popUpOnScene = false;
        GetUIPanel(UIType.OpenChessPanel).SetActive(false);
    }
    public void ShowWinGamePanel()
    {
        popUpOnScene = true;
        GameObject panel = GetUIPanel(UIType.WinGamePanel);
        panel.SetActive(true);
        PanelWinGame panelWinGame = panel.GetComponent<PanelWinGame>();
        panelWinGame.SetUpToShow();
    }
    public void CloseWinGamePanel()
    {
        popUpOnScene = false;
        GetUIPanel(UIType.WinGamePanel).SetActive(false);
    }
    public void ShowShopPanel()
    {
        //popUpOnScene = true;
        //GetUIPanel(UIType.Shop).SetActive(true);
    }
    public void CloseShopPanel()
    {
        popUpOnScene = false;
        GetUIPanel(UIType.Shop).SetActive(false);
    }
    public void ShowCustomPanel()
    {
        popUpOnScene = true;
        GameObject uiCustomPanel = GetUIPanel(UIType.Custom);
        uiCustomPanel.SetActive(true);
        UICustomManager customManager = uiCustomPanel.GetComponent<UICustomManager>();
        if (customManager.initDataDone)
            customManager.ReloadData();
    }
    public void CloseCustomPanel()
    {
        popUpOnScene = false;
        GetUIPanel(UIType.Custom).SetActive(false);
    }
    public void ShowPraisePanel()
    {
        popUpOnScene = true;
        GameObject praiseObject = GetUIPanel(UIType.Praise);
        PanelPraise panelPraise = praiseObject.GetComponent<PanelPraise>();
        praiseObject.SetActive(true);
        panelPraise.ShowPraise(PraiseID.Amazing);
    }
    public void ClosePraisePanel()
    {
        popUpOnScene = false;
        GetUIPanel(UIType.Praise).SetActive(false);
    }
    public void ShowSettingPanel() {
        popUpOnScene = true;
        GetUIPanel(UIType.SettingPanel).SetActive(true);
    }
    public void CloseSettingPanel() {
        popUpOnScene = false;
        GetUIPanel(UIType.SettingPanel).SetActive(false);
    }
    public void ShowCompleteMissionPanel() {
        popUpOnScene = true;
        GameObject panel = GetUIPanel(UIType.MissionComplete);
        panel.SetActive(true);
        MissionCompletePanel missionCompletePanel = panel.GetComponent<MissionCompletePanel>();
        missionCompletePanel.StartAnim();
    }
    public void CloseCompleteMissionPanel() {
        popUpOnScene = false;
        GetUIPanel(UIType.MissionComplete).SetActive(false);
    }
    public void ShowSpiralPanel() {
        popUpOnScene = true;
        GetUIPanel(UIType.SpiralCountPanel).SetActive(true);
        EnventManager.TriggerEvent(EventName.SpiralStartCount.ToString());
    }
    public void CloseSpiralPanel() {
        popUpOnScene = false;
        GetUIPanel(UIType.SpiralCountPanel).SetActive(false);
    }
    public void ShowSpinWheelPanel()
    {
        if (ProfileManager.Instance.playerData.CheckCanSpinWheel() != SpinWheelState.CantSpin)
        {
            popUpOnScene = true;
            GameObject panel = GetUIPanel(UIType.SpinWheelPanel);
            panel.SetActive(true);
            WheelPanel wheelPanel = panel.GetComponent<WheelPanel>();
            wheelPanel.Reset();
        }
        else {
            ShowWarningPanel(WarningType.SpinWheelInCoolDown);
        }
    }
    public void CloseSpinWheelPanel()
    {
        popUpOnScene = false;
        GetUIPanel(UIType.SpinWheelPanel).SetActive(false);
    }
    public void ShowWarningPanel(WarningType warningType, int amount = 0) {
        popUpOnScene = true;
        GameObject panel = GetUIPanel(UIType.WarningPanel);
        panel.SetActive(true);
        WarningPopUpPanel warningPopUpPanel = panel.GetComponent<WarningPopUpPanel>();
        warningPopUpPanel.SettingPanel(warningType, amount);
        panel.transform.SetAsLastSibling();
    }
    public void CloseWarningPanel() {
        popUpOnScene = false;
        GetUIPanel(UIType.WarningPanel).SetActive(false);
    }

    public GameObject GetUIPanel(UIType panelType) {
        GameObject panelReturn = null;
        if (!listPanel.TryGetValue(panelType, out panelReturn))
        {
            Resources.LoadAll("UI/");
            switch (panelType)
            {
                case UIType.MissionComplete:
                    panelReturn = Instantiate(Resources.FindObjectsOfTypeAll<MissionCompletePanel>()[0].gameObject, parentTransform);
                    break;
                case UIType.LoadGame:
                    //panelReturn = Instantiate(Resources.FindObjectsOfTypeAll<LoadGame>()[0].gameObject, mainCanvas);
                    break;
                case UIType.Custom:
                    panelReturn = Instantiate(Resources.FindObjectsOfTypeAll<UICustomManager>()[0].gameObject, parentTransform);
                    break;
                case UIType.Shop:
                    panelReturn = Instantiate(Resources.FindObjectsOfTypeAll<UIShopManager>()[0].gameObject, parentTransform);
                    break;
                case UIType.WatchReward:
                    //panelReturn = Instantiate(Resources.FindObjectsOfTypeAll<WatchReward>()[0].gameObject, mainCanvas);
                    break;
                case UIType.Praise:
                    panelReturn = Instantiate(Resources.FindObjectsOfTypeAll<PanelPraise>()[0].gameObject, parentTransform);
                    break;
                case UIType.SettingPanel:
                    panelReturn = Instantiate(Resources.FindObjectsOfTypeAll<SettingPanel>()[0].gameObject, parentTransform);
                    break;
                case UIType.SpiralCountPanel:
                    panelReturn = Instantiate(Resources.FindObjectsOfTypeAll<SpiralCountPanel>()[0].gameObject, parentTransform);
                    break;
                case UIType.StartGamePanel:
                    panelReturn = Instantiate(Resources.FindObjectsOfTypeAll<PanelStartGame>()[0].gameObject, parentTransform);
                    break;
                case UIType.LoseGamePanel:
                    panelReturn = Instantiate(Resources.FindObjectsOfTypeAll<LoseGamePanel>()[0].gameObject, parentTransform);
                    break;
                case UIType.RevivalPanel:
                    panelReturn = Instantiate(Resources.FindObjectsOfTypeAll<PanelRevival>()[0].gameObject, parentTransform);
                    break;
                case UIType.OpenChessPanel:
                    panelReturn = Instantiate(Resources.FindObjectsOfTypeAll<PanelOpenChess>()[0].gameObject, parentTransform);
                    break;
                case UIType.WinGamePanel:
                    panelReturn = Instantiate(Resources.FindObjectsOfTypeAll<PanelWinGame>()[0].gameObject, parentTransform);
                    break;
                case UIType.SelectMapPanel:
                    panelReturn = Instantiate(Resources.FindObjectsOfTypeAll<SelectMapPanel>()[0].gameObject, parentTransform);
                    break;
                case UIType.RewardPanel:
                    panelReturn = Instantiate(Resources.FindObjectsOfTypeAll<RewardPanel>()[0].gameObject, parentTransform);
                    break;
                case UIType.SpinWheelPanel:
                    panelReturn = Instantiate(Resources.FindObjectsOfTypeAll<WheelPanel>()[0].gameObject, parentTransform);
                    break;
                case UIType.WarningPanel:
                    panelReturn = Instantiate(Resources.FindObjectsOfTypeAll<WarningPopUpPanel>()[0].gameObject, parentTransform);
                    break;
                default:
                    break;
            }
            if (panelReturn) panelReturn.SetActive(true);
            return panelReturn;
        }
        return listPanel[panelType];
    }
}
