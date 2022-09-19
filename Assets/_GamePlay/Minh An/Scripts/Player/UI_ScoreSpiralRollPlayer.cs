using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScoreSpiralRollPlayer : UI_Canvas
{
    public Transform myTranform;
    [SerializeField] private Text txtScore;
    [SerializeField] private Image imgIcon;
    [Header("Sprites")]
    [SerializeField] private Sprite sp_IconYellow;
    [SerializeField] private Sprite sp_IconGreen;
    [SerializeField] private Sprite sp_IconBlue;
    [SerializeField] private Sprite sp_IconPink;
    [SerializeField] private Sprite sp_IconOrange;
    [SerializeField] private Sprite sp_IconViolet;

    private Transform cameraMainTransForm;

    private void Awake()
    {
        if (myTranform == null) { myTranform = this.transform; }
        if (cameraMainTransForm == null) { cameraMainTransForm = Camera.main.transform; }

    }
    private void Update()
    {
        myTranform.LookAt(myTranform.position + cameraMainTransForm.rotation * Vector3.forward /*+ Vector3.forward*/, cameraMainTransForm.rotation * Vector3.up);
    }
    public void LoadTextScoreUI(int score)
    {
        txtScore.text = "+" + score.ToString();
    }
    public void LoadColorIconThisSUI(NameColor nameColorTaget)
    {

        switch (nameColorTaget)
        {
            case NameColor.Yeallow:
                imgIcon.sprite = sp_IconYellow;
                break;
            case NameColor.Pink:
                imgIcon.sprite = sp_IconPink;
                break;
            case NameColor.Green:
                imgIcon.sprite = sp_IconGreen;
                break;
            case NameColor.Violet:
                imgIcon.sprite = sp_IconViolet;
                break;
            case NameColor.Orange:
                imgIcon.sprite = sp_IconOrange;
                break;
            case NameColor.Blue:
                imgIcon.sprite = sp_IconBlue;
                break;
        }
    }
}
