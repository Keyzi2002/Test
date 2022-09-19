using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public UIType uiType;
    public bool registerOnUI = true;
    public virtual void Awake() {
        if (registerOnUI) UIManager.Instance.RegisterPanel(uiType, gameObject);
    }
    public virtual void OnClose() { }
    public virtual void InitData() { }
}
