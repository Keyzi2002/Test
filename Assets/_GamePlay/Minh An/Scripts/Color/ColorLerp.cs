using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLerp : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRendererThis;
    [SerializeField] private Color colorStart;
    [SerializeField] private Color colorEnd;
    [SerializeField] private float SpeedLerp;
    [Range(0, 1)]
    [SerializeField] private float valueLerp;
    private bool isStartLerp;
    private void Awake()
    {
        OnInIt();
    }
    private void OnInIt()
    {
        Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        meshRendererThis.material = material;
        isStartLerp = false;
        meshRendererThis.enabled = false;
      //  StartLerp(0);
    }
    public void SetColor(Color value)
    {
        meshRendererThis.enabled = true;
        SetColorStartAndEnd(value, value);
        meshRendererThis.material.color = value;
    }
    public void SetColorStartAndEnd(Color colorStart, Color colorEnd)
    {
        this.colorStart = colorStart;
        this.colorEnd = colorEnd;
    }
    public Color GetColorStart()
    {
        return colorStart;
    }
    public Color GetColorEnd()
    {
        return colorEnd;
    }
    public void StartLerp(float valueColorLerpStart)
    {
        if (isStartLerp)
        {
            return;
        }
        isStartLerp = true;
        valueLerp = valueColorLerpStart;
        meshRendererThis.enabled = true;
        StartCoroutine(IE_LoadLerp());
    }
    IEnumerator IE_LoadLerp()
    {
        while(valueLerp < 1)
        {
            meshRendererThis.material.color = Color.Lerp(colorStart, colorEnd, valueLerp);
            valueLerp += Time.deltaTime * SpeedLerp;
            yield return null;
        }
        isStartLerp = false;
    }
}
