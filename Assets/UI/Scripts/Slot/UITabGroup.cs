using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabGroup : MonoBehaviour
{
    public List<UITabButton> tabButtons;
    UITabButton currentSelected;
    public Sprite backGroundOn, backGroundOff;
    public Transform tabGroupMain;
    public UITabButton tabPref;
    UICustomManager uiCustomManager;
    public void InitData() {
        uiCustomManager = UIManager.Instance.GetUIPanel(UIType.Custom).GetComponent<UICustomManager>();
        //OutFit
        UITabButton tabSkin = Instantiate(tabPref, tabGroupMain.position, Quaternion.identity, tabGroupMain);
        tabSkin.InitData(CustomMainType.Skin, "Skin", this);
        UITabButton tabShovel = Instantiate(tabPref, tabGroupMain.position, Quaternion.identity, tabGroupMain);
        tabShovel.InitData(CustomMainType.Shovel, "Shovel", this);
        UITabButton tabBag = Instantiate(tabPref, tabGroupMain.position, Quaternion.identity, tabGroupMain);
        tabBag.InitData(CustomMainType.Bag, "Bag", this);

        tabButtons.Add(tabSkin);
        tabButtons.Add(tabShovel);
        tabButtons.Add(tabBag);
    }
    public void OnSelect(UITabButton button) {
        if (button == currentSelected)
            return;
        currentSelected = button;
        ResetButton();
        currentSelected.OnSelect();
        uiCustomManager.ChangeMainGroup(currentSelected.customMainType);
    }
    public void OnDeselect() { ResetButton(); }
    void ResetButton() {
        for (int i = 0; i < tabButtons.Count; i++)
        {
            if (tabButtons[i] == currentSelected && currentSelected != null)
                continue;
            tabButtons[i].OnDeSelect();
        }
    }
    public UITabButton GetTabButton(CustomMainType type) {
        foreach (UITabButton button in tabButtons)
        {
            if (button.customMainType == type)
                return button;
        }
        return null;
    }
}
