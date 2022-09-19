using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shovel : MonoBehaviour
{
    public Transform myTransform;
    [SerializeField] private LayerMask layerTaget;
    [SerializeField] private StateRoll stateRollCurrent;
    [SerializeField] private Character player;
    [SerializeField] private float Radius;
    [SerializeField] private Transform TransHead;
    [SerializeField] private Transform TransSpawnTerrainRoll;
    [SerializeField] private Transform BanXuc;
    [SerializeField] private SettingShovel settingShovelCurrent;
    private DestructibleTerrain destructibleTerrain;
    private void Awake()
    {
        if (myTransform == null) { myTransform = this.transform; }
        LoadAndSetDataShovel();
        player = GetComponentInParent<Character>();
    }
    private void Start()
    {
        EnventManager.AddListener(EventName.StartGame.ToString(), SetActionShovel);
    }
    public void SetActionShovel() {
        if (player.GetBagTerrainRoll() != null)
        {
            //Bag full
            player.GetBagTerrainRoll().SetActionBagFull(() =>
            {
                SetStateAction(StateRoll.DontRoll);
                StartCoroutine(IE_Singtelon.IE_DelayAction(() => { player.ActionComplete(); }, 0.25f));
               
            });
            //Bag not full
            player.GetBagTerrainRoll().SetActionBagNotFull(() =>
            {
                SetStateAction(StateRoll.Roll);
            });
        }
    }
    public void LoadAndSetDataShovel()
    {
        settingShovelCurrent = DataManager.Instance.GetDataShovel().GetSettingShovel();
        BanXuc.localScale = settingShovelCurrent.scale;
        
    }
  
    public void SetStateAction(StateRoll stateRoll)
    {
        if(stateRollCurrent == stateRoll)
        {
            return;
        }
        stateRollCurrent = stateRoll;
        switch (stateRollCurrent)
        {
            case StateRoll.Roll:
                //enable script RuntimeCircleClipper de nhan dat va roll dat
                IsActiveShovelSoil(true);
                break;
            case StateRoll.DontRoll:
                //disable script RuntimeCircleClipper de khong nhan dat va roll dat
                IsActiveShovelSoil(false);
                break;
        }
    }
    
    private void IsActiveShovelSoil(bool isActive)
    {
        player.GetRuntimeCircleClipper().IsActive(isActive);
    }
    private void OnTriggerEnter(Collider other)
    {
        destructibleTerrain = Cache.GetComponetDestructibleTerrainInParent(other);
        if (player.nameColorThis != destructibleTerrain.GetNameColor())
        {
            return;
        }
        destructibleTerrain.SetActionRoll(player.ActionRoll);
        destructibleTerrain.SetActionComplete(() =>
        {
            player.ActionComplete();
        });

        //destructibleTerrain.SetActionBeforeComplete(() => 
        //{
        //    player.GetTerrainRoll()?.StopGrowth();
        //});
        player.GetRuntimeCircleClipper().SetTerrain(destructibleTerrain);
       
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public Transform GetTransHead()
    {
        return TransHead;
    }
    public Transform GetTransSpawnTerrainRoll()
    {
        return TransSpawnTerrainRoll;
    }
    public int GetScoreTerrainRoll(TerrainRoll terrainRoll)
    {
        if(terrainRoll == null)
        {
            return 0;
        }
        float phanTramDatDuoc = (terrainRoll.GetSizeCurrent() - terrainRoll.GetMinSize()) / (terrainRoll.GetMaxSize() - terrainRoll.GetMinSize());
        int Score = (int)(phanTramDatDuoc * settingShovelCurrent.ScoreMax);
        if(Score < settingShovelCurrent.ScoreMin) { Score = settingShovelCurrent.ScoreMin; }
        return Score;
    }
    public int GetScoreMax()
    {
        return settingShovelCurrent.ScoreMax;
    }
    public enum TypeShovel
    {
        Normal,
        Gold
    }
    public enum StateRoll
    {
        Roll,
        DontRoll
    }
}
