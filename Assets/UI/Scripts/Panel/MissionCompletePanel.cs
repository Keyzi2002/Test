using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionCompletePanel : UIPanel
{
    [SerializeField] Button btnClose;
    int missionDone, missionAmount, currentLevel;
    public Sprite sprMissionDone, sprMissionOnProgress, sprCurrentLevel, sprNextLevel;
    //public Image imageMission; 
    [Range(0, 2)]
    public float persentLoadSpeed;
    [SerializeField] Transform imageMissionParent;
    [SerializeField] Image persentSkinLoad, imgBlack;
    [SerializeField] Text persentText;
    float persentSkinStart, persentSkinEnd;
    bool animDone = true;
    float skinAnimTime = 0f;
    AnimationCurve skinPersentCurve;
    AnimationCurveManager animCurveManager;
    public override void Awake()
    {
        uiType = UIType.MissionComplete;
        base.Awake();
        btnClose.onClick.AddListener(ReloadGame);
        btnClose.onClick.AddListener(OnClose);
    }
    private void ReloadGame()
    {
        GameManager.Instance.ReLoadGame();
    }
    /// <summary>
    /// show missionbar
    /// </summary>
    //public void InitMissionBar() {
    //    //missionAmount = 
    //    //missionDone = 
    //    //currentLevel = 
    //    Transform[] gameObjects = imageMissionParent.GetComponentsInChildren<Transform>();
    //    for (int i = 1; i < gameObjects.Length; i++)
    //        Destroy(gameObjects[i].gameObject);
    //    Image currentLevelImage = Instantiate(imageMission, imageMissionParent);
    //    currentLevelImage.sprite = sprCurrentLevel;
    //    for (int i = 1; i < missionAmount - 1; i++)
    //    {
    //        Image newImageMission = Instantiate(imageMission, imageMissionParent);

    //        if (i <= missionDone)
    //            newImageMission.sprite = sprMissionDone;
    //        else newImageMission.sprite = sprMissionOnProgress;
    //    }
    //    Image nextLevelImage = Instantiate(imageMission, imageMissionParent);
    //    nextLevelImage.sprite = sprNextLevel;
    //}
    private void Update()
    {
        if (!animDone)
        {
            if (skinAnimTime <= skinPersentCurve.keys[skinPersentCurve.length - 1].time)
            {
                float persent = Mathf.Lerp(0, persentSkinEnd, skinPersentCurve.Evaluate(skinAnimTime));
                persentSkinLoad.fillAmount = persent;
                persentText.text = (Mathf.RoundToInt(persent * 100)).ToString()+"%";
                skinAnimTime += Time.deltaTime * persentLoadSpeed;
            }
            else { animDone = true; }
        }
    }
    public void StartAnim() {
        int currentLevel = ProfileManager.Instance.levelData.GetLevel() + 1;
        LevelGetSkin levelGetSkin = ProfileManager.Instance.modelLevelData.GetLevelMinDistance(currentLevel);
        ModelAbleObj modelObj = levelGetSkin.modelData;
        if (levelGetSkin.levelGet == currentLevel)
            switch (modelObj.modelType)
            {
                case ModelType.Shovel:
                    ProfileManager.Instance.playerData.AddShovel(modelObj.modelID, modelObj.rarity);
                    break;
                case ModelType.Skin:
                    ProfileManager.Instance.playerData.AddSkin(modelObj.modelID, modelObj.rarity);
                    break;
                case ModelType.BackPack:
                    ProfileManager.Instance.playerData.AddBackPack(modelObj.modelID, modelObj.rarity);
                    break;
                default:
                    break;
            }
        imgBlack.sprite = modelObj.iconOn;
        persentSkinLoad.sprite = modelObj.iconOn;
        int targetLevel = levelGetSkin.levelGet;
        persentSkinStart = 0f;
        persentSkinLoad.fillAmount = 0f;
        persentSkinEnd = (float)currentLevel / (float)targetLevel;
        animDone = false;
        skinAnimTime = 0f;
        animCurveManager = AnimationCurveManager.Instance;
        skinPersentCurve = animCurveManager.skinPersentCurve;
        missionAmount = targetLevel;
        missionDone = currentLevel;
        //InitMissionBar();
    }
    public override void OnClose()
    {
        UIManager.Instance.CloseCompleteMissionPanel();
    }
}
