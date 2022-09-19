using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTerrainInMap : MonoBehaviour
{
    public static Transform myTransform;
    [SerializeField] private int WithMap, HeightMap, WithTerrain, HeightTerrain;
    [SerializeField] private int amountRow, amountColum;
    [SerializeField] private Vector3 vt_Offset;
   // [SerializeField] private Queue<Queue<GameObject>> matrixTerrain = new Queue<Queue<GameObject>>(); //Queue dau la hang, Queue sau la cac terrain trong hang
    [SerializeField] private GameObject objTerrainPrefab;
    [SerializeField] private List<DestructibleTerrain> destructibles_Setting_Default = new List<DestructibleTerrain>();
    private float distance_Between_2_Terrain_Row, distance_Between_2_Terrain_Colum;
    private Vector3 pointCloneTerrain = Vector3.zero;
    private Dictionary<NameColor, List<DestructibleTerrain>> destructibleTerrains = new Dictionary<NameColor, List<DestructibleTerrain>>();
    private void Awake()
    {
        if(myTransform == null) { myTransform = this.transform; }
    }
    //private void Start()
    //{
    //    OnInIt();
    //}
    public void OnInIt()
    {
       // amountColum = GameManager.Instance.GetAmountCharacter();
        distance_Between_2_Terrain_Row = (WithMap - amountColum * WithTerrain) / amountRow;
        distance_Between_2_Terrain_Colum = (HeightMap - amountRow * HeightTerrain) / amountColum;
        pointCloneTerrain.x = -WithMap / 2;
        pointCloneTerrain.z = -HeightMap / 2 - distance_Between_2_Terrain_Row / 2;
        CreateTerrain();
        foreach(DestructibleTerrain destructibleTerrain in destructibles_Setting_Default)
        {
            List<DestructibleTerrain> destructibleTerrains = this.destructibleTerrains[destructibleTerrain.GetNameColor()];
            destructibleTerrains.Add(destructibleTerrain);
            this.destructibleTerrains[destructibleTerrain.GetNameColor()] = destructibleTerrains;

            destructibleTerrain.myTransformParent = myTransform;
            destructibleTerrain.Setresolution(WithTerrain, HeightTerrain);
            Character player = GameManager.Instance.GetCharacter_Player();
            if (player != null)
            {
                destructibleTerrain.SetActionRoll(player.ActionRoll);
                destructibleTerrain.SetActionComplete(() =>
                {
                    player.ActionComplete();
                });
            }
            destructibleTerrain.Active(false);
        }
    }
    private void CreateTerrain()
    {
        for (int i = 0; i < amountRow; i++)
        {
            List<NameColor> nameColorFakes = new List<NameColor>();
            foreach(NameColor nameColor in GameManager.Instance.GetNameColors_UseInPlayer_InGame())
            {
                nameColorFakes.Add(nameColor);
            }
            Character player = GameManager.Instance.GetCharacter_Player();
            for (int j = 0; j < amountColum; j++)
            {
                GameObject objTerrain = Instantiate(objTerrainPrefab, pointCloneTerrain, Quaternion.Euler(90, 0, 0), transform);
                objTerrain.transform.localPosition = pointCloneTerrain + vt_Offset;
                DestructibleTerrain destructibleTerrain = objTerrain.GetComponentInChildren<DestructibleTerrain>();
                destructibleTerrain.myTransformParent = myTransform;
                LoadColor:
                if (nameColorFakes.Count > 0)
                {
                    int valueRandomColor = Random.Range(0, nameColorFakes.Count);
                    destructibleTerrain.SetNameColor(nameColorFakes[valueRandomColor]);
                
                    List<DestructibleTerrain> destructibleTerrains = null;
                    if (this.destructibleTerrains.ContainsKey(nameColorFakes[valueRandomColor]))
                    {
                        destructibleTerrains = this.destructibleTerrains[nameColorFakes[valueRandomColor]];
                        destructibleTerrains.Add(destructibleTerrain);

                    }
                    else
                    {
                        destructibleTerrains = new List<DestructibleTerrain>();
                        destructibleTerrains.Add(destructibleTerrain);
                    }
                    this.destructibleTerrains[nameColorFakes[valueRandomColor]] = destructibleTerrains;
                    nameColorFakes.RemoveAt(valueRandomColor);

                }
                else
                {
                    foreach (NameColor nameColor in GameManager.Instance.GetNameColors_UseInPlayer_InGame())
                    {
                        nameColorFakes.Add(nameColor);
                    }
                    goto LoadColor;
                }
                destructibleTerrain.Setresolution(WithTerrain, HeightTerrain);
                pointCloneTerrain.x += WithMap / amountColum + distance_Between_2_Terrain_Colum/2;
                if(player != null)
                {
                    destructibleTerrain.SetActionRoll(player.ActionRoll);
                    destructibleTerrain.SetActionComplete(() =>
                    {
                        player.ActionComplete();
                    });

                    //destructibleTerrain.SetActionBeforeComplete(() =>
                    //{
                    //    player.GetTerrainRoll()?.StopGrowth();
                    //});
                }
                destructibleTerrain.Active(false);
            }
            pointCloneTerrain.z += HeightMap / amountRow + distance_Between_2_Terrain_Row/2;
            pointCloneTerrain.x = -WithMap/2;
           // matrixTerrain.Enqueue(gameObjectsInColum);
        }

    }
    public List<DestructibleTerrain> GetDestructibleTerrains(NameColor nameColor)
    {
        if(destructibleTerrains.Count == 0)
        {
            OnInIt();
        }
        if (destructibleTerrains.ContainsKey(nameColor))
        {
            return destructibleTerrains[nameColor];
        }
        return null;
    }
}

