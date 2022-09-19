using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TerrainRoll : MonoBehaviour
{
    public Transform myTransform;
    [SerializeField] private float MinSize, MaxSize;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material materialPrefab_Pink, materialPrefab_Green, materialPrefab_Blue, materialPrefab_Yeallow, materialPrefab_Violet, materialPrefab_Orange;
    [SerializeField] float spin = 1f;
    [SerializeField] float spinMax = 45;
    [SerializeField] float SpeedRoll = 0.5f;
    public Renderer render;
    public AnimationCurve animCurve;
    public float YOffset = 5;
    float timeCurve;

    private Vector3 vt_fakeScale;
    private Action actionComplete;
    private Action actionCompleteAnim;
    private bool isComplete = false;
  //  private bool isActiveGrowth = true;
    private Material materialThis;
    private Transform transformTagetComplete;
    private void Awake()
    {
        if(myTransform == null) { myTransform = this.transform; }
        isComplete = false;
        timeCurve = 0;

        //animCurve.keys[animCurve.length - 1].value = spinMax;
        //animCurve.keys[animCurve.length - 1].time = spinMax;
    }
 
    public void StartGrowth()
    {
        if (isComplete)
        {
            return;
        }
        if (timeCurve <= 1)
        {
            spin = Mathf.Lerp(1, spinMax, timeCurve);
            render.sharedMaterial.SetFloat("_Spin", spin);
            vt_fakeScale = myTransform.localScale;
            vt_fakeScale.y = Mathf.Lerp(MinSize, MaxSize, timeCurve);
            vt_fakeScale.x = Mathf.Lerp(MinSize, MaxSize, timeCurve);
            myTransform.localScale = vt_fakeScale;
            timeCurve += Time.deltaTime * SpeedRoll;
        }
        else {
            actionComplete?.Invoke();
            Complete();
        }
    }
    public void On_Destroy()
    {
        Destroy(gameObject);
    }
    public void CrateMaterial(NameColor color)
    {
        Material materialPrefab = materialPrefab_Pink;
        switch (color)
        {
            case NameColor.Pink:
                materialPrefab = materialPrefab_Pink;
                break;
            case NameColor.Green:
                materialPrefab = materialPrefab_Green;
                break;
            case NameColor.Blue:
                materialPrefab = materialPrefab_Blue;
                break;
            case NameColor.Yeallow:
                materialPrefab = materialPrefab_Yeallow;
                break;
            case NameColor.Violet:
                materialPrefab = materialPrefab_Violet;
                break;
            case NameColor.Orange:
                materialPrefab = materialPrefab_Orange;
                break;
        }
        materialThis = Instantiate(materialPrefab);
    }
    public Material GetMaterial()
    {
        return materialThis;
    }
    public void Complete()
    {
        isComplete = true;
        if (myTransform.gameObject.activeInHierarchy)
        {
            StartCoroutine(IE_AnimComplete());
        }
      
        // gameObject.SetActive(false);
    }
  
    IEnumerator IE_AnimComplete()
    {
        Vector3 point_Start = myTransform.position;
        Vector3 Scale_Start = myTransform.localScale;
        Vector3 Euler_Start = myTransform.localEulerAngles;
        float m_Time = 0;
        while(m_Time <= 1)
        {
            myTransform.position = Vector3.Lerp(point_Start, transformTagetComplete.position, m_Time) + Vector3.up * YOffset * animCurve.Evaluate(m_Time);
            if(m_Time >= 0.6f)
            {
                myTransform.localScale = Vector3.Lerp(Scale_Start, Vector3.zero, 0.6f);
            }
            //myTransform.localEulerAngles = Vector3.Lerp(Euler_Start, Vector3.right * 90, m_Time);
            m_Time += Time.deltaTime * 2;
            yield return null;
        }
        actionCompleteAnim?.Invoke();
       // gameObject.SetActive(false);
    }
    public void SetTransTagetComplete(Transform transformTaget)
    {
        transformTagetComplete = transformTaget;
    }
    public float GetMinSize()
    {
        return MinSize;
    }
    public float GetMaxSize()
    {
        return MaxSize;
    }
    public float GetSizeCurrent()
    {
        return myTransform.localScale.x;
    }
    public void SetActionComplete(Action action)
    {
        actionComplete = action;
    }
    public void SeetActionCompleteAnim(Action action)
    {
        actionCompleteAnim = action;
    }
    public void SetColor(Material materialTaget)
    {
        meshRenderer.material = materialTaget;
    }
    public void SetSpinAndLoadsharedMaterial(float value)
    {
        spin = value;
        render.sharedMaterial.SetFloat("_Spin", spin);
    }
    public float GetVlueSpinShaderMaterial()
    {
        return spin;
    }
}
