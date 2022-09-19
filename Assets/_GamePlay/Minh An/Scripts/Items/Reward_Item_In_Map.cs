using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reward_Item_In_Map : MonoBehaviour
{
    public Transform myTransform;
    [SerializeField] private RectTransform rectBGReward;
    [SerializeField] private AnimationCurve animationUpDown;
    [SerializeField] private float HeightOffset;
    [SerializeField] private float SpeedAnim;
    [SerializeField] TypeReward typeReward;
    [SerializeField] NameAnim nameAnim;
    [SerializeField] private int valueReward;
    [SerializeField] private Text textUI;
    [SerializeField] private bool isWatchVideo;
    
    private Character characterTriggerReward;
    private void Awake()
    {
        OnInIt();
        switch (nameAnim)
        {
            case NameAnim.None:
                break;
            case NameAnim.UpAndDown:
                StartCoroutine(IE_AnimUI_Reward(HeightOffset));
                break;
        }
       
    }
   
    IEnumerator IE_AnimUI_Reward(float S)
    {
        Vector3 pointStart = rectBGReward.anchoredPosition3D;
        float m_time = 0;
        while(m_time < 1)
        {
            rectBGReward.anchoredPosition3D = pointStart + Vector3.forward * animationUpDown.Evaluate(m_time) * S/2;
            m_time += Time.deltaTime * SpeedAnim;
            yield return null;
        }
        StartCoroutine(IE_AnimUI_Reward(-S));
    }
    private void OnInIt()
    {
        if(myTransform == null) { myTransform = this.transform; }
        LoadUI();
    }
    private void OnTriggerEnter(Collider other)
    {
        characterTriggerReward = Cache.GetComponetCharacter(other);
        if (characterTriggerReward != null)
        {
            if (isWatchVideo)
            {
#if UNITY_EDITOR
                Debug.Log("Watch Video");
#endif
            }
            int valueSetReward = 0;
            switch (typeReward)
            {
                case TypeReward.Add:
                    valueSetReward = valueReward;
                    break;
                case TypeReward.Subtract:
                    valueSetReward = -valueReward;
                    break;
                case TypeReward.Core:
                    valueSetReward = characterTriggerReward.GetBagTerrainRoll().GetScoreTerrainRollInBag() * valueReward - characterTriggerReward.GetBagTerrainRoll().GetScoreTerrainRollInBag();
                    break;
                case TypeReward.Divide:
                    valueSetReward = characterTriggerReward.GetBagTerrainRoll().GetScoreTerrainRollInBag() / valueReward - characterTriggerReward.GetBagTerrainRoll().GetScoreTerrainRollInBag();
                    break;
            }
            StartCoroutine(IE_SpawnTerrainRoll(0.1f, 5, () => { gameObject.SetActive(false); }));
            characterTriggerReward.GetBagTerrainRoll().SetScoreTerrainRollInBag_UP(valueSetReward);
            
        }
    }
    IEnumerator IE_SpawnTerrainRoll(float timeDelaySpawn, int amount, System.Action actionComplete)
    {
        TerrainRoll terrainRollPrefab = (TerrainRoll)Resources.Load(PathSettings.PathAssetPrefabTerrainRoll, typeof(TerrainRoll));
        int m_amount = 0;
        while(m_amount < amount)
        {
            TerrainRoll terrainRoll = Instantiate(terrainRollPrefab, myTransform.position, Quaternion.identity, characterTriggerReward.GetShovel().GetTransSpawnTerrainRoll());
            terrainRoll.myTransform.localRotation = Quaternion.identity;
            terrainRoll.SetTransTagetComplete(characterTriggerReward.GetBagTerrainRoll().transformBag);
            terrainRoll.CrateMaterial(characterTriggerReward.nameColorThis);
            terrainRoll.SetColor(terrainRoll.GetMaterial());
            terrainRoll.SetSpinAndLoadsharedMaterial(31);
            terrainRoll.Complete();
            terrainRoll.myTransform.localScale = Vector3.one;
            characterTriggerReward.GetBagTerrainRoll().SetTerrainRoll(terrainRoll);
            m_amount++;
            yield return Cache.GetWaiforSecond(timeDelaySpawn);
        }
        actionComplete?.Invoke();
    }
    private void LoadUI()
    {
        switch (typeReward)
        {
            case TypeReward.Add:
                textUI.text = "+" + valueReward;
                break;
            case TypeReward.Subtract:
                textUI.text = "-" + valueReward;
                break;
            case TypeReward.Core:
                textUI.text = "x" + valueReward;
                break;
            case TypeReward.Divide:
                textUI.text = "÷" + valueReward;
                break;
        }
    }
    public enum TypeReward
    {
        Add,
        Subtract,
        Core,
        Divide
    }
    public enum NameAnim
    {
        None,
        UpAndDown
    }
}
