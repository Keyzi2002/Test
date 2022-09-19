using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptAbleObjects/NewLevelData")]
public class LevelData : ScriptableObject
{
    public List<Level> levels;
    //public Sprite GetSpriteLevel(int level) { return levels[level].sprSprite; }
    public int GetGemLevel(int level) { 
        return levels[level].gemReward;
    }
}
