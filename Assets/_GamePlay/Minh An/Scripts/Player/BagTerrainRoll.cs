using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BagTerrainRoll : MonoBehaviour
{
    public Transform transformBag;
    private Action actionBagFull;
    private Action actionBagNotFull;
    [SerializeField] private Queue<TerrainRoll> terrainRolls = new Queue<TerrainRoll>();
    [SerializeField] private List<Transform> pointTerrains = new List<Transform>();
    [SerializeField] private SettingBag settingBagCurrent;
    [SerializeField] private float SizeBagMax;
    [SerializeField] private float SizeBagMin;
    private int ScoreTerrainRollInBag = 0;
    private float ZOffsetBag;
    private int Score_ActiveNext_TerrainRollInBag;
    private Dictionary<UnityEngine.Transform, UnityEngine.Transform> point_Parent_Child_BagTerrainRoll = new Dictionary<Transform, Transform>();
    private bool isFull;
    private void Awake()
    {
        OnInIt();
    }
    private void OnInIt()
    {
        LoadBag();
        ZOffsetBag = transformBag.localPosition.z;
        Score_ActiveNext_TerrainRollInBag = (settingBagCurrent.amountScoreMax / pointTerrains.Count);

        foreach(Transform transform in pointTerrains)
        {
            if (!point_Parent_Child_BagTerrainRoll.ContainsKey(transform))
            {
                point_Parent_Child_BagTerrainRoll.Add(transform, null);
            }
        }
        isFull = false;
    }

    private void LoadBag()
    {
        settingBagCurrent = DataManager.Instance.GetDataBag().GetSettingBag();
    }
    private void LoadSizeBag()
    {
        Vector3 vt_Size = Vector3.one * ((float)ScoreTerrainRollInBag / settingBagCurrent.amountScoreMax) * SizeBagMax;
       
        vt_Size.x = Mathf.Clamp(vt_Size.x, SizeBagMin, SizeBagMax);
        vt_Size.y = Mathf.Clamp(vt_Size.y, SizeBagMin, SizeBagMax);
        vt_Size.z = Mathf.Clamp(vt_Size.z, SizeBagMin, SizeBagMax);
        transformBag.localScale = vt_Size;
        transformBag.localPosition = Vector3.forward * ((transformBag.localScale.x - SizeBagMin) / 2 * ZOffsetBag + ZOffsetBag) + 
            Vector3.up * (transformBag.localScale.y - SizeBagMin);
    }
    public SettingBag GetSettingBag()
    {
        return settingBagCurrent;
    }
    public void CloseBagModel()
    {
        transformBag.gameObject.SetActive(false);
    }
    public void OpenBagModel()
    {
        transformBag.gameObject.SetActive(true);
    }
    public void SetTerrainRoll(TerrainRoll terrainRoll)
    {
        if (terrainRolls.Count < pointTerrains.Count && terrainRoll.GetVlueSpinShaderMaterial() > 10f)
        {
            if (terrainRolls.Contains(terrainRoll))
            {
                return;
            }
            terrainRolls.Enqueue(terrainRoll);
            terrainRoll.SeetActionCompleteAnim(() =>
            {
                foreach(Transform transform in point_Parent_Child_BagTerrainRoll.Keys)
                {
                    if(point_Parent_Child_BagTerrainRoll[transform] == null)
                    {
                        if (!point_Parent_Child_BagTerrainRoll.ContainsValue(terrainRoll.myTransform))
                        {
                            point_Parent_Child_BagTerrainRoll[transform] = terrainRoll.myTransform;
                            terrainRoll.myTransform.SetParent(transform);
                        }
                        
                        break;
                    }
                }
                terrainRoll.myTransform.localPosition = Vector3.zero;
                terrainRoll.myTransform.localScale = Vector3.one * 0.25f + Vector3.forward * 1.7f;
                terrainRoll.myTransform.localEulerAngles = Vector3.right * 90;
                terrainRoll.SetSpinAndLoadsharedMaterial(31);
            });
         
        }
        else
        {
            terrainRoll.SeetActionCompleteAnim(() =>
            {
                terrainRoll.On_Destroy();
            });
        }
    }
    public TerrainRoll GetTerrainRoll()
    {
        return terrainRolls.Dequeue();
    }
    public int GetScoreTerrainRollInBag()
    {
        return ScoreTerrainRollInBag;
    }
    public void SetScoreTerrainRollInBag_UP(int valueUp)
    {
        ScoreTerrainRollInBag += valueUp;
        ScoreTerrainRollInBag = Mathf.Clamp(ScoreTerrainRollInBag, 0, settingBagCurrent.amountScoreMax);
        EnventManager.TriggerEvent(EventName.UpdateTextScore.ToString());
        if(valueUp < 0 && (Score_ActiveNext_TerrainRollInBag * terrainRolls.Count - Score_ActiveNext_TerrainRollInBag >= ScoreTerrainRollInBag || ScoreTerrainRollInBag < 10) && terrainRolls.Count > 0)
        {
            Transform transformTreeainRoll = terrainRolls.Dequeue().myTransform;
            foreach (Transform transform in point_Parent_Child_BagTerrainRoll.Keys)
            {
                if (point_Parent_Child_BagTerrainRoll[transform] == transformTreeainRoll)
                {
                    point_Parent_Child_BagTerrainRoll[transform] = null;
                    break;
                }
            }
            
            Destroy(transformTreeainRoll.gameObject);
        }
        CheckScoreInBag();
       // LoadSizeBag();
    }
    public void CheckScoreInBag()
    {
        if(ScoreTerrainRollInBag >= settingBagCurrent.amountScoreMax && !isFull)
        {
            isFull = true;
#if UNITY_EDITOR
            Debug.Log("FULL SCORE IN BAG");
#endif
            actionBagFull?.Invoke();
            GameManager.Instance.PlayNoti(GetComponentInParent<Character>(), GameManager.TypeCall_NPCNoti.BagFull, 3);
        }
        else if(ScoreTerrainRollInBag < settingBagCurrent.amountScoreMax && isFull)
        {
            isFull = false;
            actionBagNotFull?.Invoke();
        }
    }
  
    public void SetActionBagFull(Action action)
    {
        actionBagFull += action;
    }
    public void SetActionBagNotFull(Action action)
    {
        actionBagNotFull += action;
    }
}
