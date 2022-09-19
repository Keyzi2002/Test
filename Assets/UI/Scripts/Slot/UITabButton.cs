using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UITabButton : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    public UnityEvent onSelect;
    public UnityEvent onDeselect;
    public UITabGroup tabGroup;
    public Image backGround;
    public Text buttonName;
    public CustomMainType customMainType;
    public void InitData(CustomMainType customMainTypeChange, string buttonNameChange, UITabGroup tabGroupChange) {
        buttonName.text = buttonNameChange;
        customMainType = customMainTypeChange;
        tabGroup = tabGroupChange;
        backGround.sprite = tabGroup.backGroundOff;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnSelect(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnDeselect();
    }
    public void OnSelect() {
        backGround.sprite = tabGroup.backGroundOn;
        if (onSelect != null)
            onSelect.Invoke();
    }
    public void OnDeSelect() {
        backGround.sprite = tabGroup.backGroundOff;
        if (onDeselect != null)
            onDeselect.Invoke();
    }
}
