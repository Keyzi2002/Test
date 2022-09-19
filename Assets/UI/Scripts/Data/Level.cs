using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[System.Serializable]
public class Level
{
    public string levelName;
    //public Sprite sprSprite;
    public GameObject missionObjectMap;
    public NavMeshData mapNavMesh;
    public int gemReward;
}
