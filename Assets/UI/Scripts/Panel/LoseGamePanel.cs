using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoseGamePanel : UIPanel
{
    [SerializeField] Button btnDoubleCoin;
   // [SerializeField] Button btnGetGem;
    [SerializeField] Button btnDontCare;
    public override void Awake()
    {
        uiType = UIType.LoseGamePanel;
        base.Awake();
        InitData();
    }
    public override void InitData()
    {
        //btnDoubleCoin.onClick.AddListener(DoubleCoin);
        //btnIgnoreLevel.onClick.AddListener(IgnoreLevel);
        btnDoubleCoin.onClick.AddListener(WatchToGetGem);
        btnDontCare.onClick.AddListener(DontCare);
    }
    //void DoubleCoin() { }
    //void IgnoreLevel() { }
    void WatchToGetGem() {
        ProfileManager.Instance.playerData.AddGem(100);
    }
    void DontCare() {
        UIManager.Instance.CloseLosePanel();
        GameManager.Instance.ReLoadGame();
    }
}
