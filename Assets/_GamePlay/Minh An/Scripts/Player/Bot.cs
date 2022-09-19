using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    private float TimeCoolDownResetTagetNewTerrain = 5;
    [SerializeField] private NavMeshAgent meshAgent_AI;
    [SerializeField] private CharacterController characterThis;
    private StatePlayer stateCurrent = StatePlayer.Idle;
    private DestructibleTerrain destinationTaget;
    private float m_time = 0;
    public Step_ StepTaget;
    private List<Distance> distancesTaget = new List<Distance>();
    private int indexTagetInDistance = 0;
    private int point_Threshold_Score_Active_BuildStreet = -1;
    private Vector3 vt_Gravity;

    private void Awake()
    {
        OnInIt();
    }
    public override void OnInIt()
    {
        base.OnInIt();
        indexTagetInDistance = 0;
    }
    public override void Start()
    {
        base.Start();
        EnventManager.AddListener(EventName.BotContinue.ToString(), () =>
        {
            if (statePlayer != StatePlayer.AI_TagetDesraction_BuildStreet)
            {
                isIgnoreMove = false;
                SetStatePlayer(StatePlayer.Move);
            }

        });
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
                if(stateCurrent != StatePlayer.Idle)
                {
                    stateCurrent = StatePlayer.Idle;
                }
               
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
                stateCurrent = StatePlayer.Move;
                break;
            case StatePlayer.Victory:
                Victory();
                if (stateCurrent != StatePlayer.Victory)
                {
                    stateCurrent = StatePlayer.Victory;
                }

                break;
            case StatePlayer.AI_TagetDesraction_BuildStreet:
                if (stateCurrent != StatePlayer.AI_TagetDesraction_BuildStreet)
                {
                    stateCurrent = StatePlayer.AI_TagetDesraction_BuildStreet;
                    //AI_TagetDesraction_BuildStreet();
                    LoadStepTaget();
                   // ActiveBuildStreet(true);
                }
                AI_TagetDesraction_BuildStreet();
                break;
            case StatePlayer.Die:
                Die();
                break;
            case StatePlayer.Fall:
                if (GetTerrainRoll() != null)
                {
                    GetRuntimeCircleClipper().GetDestructibleTerrain()?.ActiveActionComplete();
                }
               
                Fall();
                break;
            case StatePlayer.Standing_Up:
                Standing_Up();
                break;
        }
        if (!IsStop() && !isIgnoreMove)
        {
            if (!GameManager.Instance.IsEnding())
            {
                if (statePlayer != StatePlayer.AI_TagetDesraction_BuildStreet && stateCurrent != StatePlayer.Victory)
                {
                    CheckAndLoadDestinationTaget();
                    Check_Status_AndActiveBuildStreet();
                }
            }
        }
       
        Gravity();
        if (GameManager.Instance.IsEnding() && stateCurrent != StatePlayer.Victory && stateCurrent != StatePlayer.AI_TagetDesraction_BuildStreet)
        {
            SetStatePlayer(StatePlayer.Idle);
        }
    }
    public override void ChangeShovelModel()
    {
        base.ChangeShovelModel();
        ModelAbleObj modelAbleObj = ProfileManager.Instance.skinBotManager.GetShovelRandom();
        skinLoad.LoadShovel(modelAbleObj);
    }
    public override void Fall()
    {
        base.Fall();
        ActiveNavAI(false);
        if (m_timeCheckCompleteFall <= m_TimeCompleteFall)
        {
            m_timeCheckCompleteFall += Time.deltaTime;
        }
        else
        {
            m_timeCheckCompleteFall = 0;
            SetStatePlayer(StatePlayer.Standing_Up);
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
            SetStatePlayer(stateCurrent);
            isIgnoreMove = false;
            StartCoroutine(IE_Singtelon.IE_DelayAction(() => 
            { 
                if(statePlayer != StatePlayer.AI_TagetDesraction_BuildStreet)
                {
                    ActiveAttack(true);
                }
               
            }, 5));
            ActiveNavAI(true);
        }
    }
    private void Check_Status_AndActiveBuildStreet()
    {
        if(point_Threshold_Score_Active_BuildStreet == -1)
        {
            point_Threshold_Score_Active_BuildStreet = (int)(GetBagTerrainRoll().GetSettingBag().amountScoreMax * Random.Range(0.2f, 1.0f));
           
        }
        if (GetBagTerrainRoll().GetScoreTerrainRollInBag() > point_Threshold_Score_Active_BuildStreet)
        {
            SetStatePlayer(StatePlayer.AI_TagetDesraction_BuildStreet);
        }
    }
    public override void SetSpeed(float newSpeed)
    {
        base.SetSpeed(newSpeed);
        Speed = newSpeed;
        Speed = Mathf.Clamp(Speed, SpeedMin, SpeedMax);
        meshAgent_AI.speed = Speed;
    }

    public override void Die()
    {
        base.Die();
    }
    private void LoadStepTaget()
    {
        List<Step_> steps = new List<Step_>();
        bool isStepTagetInLevelTaget = StepController.Instance.CheckStepInLevel(StepTaget, GameManager.Instance.GetLevelCharacterInGame(this) + 1);
        if (StepTaget == null || !isStepTagetInLevelTaget)
        {
           // steps = StepController.Instance.GetStep_s_NoneColor(GameManager.Instance.GetLevelCharacterInGame(this) + 1);
            foreach(Step_ step in StepController.Instance.GetStep_s_NoneColor(GameManager.Instance.GetLevelCharacterInGame(this) + 1))
            {
                steps.Add(step);
            }

            if (steps.Count > 0)
            {
                StepTaget = steps[Random.Range(0, steps.Count)];
            }
            else
            {
                StepTaget = null;
                steps = StepController.Instance.GetStep_s_All(GameManager.Instance.GetLevelCharacterInGame(this) + 1);
                foreach (Step_ step_ in steps)
                {
                    if (step_.nameColorCurrent == nameColorThis)
                    {
                        StepTaget = step_;
                        break;
                    }
                }
                if (StepTaget == null && steps != null && steps.Count > 0)
                {
                    StepTaget = steps[Random.Range(0, steps.Count)];
                }
            }

        }
        else
        {
            StepTaget = null;
            foreach (Step_ step in StepController.Instance.GetStep_s_NoneColor(GameManager.Instance.GetLevelCharacterInGame(this) + 1))
            {
                steps.Add(step);
            }
            foreach (Step_ step_ in steps)
            {
                if (step_.nameColorCurrent == nameColorThis)
                {
                    StepTaget = step_;
                    break;
                }
            }
            if (StepTaget == null)
            {
                if (steps.Count > 0)
                {
                    StepTaget = steps[Random.Range(0, steps.Count)];
                }
                else
                {
                    steps = StepController.Instance.GetStep_s_All(GameManager.Instance.GetLevelCharacterInGame(this) + 1);
                    foreach (Step_ step_ in steps)
                    {
                        if (step_.nameColorCurrent == nameColorThis)
                        {
                            StepTaget = step_;
                            break;
                        }
                    }
                    if (StepTaget == null)
                    {
                        StepTaget = steps[Random.Range(0, steps.Count)];
                    }
                }
            }
            
           
        }
    }
    public override void SetPosition(Vector3 newPosition)
    {
        base.SetPosition(newPosition);
        characterThis.enabled = false;
        myTransform.position = newPosition;
        characterThis.enabled = true;
    }
    private void AI_TagetDesraction_BuildStreet()
    {
        if (meshAgent_AI.enabled && indexTagetInDistance > 0)
        {
            ActiveNavAI(false);
            ActiveAttack(false);
        }
        if(StepTaget == null)
        {
            LoadStepTaget();
        } 
        distancesTaget = StepTaget.GetDistances();
        Vector3 pointTaget = distancesTaget[indexTagetInDistance].myTransform.position;
        if (StepTaget.GetNameColor_Complete() == nameColorThis)
        {
            pointTaget = StepTaget.GetTransPointComplete().position;
        }
        pointTaget.y = myTransform.position.y;
        if (indexTagetInDistance == 0 && meshAgent_AI.enabled)
        {
            pointTaget.x = StepTaget.GetTransformGate().position.x;
            pointTaget.z = StepTaget.GetTransformGate().position.z;
            meshAgent_AI.SetDestination(pointTaget);

            if(Vector3.Distance(myTransform.position, pointTaget) <= 0.5f)
            {
                if (indexTagetInDistance < distancesTaget.Count - 1 && GetBagTerrainRoll().GetScoreTerrainRollInBag() > distancesTaget[indexTagetInDistance].GetScoreBuild())
                {
                    indexTagetInDistance++;
                }
                else if (StepTaget.GetNameColor_Complete() == nameColorThis)
                {
                    indexTagetInDistance = 0;
                    StepTaget = null;
                    SetStatePlayer(StatePlayer.Move);
                }
                else
                {
                    //  ActiveBuildStreet(false);
                    point_Threshold_Score_Active_BuildStreet = -1;
                    indexTagetInDistance--;
                    if (indexTagetInDistance < 0)
                    {
                        indexTagetInDistance = 0;
                        StepTaget = null;
                        SetStatePlayer(StatePlayer.Move);
                    }
                }
            }
        }
        else
        {
            if (Vector3.Distance(myTransform.position, pointTaget) >= 0.1f)
            {
                LookAt(pointTaget);
                // myTransform.Translate(Vector3.forward * Speed * Time.deltaTime);
                if (!characterThis.enabled)
                {
                    ActiveCharacter(true);
                }
                characterThis.Move(myTransform.forward * Speed * Time.deltaTime);
            }
            else
            {
                if (StepTaget.GetNameColor_Complete() != nameColorThis)
                {
                    if (indexTagetInDistance < distancesTaget.Count - 1 && GetBagTerrainRoll().GetScoreTerrainRollInBag() > distancesTaget[indexTagetInDistance].GetScoreBuild())
                    {
                        indexTagetInDistance++;
                    }
                    else
                    {
                        point_Threshold_Score_Active_BuildStreet = -1;
                       // ActiveBuildStreet(false);
                        indexTagetInDistance--;
                        if (indexTagetInDistance < 0)
                        {
                            indexTagetInDistance = 0;
                            StepTaget = null;
                            SetStatePlayer(StatePlayer.Move);
                        }
                    }
                }
                else
                {
                    pointTaget.y = StepTaget.GetTransPointComplete().position.y;
                    if (Vector3.Distance(StepTaget.GetTransPointComplete().position, pointTaget) <= 0.1f)
                    {
                        indexTagetInDistance = 0;
                        StepTaget = null;
                        SetStatePlayer(StatePlayer.Move);
                    }
                }
            }

        }
        myAnimPlayer.PlayAnim(AnimPlayer.ActionAnim.Run);
    }
   
    public override void Victory()
    {
        base.Victory();
        ActiveNavAI(false);
    }
    public override void Move()
    {
        base.Move();
        if (!meshAgent_AI.enabled)
        {
            ActiveNavAI(true);
            ActiveAttack(true);
        }
        if (meshAgent_AI.remainingDistance <= meshAgent_AI.stoppingDistance)
        {
            if (destinationTaget != null)
            {
                Vector3 vt_Taget = Vector3.zero;
                vt_Taget.x = Random.Range(destinationTaget.myTransform.position.x, destinationTaget.myTransform.position.x + destinationTaget.resolutionX);
                vt_Taget.y = destinationTaget.myTransform.position.y;
                vt_Taget.z = Random.Range(destinationTaget.myTransform.position.z, destinationTaget.myTransform.position.z + destinationTaget.resolutionY);
                LoadMeshFollow(vt_Taget);
            }
            else
            {
                statePlayer = StatePlayer.Idle;
            }
        }
    }
    public override void Idle()
    {
        base.Idle();
    }
    public void CheckAndLoadDestinationTaget()
    {
        if (destinationTaget == null)
        {
            int indexDestinationTaget = 0;
            if (TerrainMapController.Instance.GetLevelMaxInMap() > 0)
            {
                indexDestinationTaget = Random.Range(0, TerrainMapController.Instance.GetLevelMaxInMap() + 1);
            }
            destinationTaget = TerrainMapController.Instance.GetCreateTerrainInMap(GameManager.Instance.GetLevelCharacterInGame(this))?.GetDestructibleTerrains(this.nameColorThis)[indexDestinationTaget];
            if (destinationTaget != null)
            {
                LoadMeshFollow(destinationTaget.myTransform.position);
            }

            m_time = 0;
        }
        else
        {
            if (TerrainMapController.Instance.GetCreateTerrainInMap(GameManager.Instance.GetLevelCharacterInGame(this)) != null)
            {
                if (!TerrainMapController.Instance.GetCreateTerrainInMap(GameManager.Instance.GetLevelCharacterInGame(this))
              .GetDestructibleTerrains(this.nameColorThis).Contains(destinationTaget))
                {
                    destinationTaget = null;
                }
            }
          
            if (m_time < TimeCoolDownResetTagetNewTerrain)
            {
                m_time += Time.deltaTime;
            }
            else
            {
                destinationTaget = null;
                m_time = 0;
            }
        }
    }

    private void ActiveNavAI(bool isActive)
    {
        if (isActive)
        {
            meshAgent_AI.enabled = true;
            characterThis.enabled = false;
        }
        else
        {
            meshAgent_AI.enabled = false;
        }
    }
    private void ActiveCharacter(bool isActive)
    {
        if (isActive)
        {
            meshAgent_AI.enabled = false;
            characterThis.enabled = true;
            
        }
        else
        {
            characterThis.enabled = false;
        }
    }
    private void Gravity()
    {
        if (characterThis == null || !characterThis.enabled) { return; }
        if (vt_Gravity.y < ct_Gravity)
        {
            vt_Gravity.y = 0;
        }
        vt_Gravity.y += ct_Gravity * Time.deltaTime;
        characterThis.Move(vt_Gravity * Speed * Time.deltaTime);
    }

    public override void ChangeSkinModel(NameColor nameColor)
    {
        ModelAbleObj modelAbleObj = ProfileManager.Instance.skinBotManager.GetSkinRandom(nameColor);
        skinLoad.LoadSkin(modelAbleObj, ChangeSettingSkin);
        ModelSkinRoot modelRoot = GetComponentInChildren<ModelSkinRoot>();
        ModelSkinAbleObj modelSkin = modelAbleObj as ModelSkinAbleObj;
        ChangeModelRoot(modelRoot);
        ChangeShovelModel();
        ChangeBackPackModel();
        nameColorThis = modelSkin.color;
        base.ChangeSkinModel(nameColor);
    }
    public void LoadMeshFollow(Vector3 pointTaget)
    {
        ActiveNavAI(true);
        meshAgent_AI.SetDestination(pointTaget);
        SetStatePlayer(StatePlayer.Move);
        // destinationTaget = null;
    }
    public override void ChangeBackPackModel()
    {
        base.ChangeBackPackModel();
        ModelAbleObj modelAbleObj = ProfileManager.Instance.skinBotManager.GetBackPackRandom();
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

