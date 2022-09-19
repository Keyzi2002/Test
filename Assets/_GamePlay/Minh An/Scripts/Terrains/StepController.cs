using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StepController : Singleton<StepController>
{
  
    [SerializeField] private List<LevelStep> sLevelteps;

    public List<Step_> GetStep_s_NoneColor(int level)
    {
        List<Step_> step_s = new List<Step_>();
        foreach (LevelStep levelStep in sLevelteps)
        {
            if(levelStep.Level == level)
            {
                foreach(Step_ step_ in levelStep.steps)
                {
                    if(step_.nameColorCurrent == NameColor.None)
                    {
                        step_s.Add(step_);
                    }
                }
            }
        }
        return step_s;
    }
    public List<Step_> GetStep_s_All(int level)
    {
        foreach(LevelStep levelStep in sLevelteps)
        {
            if (levelStep.Level == level)
            {
                return levelStep.steps;
            } 
        }
        return null;
    }
    public bool CheckStepInLevel(Step_ step_Check, int LevelCheck)
    {
        if(step_Check == null)
        {
            return false;
        }
        foreach(Step_ step_ in GetStep_s_All(LevelCheck))
        {
            if(step_ == step_Check)
            {
                return true;
            }
        }
        return false;
    }
    public int GetLevelInStep(Step_ step_)
    {
        foreach(LevelStep levelStep in sLevelteps)
        {
            if (levelStep.steps.Contains(step_))
            {
                return levelStep.Level;
            }
        }
        return -1;
    }
}
[Serializable]
public class LevelStep
{
    public int Level;
    public List<Step_> steps = new List<Step_>();
}
