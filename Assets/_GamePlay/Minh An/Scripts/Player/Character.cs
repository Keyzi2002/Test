using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : MonoBehaviour
{
    public NameColor nameColorThis;
    public StatePlayer statePlayer = StatePlayer.Idle;
    public float Speed;
    public float SpeedMin;
    public float SpeedMax;
    public float friction = 0.01f;
    public bool isIgnoreMove = false;
    public const float ct_Gravity = -5;
    private float m_Speed;
    public Transform myTransform;
    public SkinnedMeshRenderer meshRendererThis;
    public Shovel myShovel;
    protected TerrainRoll terrainRoll;
    protected BagTerrainRoll bagTerrain;
    public RuntimeCircleClipper runtimeCircleClipper;
    public SkinLoadData skinLoad;
    private BuildStreet buildStreet;
    protected AnimPlayer myAnimPlayer;
    private Attack attack;
    protected UI_InCharacter UI_inCharacter;
    private float TimeFreeFall_Die = 1;
    private float m_TimeCheckFreeFall = 0;
    public static float m_TimeCompleteFall = 1;
    public static float m_TimeCompleteStateStandingUp = 11.12f / 4;
    protected StatePlayer statePlayerFWD = StatePlayer.Idle;
    protected float m_timeCheckCompleteFall = 0;
    protected float m_timeCheckCompleteStandingUp = 0;
    private bool isStop;
   
    public virtual void OnInIt()
    {
        if(myTransform == null) { myTransform = this.transform; }
        if(myShovel == null) { myShovel = GetComponentInChildren<Shovel>(); }
        if(runtimeCircleClipper == null) { runtimeCircleClipper = GetComponentInChildren<RuntimeCircleClipper>(); }
        if(attack == null) { attack = GetComponentInChildren<Attack>(); }
        if(UI_inCharacter == null) { UI_inCharacter = GetComponent<UI_InCharacter>(); }
        terrainRoll = null;
        m_Speed = Speed;
        isIgnoreMove = false;
        myTransform.eulerAngles = Vector3.up * 180;
        GameManager.Instance.SetActionStartGame(Stop);
        if(myAnimPlayer == null) { myAnimPlayer = GetComponent<AnimPlayer>(); }
      
    }
   
    private void ResetThis()
    {
        statePlayer = StatePlayer.Idle;
        OnInIt();
    }
    public virtual void Start()
    {
        //LoadColor();
        //EnventManager.AddListener(EventName.Ending_Player_NotDie.ToString(), () =>
        //{
        //    myShovel.Close();
        //    GetBagTerrainRoll().CloseBagModel();
        //});

    }
    public void ActiveAttack(bool isActive)
    {
        attack.gameObject.SetActive(isActive);
    }
    public virtual void Move()
    {
        myAnimPlayer.PlayAnim(AnimPlayer.ActionAnim.Run);
    }
    public bool IsStop()
    {
        return isStop;
    }
    public virtual void Standing_Up()
    {
        isIgnoreMove = true;
        myAnimPlayer.PlayAnim(AnimPlayer.ActionAnim.StandingUp);
       
    }
    public virtual void Fall()
    {
        isIgnoreMove = true;
        myAnimPlayer.PlayAnim(AnimPlayer.ActionAnim.Fall);
        ActiveAttack(false);
     
    }
    public void Stop(bool isStop)
    {
        this.isStop = isStop;
    }
    public virtual void Update()
    {
        if (!CheckFreeFall() && statePlayer != StatePlayer.Die)
        {
            m_TimeCheckFreeFall += Time.deltaTime;
            if(m_TimeCheckFreeFall >= TimeFreeFall_Die)
            {
                statePlayer = StatePlayer.Die;
            }
        }
        else
        {
            m_TimeCheckFreeFall = 0;
        }
    }
    public bool CheckFreeFall()
    {
        if(Physics.Raycast(myTransform.position - myTransform.forward * 1.2f, Vector3.down * 2, 2))
        {
            return true;
        }
        return false;
    }
    public virtual void ActionRoll()
    {
        SpeedDown(friction);
        if (terrainRoll == null)
        {
            TerrainRoll terrainRollPrefab = (TerrainRoll)Resources.Load(PathSettings.PathAssetPrefabTerrainRoll, typeof(TerrainRoll));
            terrainRoll = Instantiate(terrainRollPrefab, myShovel.GetTransSpawnTerrainRoll().position, Quaternion.identity, myShovel.GetTransSpawnTerrainRoll());
            terrainRoll.myTransform.localEulerAngles = Vector3.zero;
            terrainRoll.SetTransTagetComplete(GetBagTerrainRoll().transformBag);
            terrainRoll.SetActionComplete(() =>
            {
                //complete
                ActionComplete();
            });
            terrainRoll.CrateMaterial(runtimeCircleClipper.GetDestructibleTerrain().GetNameColor());
            terrainRoll.SetColor(terrainRoll.GetMaterial());
        }
        terrainRoll.StartGrowth();
       
    }
    public virtual void Die()
    {
        myAnimPlayer.PlayAnim(AnimPlayer.ActionAnim.Die);
    }
    public virtual void Victory()
    {
        myTransform.localEulerAngles = Vector3.up * 180;
        myAnimPlayer.PlayAnim(AnimPlayer.ActionAnim.Victory);
        myShovel.Close();
        GetBagTerrainRoll().CloseBagModel();
        UI_inCharacter.Close_ScorePlayer();
        //if (skinLoad != null)
        //{
        //    if (skinLoad.shovelRoot.childCount > 0) Destroy(skinLoad.shovelRoot.GetChild(0).gameObject);
        //    if (skinLoad.backPackRoot.childCount > 0) Destroy(skinLoad.backPackRoot.GetChild(0).gameObject);
        //}
    }
    public void ActionComplete()
    {
        if (terrainRoll != null)
        {
            GetBagTerrainRoll().SetTerrainRoll(terrainRoll);
            terrainRoll.Complete();
            GetBagTerrainRoll().SetScoreTerrainRollInBag_UP(myShovel.GetScoreTerrainRoll(terrainRoll));
            ResetSpeed();
            terrainRoll = null;
        }
        UI_inCharacter.Close_UIScoreSpiralTerrainRoll();
    }
    public virtual void SetPosition(Vector3 newPosition)
    {
        
    }
    public virtual void Idle()
    {
        myAnimPlayer.PlayAnim(AnimPlayer.ActionAnim.Idle);
    }
    public virtual void Rivival()
    {
        statePlayer = StatePlayer.Idle;
        isIgnoreMove = false;
    }
    private void LoadColor()
    {
        Material material = new Material(Shader.Find("Universal Render Pipeline/Simple Lit"));
        switch (nameColorThis)
        {
            case NameColor.Pink:
                material.color = ColorSettings.Instance.color_Pink;
                break;
            case NameColor.Green:
                material.color = ColorSettings.Instance.color_Green;
                break;
            case NameColor.Blue:
                material.color = ColorSettings.Instance.color_Blue;
                break;
            case NameColor.Yeallow:
                material.color = ColorSettings.Instance.color_Yeallow;
                break;
            case NameColor.Violet:
                material.color = ColorSettings.Instance.color_Violet;
                break;
            case NameColor.Orange:
                material.color = ColorSettings.Instance.color_Orange;
                break;
        }
        meshRendererThis.material = material;
    }
    public virtual void SetSpeed(float newSpeed)
    {
       
    }
    public void ResetSpeed()
    {
        Speed = m_Speed;
    }
    public void SpeedDown(float valueDown)
    {
        SetSpeed(Speed - valueDown);
    }
    public TerrainRoll GetTerrainRoll()
    {
        return terrainRoll;
    }
    public BagTerrainRoll GetBagTerrainRoll()
    {
        if (bagTerrain == null)
        {
            bagTerrain = GetComponentInChildren<BagTerrainRoll>();
        }
        return bagTerrain;
    }
    public RuntimeCircleClipper GetRuntimeCircleClipper()
    {
        return runtimeCircleClipper;
    }
    public void LookAt(Vector3 vectorLookAt)
    {
        vectorLookAt.y = myTransform.position.y;
        myTransform.LookAt(vectorLookAt);
    }
    public void ActiveBuildStreet(bool isActive)
    {
        if(buildStreet == null)
        {
            buildStreet = GetComponent<BuildStreet>();
        }
        buildStreet.enabled = isActive;
    }
    public void SetStatePlayer(StatePlayer statePlayer)
    {
        this.statePlayer = statePlayer;
    }
   // protected void LoadCheck load and check state truoc do ma character thuc hien
    public Shovel GetShovel()
    {
        return myShovel;
    }
    public void Active(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
    public virtual void ChangeSettingSkin() {
        myAnimPlayer.ResetAnim();
    }
    public virtual void ChangeModelRoot(ModelSkinRoot modelRoot) {
        skinLoad.backPackRoot = modelRoot.bagPoint;
    }
    public virtual void ChangeSkinModel(NameColor nameColor)
    {
        LoadColor();
    }
    public virtual void ChangeShovelModel()
    {

    }
    public virtual void ChangeBackPackModel()
    {

    }
    public virtual void ChangeSettingBackPack()
    {
        if (myShovel != null)
            myShovel.SetActionShovel();
    }
}
