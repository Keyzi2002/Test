using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ThrillingPlayer : UI_Canvas
{
    public Transform myTranform;
    [SerializeField] private List<Sprite> Angrys;
    [SerializeField] private List<Sprite> Funs;
    [SerializeField] private Image imgThrilling;
    [SerializeField] private AnimationCurve animationScaleCurve;
    [SerializeField] private float SpeedThrilling = 1;
    [SerializeField] private GameObject objFull;
    private Transform cameraMainTransForm;
    private Transform transformImgThrilling;
    private void Awake()
    {
        if (myTranform == null) { myTranform = this.transform; }
        if (cameraMainTransForm == null) { cameraMainTransForm = Camera.main.transform; }
        if(transformImgThrilling == null) { transformImgThrilling = imgThrilling.transform; }
    }
    public void PlayThrilling(ThrillingManager.NameThrilling nameThrilling)
    {
        switch (nameThrilling)
        {
            case ThrillingManager.NameThrilling.Angry:
                SetImgThrilling(Angrys[Random.Range(0, Angrys.Count)]);
                break;
            case ThrillingManager.NameThrilling.Fun:
                SetImgThrilling(Funs[Random.Range(0, Funs.Count)]);
                break;
        }
        AnimThrillingPlayer();
    }
    private void AnimThrillingPlayer()
    {
        StartCoroutine(IE_Anim());
    }
    IEnumerator IE_Anim()
    {
        float m_time = 0;
        Vector3 vt_ScaleStart = transformImgThrilling.localScale = Vector3.zero;
        while (m_time < 1)
        {
            transformImgThrilling.localScale = Vector3.Lerp(vt_ScaleStart, Vector3.one * animationScaleCurve.Evaluate(m_time), animationScaleCurve.Evaluate(m_time));
            m_time += Time.deltaTime * SpeedThrilling;
            yield return null;
        }
        Close();
        yield return null;
    }
    //public override void Open()
    //{
    //    objFull.SetActive(true);
    //}
    //public override void Close()
    //{
    //    objFull.SetActive(false);
    //}
    private void SetImgThrilling(Sprite sprite)
    {
        imgThrilling.sprite = sprite;
    }
    private void Update()
    {
        myTranform.LookAt(myTranform.position + cameraMainTransForm.rotation * Vector3.forward /*+ Vector3.forward*/, cameraMainTransForm.rotation * Vector3.up);
    }
}

