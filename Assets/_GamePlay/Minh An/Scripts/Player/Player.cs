using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private CharacterController characterController;
    private StatePlayer stateCurrent = StatePlayer.Idle;
    private Vector3 DiractionMove = Vector3.zero;
    private Vector3 vt_Gravity;

    private void Awake()
    {
        OnInIt();
    }
    public override void Start()
    {
        base.Start();
        EnventManager.AddListener(EventName.StartGame.ToString(), () => 
        {
            myTransform.eulerAngles = Vector3.zero;
        });
        EnventManager.AddListener(EventName.ChangeSkinOnPlayer.ToString(), () => 
        { 
            ChangeSkinModel();
        });
        EnventManager.AddListener(EventName.ChangeShovelOnPlayer.ToString(), ChangeShovelModel);
        EnventManager.AddListener(EventName.ChangeBackPackPlayer.ToString(), ChangeBackPackModel);
        ChangeSkinModel();
    }
    public override void OnInIt()
    {
        base.OnInIt();
    }
    public override void Update()
    {
        base.Update();
        ActionMain();
    }
  
    private void ActionMain()
    {
        switch (statePlayer)
        {
            case StatePlayer.Idle:
                if (GetTerrainRoll() != null)
                {
                    GetRuntimeCircleClipper().GetDestructibleTerrain()?.ActiveActionComplete();
                }
                Idle();
               // GetTerrainRoll()?.StopGrowth();
                break;
            case StatePlayer.Move:
                if (IsStop())
                {
                    return;
                }
                if (isIgnoreMove)
                {
                    break;
                }
                Move();
                break;
            case StatePlayer.Victory:
                Victory();
                break;
            case StatePlayer.Die:
                Die();
                break;
            case StatePlayer.Standing_Up:
                Standing_Up();
                break;
            case StatePlayer.Fall:
                if (GetTerrainRoll() != null)
                {
                    GetRuntimeCircleClipper().GetDestructibleTerrain()?.ActiveActionComplete();
                }
              
                Fall();
                break;

        }

        if (!isIgnoreMove)
        {
            if (Input.GetMouseButtonDown(0) && stateCurrent != StatePlayer.Move)
            {
                statePlayer = StatePlayer.Move;
                stateCurrent = statePlayer;
            }
            if (Input.GetMouseButtonUp(0) && stateCurrent != StatePlayer.Idle)
            {
                stateCurrent = StatePlayer.Idle;
                statePlayer = StatePlayer.Idle;
            }
        }
        if(statePlayer != StatePlayer.Die)
        {
            Gravity();
        }
       
    }
    public override void ActionRoll()
    {
        base.ActionRoll();
        UI_ScoreSpiralRollPlayer uI_ScoreSpiralRollPlayer = UI_inCharacter.Open_UIScoreSpiralTerrainRoll();
        uI_ScoreSpiralRollPlayer.LoadTextScoreUI(myShovel.GetScoreTerrainRoll(terrainRoll));
        uI_ScoreSpiralRollPlayer.LoadColorIconThisSUI(nameColorThis);
    }
    public override void ChangeSkinModel(NameColor nameColor = NameColor.None)
    {
        ProfileManager profileManager = ProfileManager.Instance;
        int currentModelID = profileManager.playerData.GetCurrentSkinID();
        Rarity currentModelRarity = profileManager.playerData.GetCurrentSkinRarity();
        ModelAbleObject modelData = profileManager.profileDataConfig.modelAbleObjData;
        ModelAbleObj modelAbleObj = modelData.GetModelSkin(currentModelID, currentModelRarity);
        skinLoad.LoadSkin(modelAbleObj, ChangeSettingSkin);
        ModelSkinRoot modelRoot = GetComponentInChildren<ModelSkinRoot>();
        ChangeModelRoot(modelRoot);
        ChangeBackPackModel();
        ChangeShovelModel();
        ModelSkinAbleObj modelSkin = modelAbleObj as ModelSkinAbleObj;
        nameColorThis = modelSkin.color;
        base.ChangeSkinModel(nameColor);
    }
    public override void Fall()
    {
        base.Fall();
        if (m_timeCheckCompleteFall <= m_TimeCompleteFall)
        {
            m_timeCheckCompleteFall += Time.deltaTime;
        }
        else
        {
            m_timeCheckCompleteFall = 0;
            statePlayer = StatePlayer.Standing_Up;
        }
    }
    public override void Standing_Up()
    {
        base.Standing_Up();
        if (m_timeCheckCompleteStandingUp <= m_TimeCompleteStateStandingUp)
        {
            m_timeCheckCompleteStandingUp += Time.deltaTime;
        }
        else
        {
            m_timeCheckCompleteStandingUp = 0;
            statePlayer = StatePlayer.Idle;
            isIgnoreMove = false;
            SetSpeed(SpeedMax);
            StartCoroutine(IE_Singtelon.IE_DelayAction(() => { ActiveAttack(true); }, 5));
        }
    }
    public override void ChangeShovelModel()
    {
        base.ChangeShovelModel();
        ProfileManager profileManager = ProfileManager.Instance;
        int currentModelID = profileManager.playerData.GetCurrentShovelID();
        Rarity currentModelRarity = profileManager.playerData.GetCurrentShovelRarity();
        ModelAbleObject modelData = profileManager.profileDataConfig.modelAbleObjData;
        ModelAbleObj modelAbleObj = modelData.GetModelShovel(currentModelID, currentModelRarity);
        skinLoad.LoadShovel(modelAbleObj);
    }
    public override void SetSpeed(float newSpeed)
    {
        base.SetSpeed(newSpeed);
        Speed = newSpeed;
        Speed = Mathf.Clamp(Speed, SpeedMin, SpeedMax);
    }
    public override void Victory()
    {
        base.Victory();
    }

    public override void Die()
    {
        base.Die();
        GameManager.Instance.Ending(GameManager.TypeEnding.Lose);
        isIgnoreMove = true;
    }
    public override void Move()
    {
        if (characterController == null || !characterController.enabled || GameManager.Instance.IsEnding()) { return; }
        base.Move();
        //position
        Vector3 DiractionJoystiock = Canvas_JOYSTICK.Share_CANVASJOYSTICK.Get_Diraction();
        DiractionMove.x = DiractionJoystiock.x;
        DiractionMove.z = DiractionJoystiock.z;
        characterController.Move(DiractionMove * Speed * Time.deltaTime);
        //rotation
        if (DiractionJoystiock.magnitude != 0)
        {
            myTransform.rotation = Quaternion.LookRotation(DiractionJoystiock);
        }
    }
    public override void SetPosition(Vector3 newPosition)
    {
        base.SetPosition(newPosition);
        characterController.enabled = false;
        myTransform.position = newPosition;
        characterController.enabled = true;
    }
    private void Gravity()
    {
        if(characterController == null || !characterController.enabled) {return;}
        if(vt_Gravity.y < ct_Gravity)
        {
            vt_Gravity.y = 0;
        }
        vt_Gravity.y += ct_Gravity * Time.deltaTime;
        characterController.Move(vt_Gravity * Speed * Time.deltaTime);
    }
    public override void ChangeBackPackModel()
    {
        base.ChangeBackPackModel();
        ProfileManager profileManager = ProfileManager.Instance;
        int currentModelID = profileManager.playerData.GetCurrentBackPackID();
        Rarity currentModelRarity = profileManager.playerData.GetCurrentBackPackRarity();
        ModelAbleObject modelData = profileManager.profileDataConfig.modelAbleObjData;
        ModelAbleObj modelAbleObj = modelData.GetModelBackPack(currentModelID, currentModelRarity);
        skinLoad.LoadBackPack(modelAbleObj, ChangeSettingBackPack);
    }
    //void updatedd() {
    //    Player player = new Player();
    //    player.onCallBackChange += changeNUmber;
    //}
    //void changeNUmber() { }
    //public delegate void OnCallBack();
    //public OnCallBack onCallBackChange;
    //public void Callback() {
    //    if (onCallBackChange != null)
    //        onCallBackChange.Invoke();
    //}
}
public enum StatePlayer
{
    Idle,
    Move,
    Victory,
    AI_TagetDesraction_BuildStreet,
    Die,
    Standing_Up,
    Fall
}