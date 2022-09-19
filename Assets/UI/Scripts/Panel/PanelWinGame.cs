using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelWinGame : UIPanel
{
    [SerializeField] Button btnWatchX2;
    [SerializeField] Button btnEnough;
    [SerializeField] Text txtCoinEarned;
    [SerializeField] Text txtCoinAffterWatch;
    [SerializeField] Text txtCurrentMission;
    public GameObject prebPElement = null;
    public Sprite sprDone, sprNotDone;
    [SerializeField]
    List<Image> stars;
    [SerializeField]
    List<Image> starBacklight;
    [SerializeField]
    float starRaiseSpeed = 1f;
    int starEarned = 2;
    float currentShowStar = 0f;
    int count;
    int gemEarn;
    public override void Awake()
    {
        uiType = UIType.WinGamePanel;
        base.Awake();
        InitData();
    }
    public override void InitData()
    {
        btnWatchX2.onClick.AddListener(WatchDoubleCoin);
        btnEnough.onClick.AddListener(OpenLoadSkin);
    }
    float timeWait;
    private void Update()
    {
        //if (timeWait < 5f)
        //    timeWait += Time.deltaTime;
        //else OpenLoadSkin();
        if (currentShowStar < starEarned)
        {
            stars[(int)currentShowStar].fillAmount += Time.deltaTime * starRaiseSpeed;
            starBacklight[(int)currentShowStar].fillAmount += Time.deltaTime * starRaiseSpeed;
            currentShowStar += Time.deltaTime * starRaiseSpeed;
        }
    }
    public void SetUpToShow() {
        //imgCurrentLevel.sprite = sprCLevel;
        //imgNextLevel.sprite = sprNLevel;
        //foreach (Transform child in imgProgressLevel.transform)
        //    GameObject.Destroy(child.gameObject);
        //while (count < missionTotal)
        //{
        //    GameObject newElement = Instantiate(prebPElement, imgProgressLevel.transform);
        //    ProgressElement pElement = newElement.GetComponent<ProgressElement>();
        //    pElement.icon.sprite = sprNotDone;
        //    if (count < currentMissionIndex) StartCoroutine(ChangeImage(pElement, sprDone));
        //    count++;
        //}
        //imgEvaluate.fillAmount = evaluate / 3;
        //if (evaluate == 3) imgKey.SetActive(true);
        //else imgKey.SetActive(false);
        int currentLevel = ProfileManager.Instance.levelData.GetLevel() - 1;
        gemEarn = /*ProfileManager.Instance.profileDataConfig.levelDataConfig.GetGemLevel(currentLevel)*/
            GameManager.GemReward_CompleteLevelDefault + GameManager.GemUpInOneLevel * currentLevel;
        ResetStar(3);
        timeWait = 0f;
        txtCoinEarned.text = gemEarn.ToString() + "$";
        txtCoinAffterWatch.text = (gemEarn * 2).ToString() + "$";
    }
    //IEnumerator ChangeImage(ProgressElement element, Sprite spr) {
    //    yield return new WaitForSeconds(.25f * count);
    //    element.icon.sprite = spr;
    //}
    void OpenLoadSkin() {
        GetGem();
        UIManager.Instance.CloseWinGamePanel();
        UIManager.Instance.ShowCompleteMissionPanel();
    }
    void WatchDoubleCoin() {
        gemEarn *= 2;
        StartCoroutine(IE_Singtelon.IE_DelayAction(() => { OpenLoadSkin(); }, 2));
    }
    void GetGem()
    { 
        ProfileManager.Instance.playerData.AddGem(gemEarn);
        StartCoroutine(IE_Singtelon.IE_DelayAction(() => { OpenLoadSkin(); }, 2));
    }
    private void ResetStar(int evaluate)
    {
        currentShowStar = 0;
        starEarned = evaluate;
        for (int i = 0; i < stars.Count; i++)
        {
            stars[i].fillAmount = 0f;
            starBacklight[i].fillAmount = 0f;
        }
    }
}

