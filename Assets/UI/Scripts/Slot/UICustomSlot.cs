using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICustomSlot : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    public CustomMainGroup myCustomMainGroup;
    public int modelID;
    public UnityEvent onSelect, onDeselect;
    [SerializeField] Image icon;
    [SerializeField] Image iconBtn;
    [SerializeField] Image imgBackGround;
    [SerializeField] GameObject lockObj;
    [SerializeField] Text txtPrice;
    [SerializeField] Button btnBuy;
    [SerializeField] Image imgLight;
    public Sprite sprWatch, sprGem;
    public int priceAmount;
    public Rarity rarity;
    ModelWatched modelWatched;
    Sprite sprBG, sprOn;
    public void InitData(ModelAbleObj data, Sprite sprBG = null, Sprite iconChange = null, Sprite sprOn = null, bool able = true) {
        icon.sprite = iconChange;
        if (able)
            imgBackGround.sprite = sprBG;
        btnBuy.gameObject.SetActive(!able);
        lockObj.gameObject.SetActive(!able);
        modelID = data.modelID;
        this.sprBG = sprBG;
        this.sprOn = sprOn;
        txtPrice.text = data.priceAmount.ToString();
        priceAmount = data.priceAmount;
        rarity = data.rarity;
        switch (data.payment)
        {
            case Payment.Gem:
                btnBuy.onClick.AddListener(OnBuy);
                iconBtn.gameObject.SetActive(true);
                iconBtn.sprite = sprGem;
                break;
            case Payment.Watch:
                btnBuy.onClick.AddListener(OnWatch);
                iconBtn.gameObject.SetActive(true);
                iconBtn.sprite = sprWatch;
                OfferWatchedData offerWatchedData = ProfileManager.Instance.offerWatchedData;
                modelWatched = offerWatchedData.GetOfferWatched(modelID, rarity);
                InitDataWatch();
                break;
            case Payment.Offer:
                btnBuy.onClick.AddListener(OpenShop);
                iconBtn.gameObject.SetActive(false);
                txtPrice.text = "SHOP";
                break;
            case Payment.LevelGet:
                txtPrice.text = "LEVEL "+ priceAmount.ToString();
                iconBtn.gameObject.SetActive(false);
                break;
            case Payment.Spin:
                btnBuy.onClick.AddListener(OnSpin);
                iconBtn.gameObject.SetActive(false);
                txtPrice.text = "SPIN";
                break;
            default:
                break;
        }
    }
    void InitDataWatch() {
        txtPrice.text = modelWatched.watchedCount.ToString() + "/" + priceAmount;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        myCustomMainGroup.OnDeSelectSlot();
    }
    public void OnSelect() {
        imgLight.gameObject.SetActive(true);
        if (onSelect != null)
            onSelect.Invoke();
    }
    public void OnDeSelect() {
        imgLight.gameObject.SetActive(false);
        if (onDeselect != null)
            onDeselect.Invoke();
    }
    public void OnChoose() {
        //imgBackGround.sprite = bgChoose;
        imgLight.gameObject.SetActive(true);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        myCustomMainGroup.OnSelectSlot(this);
    }
    void OnBuy() {
        myCustomMainGroup.OnBuy(this);
    }
    void OnSpin() {
        UIManager.Instance.CloseCustomPanel();
        UIManager.Instance.ShowSpinWheelPanel();
    }
    void OnWatch() {
        OfferWatchedData offerWatchedData = ProfileManager.Instance.offerWatchedData;
        if (modelWatched.watchedCount == priceAmount)
        {
            priceAmount = 0;
            OnBuy();
        }
        else {
            offerWatchedData.IncreaseWatched(modelWatched);
            InitDataWatch();
        }
    }
    void OpenShop() {
        UIManager.Instance.CloseCustomPanel();
        UIManager.Instance.ShowShopPanel();
    }
    public void UnLock() {
        lockObj.gameObject.SetActive(false);
        btnBuy.gameObject.SetActive(false);
        icon.sprite = sprOn;
        imgBackGround.sprite = sprBG;
    }
}
