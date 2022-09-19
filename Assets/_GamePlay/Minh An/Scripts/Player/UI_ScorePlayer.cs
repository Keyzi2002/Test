using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScorePlayer : UI_Canvas
{
    public Transform myTranform;
    [SerializeField] private Text text_Score;
    [SerializeField] private BagTerrainRoll bagTerrain;
    [SerializeField] private GameObject objFull;
    private Transform cameraMainTransForm;

    private void Awake()
    {
        if (myTranform == null) { myTranform = this.transform; }
        if (cameraMainTransForm == null) { cameraMainTransForm = Camera.main.transform; }
        SetTextScore(0);
    }
    void Start()
    {
        EnventManager.AddListener(EventName.UpdateTextScore.ToString(), LoadTextScore);

       // Close();
    }
    private void Update()
    {
        myTranform.LookAt(myTranform.position + cameraMainTransForm.rotation * Vector3.forward /*+ Vector3.forward*/, cameraMainTransForm.rotation * Vector3.up);
    }

    private void LoadTextScore()
    {
        if (bagTerrain == null)
        {
            bagTerrain = GetComponentInParent<Character>().GetBagTerrainRoll();
            UI_InCharacter uI_InCharacter = GetComponentInParent<UI_InCharacter>();
            bagTerrain?.SetActionBagFull(() => { uI_InCharacter.Open_UIFull(); });
            bagTerrain?.SetActionBagNotFull(() => { uI_InCharacter.Close_UIFull(); });
        }
        if(bagTerrain != null)
        {
            SetTextScore(bagTerrain.GetScoreTerrainRollInBag());
        }
       
    }
   
    private void SetTextScore(int score)
    {
        text_Score.text = score.ToString();
    }
    //public override void Open()
    //{
    //    objFull.SetActive(true);
    //}
    //public override void Close()
    //{
    //    objFull.SetActive(false);
    //}
}
