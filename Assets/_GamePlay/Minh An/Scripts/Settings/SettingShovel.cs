using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings/Shovel")]
public class SettingShovel : ScriptableObject
{
    public Shovel.TypeShovel typeShovel;
    public int Level;
    public Vector3 scale;
    public int ScoreMin, ScoreMax;

}
