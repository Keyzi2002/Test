using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelRevival : UIPanel
{
    [SerializeField] Button btnRevival;
    [SerializeField] Button btnIgnore;
    [SerializeField] Image slideTimeWatch;
    [Range(1, 10)]
    [SerializeField] float timeWatch;
    float currentTime;
    public override void Awake()
    {
        uiType = UIType.RevivalPanel;
        base.Awake();
        InitData();
       
    }
    private void Start()
    {
        EnventManager.AddListener(EventName.WatchRevivalDone.ToString(), OnClose);
    }
    public override void OnClose()
    {
        base.OnClose();
        UIManager.Instance.CloseRevivalPanel();
    }
    private void Update()
    {
        if (currentTime >= 0)
        {
            slideTimeWatch.fillAmount = currentTime / timeWatch;
            currentTime -= Time.deltaTime;
        }
        else Ignore();
    }
    public override void InitData()
    {
        btnRevival.onClick.AddListener(Revival);
        btnIgnore.onClick.AddListener(Ignore);
    }
    public void SettingTime() { currentTime = timeWatch; }
    void Revival() 
    {
        GameManager.Instance.revivalPlayer();
    }

    void Ignore() {
        UIManager.Instance.ShowLosePanel();
        UIManager.Instance.CloseRevivalPanel();
    }
}
