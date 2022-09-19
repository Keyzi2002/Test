using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMapSlot : MonoBehaviour
{
    [SerializeField] Button btnSelectMap;
    [SerializeField] Image imgIcon;
    [SerializeField] List<GameObject> stars;
    [SerializeField] Text txtLevelName;
    [SerializeField] GameObject lockImage;
    private void Awake()
    {
        btnSelectMap.onClick.AddListener(SelectMap);
    }
    void SelectMap() {
        SelectMapPanel selectMapPanel = UIManager.Instance.GetUIPanel(UIType.SelectMapPanel).GetComponent<SelectMapPanel>();
        Debug.Log(transform.GetSiblingIndex());
        selectMapPanel.OnSelectedLevel(transform.GetSiblingIndex());
    }
    public void InitData(Sprite spr, int starInt, string levelNameChange) {
        imgIcon.sprite = spr;
        if (starInt >= 0)
        {
            lockImage.SetActive(false);
            for (int i = 0; i < starInt; i++)
            {
                if (i == stars.Count) // Incase parameter starInt is more than 3 
                {
                    break;
                }
                stars[i].SetActive(true);
            }
            btnSelectMap.interactable = true;
        }
        else
        {
            lockImage.SetActive(true); 
            btnSelectMap.interactable = false;
        }
        txtLevelName.text = levelNameChange;
    }
}
