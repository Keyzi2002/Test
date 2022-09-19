using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingManager : Singleton<RankingManager>
{
    private static Dictionary<Character, Top> ranks = new Dictionary<Character, Top>();
    private Queue<Top> tops = new Queue<Top>();
    public override void Awake()
    {
        base.Awake();
        OnInIt();
    }
   
    private void OnInIt()
    {
        ResetAll();
    }
    private void ResetAll()
    {
        ranks.Clear();
        for(int  i = 0; i < System.Enum.GetValues(typeof(Top)).Length; i++)
        {
            Top top = (Top)System.Enum.GetValues(typeof(Top)).GetValue(i);
          
            if (top != Top.None && !tops.Contains(top))
            {
                tops.Enqueue(top);
            }
        }
    }
    public Top GetRank(Character characterSet)
    {
        if (ranks.ContainsKey(characterSet))
        {
            return ranks[characterSet];
        }
        return Top.None;
    }

    public void SetRank(Character characterSet)
    {
        if (!ranks.ContainsKey(characterSet) && ranks.Count < 3)
        {
            ranks.Add(characterSet, tops.Dequeue());
        }
    }

    public enum Top
    {
        None,
        Top_1,
        Top_2,
        Top_3,
    }
}
