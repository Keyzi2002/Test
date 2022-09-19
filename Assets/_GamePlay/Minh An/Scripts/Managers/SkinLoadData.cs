using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SkinLoadData : MonoBehaviour
{
    public Transform skinRoot;
    public Transform shovelRoot;
    public Transform backPackRoot;
    #region model on Charactor
    public void LoadSkin(ModelAbleObj modelAbleObj, UnityAction loadDone = null) {
        Transform root = skinRoot;
        LoadModel(modelAbleObj, root, 1.5f, loadDone);
    }
    public void LoadBackPack(ModelAbleObj modelAbleObj, UnityAction loadDone = null)
    {
        Transform root = backPackRoot;
        LoadModel(modelAbleObj, root, 1, loadDone);
    }
    public void LoadShovel(ModelAbleObj modelAbleObj) {
        Transform root = shovelRoot;
        LoadModel(modelAbleObj, root, 1);
    }
    /// <summary>
    /// don't need scaleMultiple
    /// </summary>
    /// <param name="modelAbleObj"></param>
    /// <param name="root"></param>
    /// <param name="scaleMultiple"></param>
    void LoadModel(ModelAbleObj modelAbleObj, Transform root, float scaleMultiple, UnityAction loadDone = null) {
        if (root.childCount > 0)
            Destroy(root.GetChild(0).gameObject);
        GameObject newObj = Instantiate(modelAbleObj.skin, root.position, Quaternion.identity, root);
        newObj.transform.SetAsFirstSibling();
        newObj.transform.localRotation = new Quaternion(0, 0, 0, 0);
        newObj.transform.localPosition = new Vector3(0, 0, 0);
        newObj.transform.localScale = new Vector3(1 * scaleMultiple, 1 * scaleMultiple, 1 * scaleMultiple);
        if (loadDone != null)
            loadDone();
    }
    #endregion

    #region Preview
    RuntimeAnimatorController controllerPreview;
    public void LoadSkinPreview(ModelAbleObj modelAbleObj) {
        Transform root = skinRoot;
        LoadModelPreview(modelAbleObj, root, true);
    }
    public void LoadBackPackPreview(ModelAbleObj modelAbleObj) {
        Transform root = backPackRoot;
        LoadModelPreview(modelAbleObj, root);
    }
    public void LoadShovelPreview(ModelAbleObj modelAbleObj) {
        Transform root = shovelRoot;
        LoadModelPreview(modelAbleObj, root);
    }
    void LoadModelPreview(ModelAbleObj modelAbleObj, Transform root, bool skinLoad = false) {
        if (root.childCount > 0)
            Destroy(root.GetChild(0).gameObject);
        GameObject newObj = Instantiate(modelAbleObj.skin, root.position, Quaternion.identity, root);
        newObj.transform.SetAsFirstSibling();
        newObj.transform.rotation = new Quaternion(0, 0, 0, 0);
        newObj.transform.localPosition = new Vector3(0, 0, 0);
        newObj.transform.localScale = new Vector3(1, 1, 1);
        if (skinLoad)
        {
            AnimatorPreviewModelController animController = newObj.AddComponent<AnimatorPreviewModelController>();
            animController.InitData();
            Animator anim = animController.GetAnimator();
            anim.runtimeAnimatorController = controllerPreview;
        }
        SetLayerUIToChild(newObj, 5);
    }
    void SetLayerUIToChild(GameObject parent, int layer) {
        parent.layer = layer;
        foreach (Transform child in parent.transform)
        {
            SetLayerUIToChild(child.gameObject, layer);
        }
    }
    public void SetControllerPreview(RuntimeAnimatorController controllerChange) {
        controllerPreview = controllerChange;
    }
    #endregion
}
