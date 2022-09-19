using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UICustomContentData
{
    public List<UICustomSlotData> slotCommond;
    public List<UICustomSlotData> slotRare;
    public List<UICustomSlotData> slotEpic;
    public List<UICustomSlotData> slotLegend;
    public Sprite sprIconOff;
    public ModelType modelType;
    public UICustomSlotData GetCustomData(int modelID, Rarity rarity) {
        switch (rarity)
        {
            case Rarity.Common:
                foreach (UICustomSlotData item in slotCommond)
                {
                    if (modelID == item.modelID)
                        return item;
                }
                break;
            case Rarity.Rare:
                foreach (UICustomSlotData item in slotRare)
                {
                    if (modelID == item.modelID)
                        return item;
                }
                break;
            case Rarity.Epic:
                foreach (UICustomSlotData item in slotEpic)
                {
                    if (modelID == item.modelID)
                        return item;
                }
                break;
            case Rarity.Legend:
                foreach (UICustomSlotData item in slotLegend)
                {
                    if (modelID == item.modelID)
                        return item;
                }
                break;
            default:
                break;
        }
        return null;
    }
}
