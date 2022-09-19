using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopSlot : MonoBehaviour
{
    public Text title;
    public Text description;
    public Text price;
    public Image icon;
    public Button btnBuy;
    public List<RawImage> rawIcons;
    OfferID offerID;
    public void InitData(OfferData offerData) {
        title.text = offerData.titleDeal.ToUpper();
        description.text = offerData.descriptions;
        price.text = offerData.price.ToString() + "$";
        
        offerID = offerData.offerID;

        if (offerData.icon != null)
            icon.sprite = offerData.icon;
        else {
            icon.gameObject.SetActive(false);
        }
    }
    private void Awake()
    {
        btnBuy.onClick.AddListener(Buy);
    }
    void Buy() { }
}
