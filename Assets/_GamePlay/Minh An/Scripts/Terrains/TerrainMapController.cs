using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TerrainMapController : MonoBehaviour
{
    private static TerrainMapController instance;
    public static TerrainMapController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<TerrainMapController>();
            }
            return instance;
        }
    }

    [SerializeField] private List<DictInfoCreateTerrainMap> createTerrainInMaps;
    [SerializeField] private int levelMaxInMap = 0;
    private void Start()
    {
        EnventManager.AddListener(EventName.StartGame.ToString(), ReLoadTerrainInMap);
        EnventManager.AddListener(EventName.LevelUpPlayerInMap.ToString(), ReLoadTerrainInMap);
    }
    public CreateTerrainInMap GetCreateTerrainInMap(int Level)
    {
        List<CreateTerrainInMap> _createTerrainInMaps = new List<CreateTerrainInMap>();
        foreach(DictInfoCreateTerrainMap dictInfoCreateTerrainMap in createTerrainInMaps)
        {
            if(dictInfoCreateTerrainMap.level == Level)
            {
                _createTerrainInMaps.Add(dictInfoCreateTerrainMap.createTerrain);
            }
        }
        if (_createTerrainInMaps.Count > 0)
        {
            return _createTerrainInMaps[UnityEngine.Random.Range(0, _createTerrainInMaps.Count)];
        }
        return null;
    }
    private void ReLoadTerrainInMap()
    {
        foreach(Character character in GameManager.Instance.GetCharacters_InGame())
        {
            for(int i = 0; i < createTerrainInMaps.Count; i++)
            {
                if (GameManager.Instance.GetLevelCharacterInGame(character) >= createTerrainInMaps[i].level)
                {
                    foreach(DestructibleTerrain destructibleTerrain in createTerrainInMaps[i].createTerrain.GetDestructibleTerrains(character.nameColorThis))
                    {
                        destructibleTerrain.Active(true);
                    }
                }
                else
                {
                    foreach (DestructibleTerrain destructibleTerrain in createTerrainInMaps[i].createTerrain.GetDestructibleTerrains(character.nameColorThis))
                    {
                        destructibleTerrain.Active(false);
                    }
                }
            }
            
        }

    }
    public int GetLevelMaxInMap()
    {
        return levelMaxInMap;
    }
}
[Serializable]
public class DictInfoCreateTerrainMap
{
    public int level;
    public CreateTerrainInMap createTerrain;
}
