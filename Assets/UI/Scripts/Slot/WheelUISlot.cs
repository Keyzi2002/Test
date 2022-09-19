using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelUISlot : MonoBehaviour
{
    [SerializeField] Image imgIcon;
    [SerializeField] Text txtName;
    [SerializeField] Text txtRarity;
    [SerializeField] Transform trsEffect;
    Animator anim;
    Item item;
    DecorItem decoreItem;
    public float minAngle, maxAngle;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void InitData() {
        imgIcon.sprite = item.itemSprite;
        decoreItem = ProfileManager.Instance.profileDataConfig.rewardData.GetDecore(item.itemRarity);
        switch (item.itemType)
        {
            case ItemType.Gem:
                txtName.text = item.itemName;
                txtRarity.text = item.amount.ToString();
                break;
            case ItemType.Skin:
                txtName.text = item.itemName;
                txtRarity.text = "<color=#" + ColorUtility.ToHtmlStringRGBA(decoreItem.color) + ">" + item.itemRarity.ToString() + "</color>";
                if (decoreItem.effect != null)
                {
                    if (trsEffect.childCount > 0)
                        Destroy(trsEffect.GetChild(0));
                    Instantiate(decoreItem.effect, trsEffect);
                }
                break;
            default:
                break;
        }
        
        
    }
    public void SetItem(Item itemChange) { item = itemChange; }
    public Item GetItem() { return item; }
    public void SetMaxMinAngle(float lastAngle) {
        minAngle = lastAngle + 22.5f + 1f;
        maxAngle = minAngle + 45f - 1f;
    }
    public void SetFirstMinAngle(float angle) {
        minAngle = angle;
    }
    public void BingoAnim() { anim.SetTrigger("Bingo"); }
}
