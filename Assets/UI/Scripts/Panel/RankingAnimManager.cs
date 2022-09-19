using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingAnimManager : GenerticSingleton<RankingAnimManager>
{
    [Header("===========List controll============")]
    public List<bool> animStart = new List<bool>();
    public List<float> timeAnim = new List<float>();
    public List<float> startScaleY = new List<float>(3);
    public List<Transform> topObjs = new List<Transform>();
    public List<Transform> topObjsNumber = new List<Transform>();
    public List<Character> charactorObjs = new List<Character>();
    [SerializeField] private Transform transformRanking;
    public List<float> maxScaleY;
    [Header("===========Curve============")]
    public AnimationCurve animScaleShakeY;
    [Header("===========Other variable============")]
    [Range(0, 5)]
    public float timeAnimLength;
    [Range(0, 1)]
    public float timeDelay;
    public float timeSpeed;
    int topObjIndex;
    public Destination destination;
    private void Start()
    {
        topObjIndex = 0;
    }
    public void AnimStart() {
        for (int i = 0; i < animStart.Count; i++)
        {
            if (!animStart[i] && i <= topObjIndex)
            {
                timeAnim[i] = 0f;
                startScaleY[i] = topObjs[i].localScale.y;
            }
        }
        if (topObjIndex == 0)
        {
            animStart[0] = true;
        }
    }
    private void Update()
    {
        for (int i = 0; i < animStart.Count && i < charactorObjs.Count; i++)
        {
            if (animStart[i])
            {
                if (timeAnim[i] <= timeAnimLength)
                {
                    Vector3 scaleChange = topObjs[i].localScale;
                    scaleChange.y = Mathf.Clamp(animScaleShakeY.Evaluate(timeAnim[i]) * maxScaleY[i], startScaleY[i], maxScaleY[i] + 100);
                    topObjs[i].localScale = scaleChange;
                    topObjsNumber[i].localScale = Vector3.up * 1 / scaleChange.y * 3 + Vector3.left * 0.6f + Vector3.forward * 0.6f; 
                    timeAnim[i] += Time.deltaTime * timeSpeed;
                }
                else
                {
                    animStart[i] = false;
                    charactorObjs[i].SetPosition(destination.GetTransformTop(i).position);
                }
                if (timeAnim[topObjIndex] >= timeDelay)
                {
                    if (topObjIndex+1 < startScaleY.Count && !animStart[topObjIndex+1])
                    {
                        topObjIndex++;
                        animStart[topObjIndex] = true;
                    }
                }
            }
        }
    }
    public void AddCharactor(Character charactorAdd) { charactorObjs.Add(charactorAdd); }
    public List<Character> GetCharacters_InRank()
    {
        return charactorObjs;
    }
    public Transform GetTransformRanking()
    {
        return transformRanking;
    }
}
