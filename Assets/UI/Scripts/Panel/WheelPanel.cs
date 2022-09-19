using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelPanel : UIPanel
{
    [SerializeField] Button btnStartWheel;
    [SerializeField] Button btnWatchVideo;
    [SerializeField] Button btnClose;
    [SerializeField] GameObject prebWheelSpinSlot;
    public Transform spinUI;
    float timeSpin;
    float currentAngle;
    bool spinActivate;
    float spinSpeed;
    float spinSpeedSetting;
    AnimationCurve speedCurve;
    [SerializeField] float timeSpinSetting;
    public List<WheelUISlot> wheelSlots;
    WheelUISlot wheelReward;
    bool isSpin;
    public override void Awake()
    {
        uiType = UIType.SpinWheelPanel;
        base.Awake();
        btnStartWheel.onClick.AddListener(Spin);
        btnWatchVideo.onClick.AddListener(WatchVideo);
        btnClose.onClick.AddListener(OnClose);
        btnWatchVideo.gameObject.SetActive(false);
        Reset();
    }
    public void Reset()
    {
        isSpin = false;
        InitData();
    }
    public override void InitData()
    {
        wheelSlots.Clear();
        for (int i = 0; i < spinUI.childCount; i++)
            Destroy(spinUI.GetChild(i).gameObject);
        spinUI.transform.eulerAngles = new Vector3(0, 0, 0);
        speedCurve = AnimationCurveManager.Instance.speedSpin;
        GameManager.Instance.wheelManager.StartRandom(InstanceNewSlot);
        SpinWheelState state = ProfileManager.Instance.playerData.GetWheelState();
        switch (state)
        {
            case SpinWheelState.FreeSpin:
                isSpin = false;
                btnWatchVideo.gameObject.SetActive(false);
                break;
            case SpinWheelState.WatchSpin:
                isSpin = true;
                ShowBtnWatchVideo();
                break;
            default:
                break;
        }
    }
    public void InstanceNewSlot() {
        float angleSlot = 0;
        for (int i = 0; i < 8; i++)
        {
            GameObject newSlot = Instantiate(prebWheelSpinSlot, spinUI);
            newSlot.transform.eulerAngles = new Vector3(0, 0, angleSlot);
            angleSlot -= 45;
            Rarity slotRarity = GameManager.Instance.wheelManager.GetWheelPiece().rarity;
            Item rewardItem = ProfileManager.Instance.spinReward.GetRandomItem(slotRarity);
            WheelUISlot wheelUISlot = newSlot.GetComponent<WheelUISlot>();
            wheelUISlot.SetItem(rewardItem);
            wheelUISlot.InitData();
            if (i == 0)
                wheelUISlot.SetMaxMinAngle(-45);
            else wheelUISlot.SetMaxMinAngle(wheelSlots[i - 1].maxAngle - 22.5f);
            wheelSlots.Add(wheelUISlot);
        }
        wheelSlots[0].SetFirstMinAngle(337.5f);
    }
    void Spin() {
        if (!isSpin) EnventManager.TriggerEvent(EventName.SaveTimeWatchVideo.ToString());
        isSpin = true;
        spinActivate = true;
        timeSpin = 0;
        currentAngle = spinUI.transform.eulerAngles.z;
        timeSpinSetting = Random.Range(3, 5);
        spinSpeedSetting = Random.Range(1000, 2000);
        btnWatchVideo.gameObject.SetActive(false);
    }
    private void Update()
    {
        btnStartWheel.interactable = !isSpin;
        if (spinActivate)
        {
            if (timeSpin <= speedCurve.keys[speedCurve.length - 1].time * timeSpinSetting)
            {
                spinSpeed = spinSpeedSetting * speedCurve.Evaluate(timeSpin / timeSpinSetting);
                timeSpin += Time.deltaTime;
            }
            else
            {
                spinActivate = false;
                spinSpeed = 0;
                if (CheckReward(currentAngle % 360f))
                {
                    wheelReward.BingoAnim();
                    StartCoroutine(WaitingAnim());
                }
                else
                {
                    CountinuteRotage();
                    return;
                }
            }
            currentAngle += Time.deltaTime * spinSpeed;
            spinUI.eulerAngles = new Vector3(0, 0, currentAngle);
        }
    }
    IEnumerator WaitingAnim() {
        yield return new WaitForSeconds(2f);
        ShowPanelReward();
    }
    void ShowBtnWatchVideo() {btnWatchVideo.gameObject.SetActive(true);}
    bool CheckReward(float angle) {
        wheelReward = null;
        if (angle > wheelSlots[0].minAngle || angle < wheelSlots[0].maxAngle)
        {
            wheelReward = wheelSlots[0];
            return true;
        }
        for (int i = 1; i < wheelSlots.Count; i++)
        {
            if (angle > wheelSlots[i].minAngle && angle < wheelSlots[i].maxAngle)
            {
                wheelReward = wheelSlots[i];
                return true;
            }
        }
        return false;
    }
    void CountinuteRotage() {
        spinActivate = true;
        timeSpin = 0;
        currentAngle = spinUI.transform.eulerAngles.z;
        timeSpinSetting = .25f;
        spinSpeedSetting = 50f;
    }
    void ShowPanelReward() {
        Item itemReward = wheelReward.GetItem();
        UIManager.Instance.ShowRewardPanel(itemReward);
        btnWatchVideo.gameObject.SetActive(true);
    }
    void WatchVideo() {
        EnventManager.TriggerEvent(EventName.IncreaseCountSpinWatchVideo.ToString());
        btnWatchVideo.gameObject.SetActive(false);
        InitData();
        Spin();
    }
    public override void OnClose()
    {
        ResetValue();
        UIManager.Instance.CloseSpinWheelPanel();
    }
    void ResetValue() {
        InitData();
    }
}
