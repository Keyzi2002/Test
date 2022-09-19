using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetStep : MonoBehaviour
{
    public Transform myTransform;
    public NameColor nameColor;
    [SerializeField] private GameObject objLimit;
    [Range(0, 1)]
    [SerializeField] private float valueLoadStep = 0;
    private float Distane_With = 0;
    private float Distane_Height = 0;
    private Vector3 scaleTaget;

    private void Awake()
    {
        OnInIt();
    }
    private void OnInIt()
    {
        if(myTransform == null) { myTransform = this.transform; }
    }
    public void Complete()
    {
        objLimit.SetActive(false);
    }
    public void BuildStreet(int scoreLoad = 0, int amountScoreRollToBuild = 1)
    {
        valueLoadStep += scoreLoad * 1.0f / amountScoreRollToBuild * 1.0f;
        valueLoadStep = Mathf.Clamp(valueLoadStep, 0, 1);
        scaleTaget.x = valueLoadStep * Distane_Height;
        myTransform.localScale = Vector3.right * scaleTaget.x + Vector3.up * scaleTaget.y + Vector3.forward;
    }
    public float GetValueLoadStep()
    {
        return valueLoadStep;
    }
    public void SetLayerLimit(string nameLayer)
    {
        objLimit.layer = LayerMask.NameToLayer(nameLayer);
    }
    public void SetDistaneWith(float value)
    {
        Distane_With = value;
    }
    public void SetDistaneHeight(float value)
    {
        Distane_Height = value;
    }
    public void SetScaleTaget(Vector3 value)
    {
        scaleTaget = value;
    }
    public Vector3 GetScaleTaget()
    {
        return scaleTaget;
    }
}
