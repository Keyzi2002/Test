using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPraise : UIPanel
{
    public override void Awake()
    {
        uiType = UIType.Praise;
        base.Awake();
    }
    public GameObject amazing;
    public GameObject perfect;
    public GameObject good;
    public Vector3 offset;
    public AnimationCurve posAnimCurve;
    public AnimationCurve scaleAnimCurve;
    public float timeEndActive;
    Transform transformActive;
    Vector3 startPoint, endPoint;
    Vector3 startScale, endScale;
    float timeCurve = 0;
    bool animDone = true;
    private void Update()
    {
        if (animDone)
            return;
        if (transformActive != null)
        {
            if (timeCurve <= posAnimCurve.keys[posAnimCurve.length - 1].time)
            {
                transformActive.position = Vector3.Lerp(startPoint, endPoint, posAnimCurve.Evaluate(timeCurve));
                Vector3 vectorAdd = startScale * scaleAnimCurve.Evaluate(timeCurve);
                transformActive.localScale = vectorAdd;
                timeCurve += Time.deltaTime;
            }
            else {
                animDone = true;
                StopAllCoroutines();
                StartCoroutine(EndActive());
            }
        }
    }
    IEnumerator EndActive() {
        yield return new WaitForSeconds(timeEndActive);
        transformActive.gameObject.SetActive(false);
        transformActive.position = startPoint;
        transformActive.localScale = startScale;
        transformActive = null;
        UIManager.Instance.ClosePraisePanel();
    }
    public void ShowPraise(PraiseID idPraise) {
        if (!animDone) return;
        switch (idPraise)
        {
            case PraiseID.Amazing:
                amazing.SetActive(true);
                perfect.SetActive(false);
                good.SetActive(false);
                transformActive = amazing.transform;
                break;
            case PraiseID.Perfect:
                amazing.SetActive(false);
                perfect.SetActive(true);
                good.SetActive(false);
                transformActive = perfect.transform;
                break;
            case PraiseID.Good:
                amazing.SetActive(false);
                perfect.SetActive(false);
                good.SetActive(true); 
                transformActive = good.transform;
                break;
            default:
                break;
        }
        startPoint = transformActive.position;
        startScale = transformActive.localScale;
        endPoint = startPoint + offset;
        transformActive.localScale = Vector3.zero;
        timeCurve = 0f;
        animDone = false;
    }
}
