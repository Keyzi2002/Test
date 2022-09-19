using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingLineController : Singleton<StartingLineController>
{
    [SerializeField] private List<Transform> transformsPointPlayerSpawn;
    public List<Transform> GetTransformsPointPlayerSpawn()
    {
        return transformsPointPlayerSpawn;
    }
}
