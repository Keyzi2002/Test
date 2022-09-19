using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Notion : UI_Canvas
{
    [SerializeField] private Text txt_Noti;

    public void LoadTextNoti(string str_noti)
    {
        txt_Noti.text = str_noti;
    }
}
