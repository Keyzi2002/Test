using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiralCountSlot : MonoBehaviour
{
    [SerializeField] Text spiralCount;
    public Vector3 pointSpawn;
    public Vector3 offSet;
    int count;
    AnimationCurveManager animManager;
    AnimationCurve animCurve;
    AnimationCurve animScaleCurve;
    SpiralCountPanel spiralManager;
    float timeCurve;
    bool animCurveStart;
    float startScale;
    private void Start()
    {
        spiralCount.text = "0";
        transform.position = pointSpawn;
        startScale = transform.localScale.x;
    }
    public void EndCount() {
        timeCurve = 0f;
        animCurveStart = true;
        animManager = AnimationCurveManager.Instance;
        animCurve = animManager.spiralCountEndAnimPosCurve;
        animScaleCurve = animManager.spiralCountEndAnimScaleCurve;
    }
    public void StartCount() {
        count = 0;
        animCurveStart = false;
        animManager = AnimationCurveManager.Instance;
        animCurve = animManager.spiralCountPositionAnimCurve;
        animScaleCurve = animManager.spiralCountScaleAnimCurve;
        timeCurve = 0f;
    }
    private void Update()
    {
        if (animCurveStart)
        {
            if (timeCurve <= animCurve.keys[animCurve.length - 1].time)
            {
                transform.position = Vector3.Lerp(pointSpawn, pointSpawn + offSet, animCurve.Evaluate(timeCurve));
                float scale = startScale + animScaleCurve.Evaluate(timeCurve);
                transform.localScale = new Vector3(scale, scale, transform.localScale.z);
                timeCurve += Time.deltaTime;
            }
            else spiralManager.RemoveSlot(this);
        }
        else
        {
            spiralCount.text = count.ToString();
            if (timeCurve <= animCurve.keys[animCurve.length - 1].time)
            {
                float scale = startScale + animScaleCurve.Evaluate(timeCurve);
                transform.localScale = new Vector3(scale, scale, transform.localScale.z);
                timeCurve += Time.deltaTime;
            }
            else timeCurve = 0f;
        }
    }
    public void ChangeSpiral(int spirals) { count = spirals; }
    public void ChangeSpiralManager(SpiralCountPanel spiralCountPanel) { spiralManager = spiralCountPanel; }
}
