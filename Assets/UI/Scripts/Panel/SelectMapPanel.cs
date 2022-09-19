using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMapPanel : UIPanel
{
    public override void Awake()
    {
        uiType = UIType.SelectMapPanel;
        base.Awake();
    }
    [SerializeField] GameObject mapSlot;
    [SerializeField] Transform parentRoot;
    [SerializeField] Button btnClose;
    [SerializeField] ScrollRect scroll;
    public List<UIMapSlot> mapSlots;
    [HideInInspector]public bool initDone;
    private void Start()
    {
        btnClose.onClick.AddListener(OnClose);
        InitData();
    }
    public override void InitData()
    {
        LevelSaveData levelSaveData = ProfileManager.Instance.levelData;
        LevelData levelDataConfig = ProfileManager.Instance.profileDataConfig.levelDataConfig;
        for (int i = 0; i < levelDataConfig.levels.Count; i++)
        {
            UIMapSlot newMapSlot = Instantiate(mapSlot, parentRoot).GetComponent<UIMapSlot>();
            //newMapSlot.InitData(levelDataConfig.GetSpriteLevel(i), playerData.GetStar(i), "Level " + (i + 1));
            //newMapSlot.InitData(levelDataConfig.GetSpriteLevel(i), levelSaveData.GetStar(i), levelDataConfig.levels[i].levelName);
            mapSlots.Add(newMapSlot);
        }
        initDone = true;
        scroll.verticalNormalizedPosition = 0;
    }
    public void ReloadData() {
        LevelSaveData levelSaveData = ProfileManager.Instance.levelData;
        LevelData levelDataConfig = ProfileManager.Instance.profileDataConfig.levelDataConfig;
        //for (int i = 0; i < mapSlots.Count; i++)
        //    mapSlots[i].InitData(levelDataConfig.GetSpriteLevel(i), levelSaveData.GetStar(i), "Level " + (i + 1));
        //scroll.verticalNormalizedPosition = 0;
    }
    public override void OnClose()
    {
        UIManager.Instance.CloseMapPanel();
    }
    public void OnSelectedLevel(int levelSelected) 
    {
        //int selectMission = 0;
        //for (int i = 0; i < levelSelected; i++)
        //{
        //    selectMission += ProfileManager.Instance.profileDataConfig.levelDataConfig.levels[i].missionObjectMaps.Count;
        //}
        //selectMission++;
        //Debug.Log("Player Choose map level: " + selectMission);
    }
}
