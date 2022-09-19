using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Step_ : MonoBehaviour
{
    public NameColor nameColorCurrent = NameColor.None;
    [SerializeField] private Transform transHead;
    [SerializeField] private Distance distancePrefab;
    [SerializeField] private Transform transGate;
    [SerializeField] private Transform transPointComplete;
    [SerializeField] private Transform transEnd;

    [Range(0, 1)]
    [SerializeField] private float valueLoadStep;
    [SerializeField] private SettingStep settingStepCurrent;
    private List<Distance> distances = new List<Distance>();
    private Action actionComplete;
    private int countDistanceFull;
    private int countCurrentMax;
    private int countDistances_ColorPink = 0;
    private int countDistances_ColorBlue = 0;
    private int countDistances_ColorYeallow = 0;
    private int countDistances_ColorGreen = 0;
    private int countDistances_ColorViolet = 0;
    private int countDistances_ColorOrange = 0;
    private int maxCount = 0;
    private NameColor nameColorComplete_Current = NameColor.None;

    void Awake()
    {
        countDistanceFull = (int)(Mathf.Abs(transHead.localPosition.x) + Mathf.Abs(transEnd.localPosition.x));
        settingStepCurrent = DataManager.Instance.GetDataStep().GetSettingStep();
        SetActionComplete(() => 
        {
            List<Character> characters = new List<Character>();
            foreach (Character character in GameManager.Instance.GetCharacters_InGame())
            {
                characters.Add(character);
            }
           
            foreach(Character character in characters)
            {
                if(character.nameColorThis == nameColorComplete_Current)
                {
                    GameManager.Instance.SetLevelCharacterInGame(character, StepController.Instance.GetLevelInStep(this));
                }
            }
        });
    }
    private void Start()
    {
        EnventManager.AddListener(EventName.DistanceSetNewColor.ToString(), () => 
        {
            if (distances.Count > 0)
            {
                foreach (Character character in GameManager.Instance.GetCharacters_InGame())
                {
                    if (distances[distances.Count - 1].GetNameColor() == character.nameColorThis && StepController.Instance.GetLevelInStep(this) - 1 == GameManager.Instance.GetLevelCharacterInGame(character))
                    {
                        nameColorComplete_Current = distances[distances.Count - 1].GetNameColor();
                        actionComplete?.Invoke();
                    }
                }
                //if (distances[distances.Count - 1].GetNameColor() != nameColorComplete_Current)
                //{
                //    nameColorComplete_Current = distances[distances.Count - 1].GetNameColor();
                //    actionComplete?.Invoke();
                //}
            }

        });
    }
    void Update()
    {
        countCurrentMax = (int)(countDistanceFull * valueLoadStep);
        valueLoadStep += 1.0f/settingStepCurrent.amountScoreRollToBuild;
        valueLoadStep = Mathf.Clamp(valueLoadStep, 0, 1);
        if (distances.Count < countCurrentMax)
        {
            Distance distance = Instantiate(distancePrefab, transHead);
            distance.actionSetColor = LoadCheckColor;
            distance.myTransform.localPosition = Vector3.right * (distances.Count + 1) * 0.98f;
            distance.myTransform.localEulerAngles = Vector3.forward * -90;
            distance.SetNameColor(NameColor.None);
            distances.Add(distance);
        }
       
    }
    private void LoadCheckColor()
    {
        countDistances_ColorPink = 0;
        countDistances_ColorBlue = 0;
        countDistances_ColorYeallow = 0;
        countDistances_ColorGreen = 0;
        countDistances_ColorViolet = 0;
        countDistances_ColorOrange = 0;
        foreach (Distance distance in distances)
        {
            switch (distance.GetNameColor())
            {
                case NameColor.Pink:
                    countDistances_ColorPink++;
                    break;
                case NameColor.Green:
                    countDistances_ColorGreen++;
                    break;
                case NameColor.Blue:
                    countDistances_ColorBlue++;
                    break;
                case NameColor.Yeallow:
                    countDistances_ColorYeallow++;
                    break;
                case NameColor.Violet:
                    countDistances_ColorViolet++;
                    break;
                case NameColor.Orange:
                    countDistances_ColorOrange++;
                    break;
            }
        }
        maxCount = Mathf.Max(countDistances_ColorPink, countDistances_ColorBlue, countDistances_ColorYeallow, countDistances_ColorGreen, countDistances_ColorViolet, countDistances_ColorOrange);
        if(maxCount > 0)
        {
            if (maxCount == countDistances_ColorPink)
            {
                nameColorCurrent = NameColor.Pink;
            }
            else if (maxCount == countDistances_ColorBlue)
            {
                nameColorCurrent = NameColor.Blue;
            }
            else if (maxCount == countDistances_ColorYeallow)
            {
                nameColorCurrent = NameColor.Yeallow;
            }
            else if(maxCount == countDistances_ColorGreen)
            {
                nameColorCurrent = NameColor.Green;
            }
            else if(maxCount == countDistances_ColorViolet)
            {
                nameColorCurrent = NameColor.Violet;
            }
            else
            {
                nameColorCurrent = NameColor.Orange;
            }
        }
     
    }
    public List<Distance> GetDistances()
    {
        return distances;
    }
    public Transform GetTransformGate()
    {
        return transGate;
    }
    public Transform GetTransPointComplete()
    {
        return transPointComplete;
    }
    public float GetProgress()
    {
        return valueLoadStep;
    }
    public NameColor GetNameColor_Complete()
    {
        return nameColorComplete_Current;
    }
    public void SetActionComplete(Action action)
    {
        actionComplete = action;
    }
}
