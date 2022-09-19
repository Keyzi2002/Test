using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ShowSkinPanel : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController showSkinController;
    [SerializeField] Transform rootTransform;
    [SerializeField] Transform pointIdleTransform;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Button btnClose;
    [SerializeField] Button btnClaim;
    [SerializeField] float speed;
    [SerializeField] RectTransform showRect;
    [SerializeField] Text txtName;
    [SerializeField] Text txtRarity;

    public Animator anim;
    public AnimationCurve animRotage;
    const string animWalk = "Walking";
    const string animCelebration = "Celebration";
    bool rotage;
    bool move;
    bool done;
    float timeCurve;
    Vector3 startPoint;
    Quaternion rotageTo, rotageFrom;
    Transform newSkinTransform;

    VcamController vcamController;
    private void Awake()
    {
        btnClaim.onClick.AddListener(OnClose);
        btnClose.onClick.AddListener(OnClose);
    }
    public void InitData(ModelItem modelItem) {
        anim = null;
        rotage = false;
        move = true;
        done = false;
        timeCurve = 0;
        ModelAbleObj modelAbleObj = ProfileManager.Instance.profileDataConfig.modelAbleObjData.GetModelSkin(modelItem.modelID, modelItem.itemRarity);
        LoadModelPreview(modelAbleObj, rootTransform, true);

        showRect.sizeDelta = new Vector2(showRect.sizeDelta.x, showRect.rect.width);

    }
    void LoadModelPreview(ModelAbleObj modelAbleObj, Transform root, bool skinLoad = false)
    {
        if (root.childCount > 0)
            Destroy(root.GetChild(0).gameObject);
        GameObject newObj = Instantiate(modelAbleObj.skin, root.position, Quaternion.identity, root);
        newObj.transform.SetAsFirstSibling();
        newObj.transform.rotation = new Quaternion(0, 0, 0, 0);
        newObj.transform.localPosition = new Vector3(0, 0, 0);
        newObj.transform.localScale = new Vector3(1, 1, 1);
        newSkinTransform = newObj.transform;
        if (skinLoad)
        {
            anim = newObj.GetComponent<Animator>();
            anim.runtimeAnimatorController = showSkinController;
            SetAnimToShow();
        }
        SetLayerUIToChild(newObj, 5);
        //Vector3 dir = newSkinTransform.localPosition - pointIdleTransform.localPosition;
        //float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        //newSkinTransform.eulerAngles = new Vector3(0, angle, 0);
        newSkinTransform.LookAt(pointIdleTransform);
        startPoint = newSkinTransform.position;
    }
    void SetLayerUIToChild(GameObject parent, int layer)
    {
        parent.layer = layer;
        foreach (Transform child in parent.transform)
        {
            SetLayerUIToChild(child.gameObject, layer);
        }
    }
    void SetAnimToShow() {
        int ran = UnityEngine.Random.Range(0, 5);
        if (ran == 0)
            speed = .4f;
        else speed = .2f;
        anim.Play(animWalk + ran.ToString());
    }
    private void Update()
    {
        if (done)
            return;
        if (move)
        {
            if (timeCurve <= animRotage.keys[animRotage.length - 1].time)
            {
                newSkinTransform.position = Vector3.Lerp(startPoint, pointIdleTransform.position, animRotage.Evaluate(timeCurve));
                timeCurve += Time.deltaTime * speed;
            }
            else
            {
                newSkinTransform.position = pointIdleTransform.position;
                move = false;
                SetRotageTo();
            }
            return;
        }
        if (rotage)
        {
            if (timeCurve <= animRotage.keys[animRotage.length - 1].time)
            {
                newSkinTransform.rotation = Quaternion.Slerp(rotageFrom, rotageTo, animRotage.Evaluate(timeCurve));
                timeCurve += Time.deltaTime;
            }
            else {
                newSkinTransform.rotation = rotageTo;
                rotage = false;
            }
            return;
        }
        if (anim != null)
        {
            int randomState = UnityEngine.Random.Range(0, 8);
            anim.Play(animCelebration + randomState.ToString());
            done = true;
        }
    }
    void SetRotageTo() {
        Vector3 dir = cameraTransform.position - newSkinTransform.position;
        rotageTo = Quaternion.LookRotation(dir, Vector3.up);
        rotage = true;
        rotageFrom = newSkinTransform.rotation;
    }
    void OnClose() {
        gameObject.SetActive(false);
        UIManager.Instance.CloseRewardPanel();
    }
}
