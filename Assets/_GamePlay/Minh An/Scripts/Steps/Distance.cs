using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Distance : MonoBehaviour
{
    public Transform myTransform;
    public Action actionSetColor;
    [SerializeField] private NameColor nameColor = NameColor.None;
    [SerializeField] private ColorLerp colorLerp;
    [SerializeField] private float SpeedLoad;
    [SerializeField] private int ScoreBuild = 10;
    
    private void Awake()
    {
        OnInIt();
    }
    private void OnInIt()
    {
        if(myTransform == null) { myTransform = this.transform; }
        myTransform.localScale = Vector3.one;
    }
    public void SetNameColor(NameColor nameColor)
    {
        this.nameColor = nameColor;
        actionSetColor?.Invoke();
        if(nameColor != NameColor.None)
        {
            EnventManager.TriggerEvent(EventName.DistanceSetNewColor.ToString());
        }
       
    }
    public NameColor GetNameColor()
    {
        return nameColor;
    }
    public ColorLerp GetColorLerp()
    {
        return colorLerp;
    }
    public int GetScoreBuild()
    {
        return ScoreBuild;
    }
}
