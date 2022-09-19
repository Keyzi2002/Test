using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessSlot : MonoBehaviour
{
    [SerializeField] Sprite sprChessNormal;
    [SerializeField] Sprite sprChessOpen;
    [SerializeField] Image icon;
    [SerializeField] Image iconItem;
    [SerializeField] Button btnChess;
    public bool opened;
    PanelOpenChess panelOpenChess;
    private void Start()
    {
        panelOpenChess = UIManager.Instance.GetUIPanel(UIType.OpenChessPanel).GetComponent<PanelOpenChess>();
        btnChess.onClick.AddListener(Open);
    }
    private void Update()
    {
        btnChess.interactable = opened;
    }
    public void ResetSlot() { 
        opened = false;
        icon.sprite = sprChessNormal;
        iconItem.gameObject.SetActive(false);
    }
    public void ChangeIcon(Sprite sprChange) {
        iconItem.gameObject.SetActive(true);
        iconItem.sprite = sprChange;
        icon.sprite = sprChessOpen;
    }
    void Open() { 
        if (!opened) 
            panelOpenChess.OpenChess(this); 
    }
}
