using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpenChess : UIPanel
{
    public List<ChessSlot> chessSlots = new List<ChessSlot>();
    public override void Awake()
    {
        uiType = UIType.OpenChessPanel;
        base.Awake();
    }
    public override void InitData()
    {
        for (int i = 0; i < chessSlots.Count; i++)
            chessSlots[i].ResetSlot();
    }
    public void OpenChess(ChessSlot chessSlot) {
        Debug.Log("Random Reward");
        chessSlot.ChangeIcon(null);
        chessSlot.opened = false;
    }
}
