using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class GameManager : Singleton<GameManager>
{
    public static int GemUpInOneLevel = 100;
    public static int GemReward_CompleteLevelDefault = 200;
    [SerializeField] private List<NameColor> nameColor_UseInPlayer_InGames = new List<NameColor>();
    [SerializeField] private List<Character> characters_InGame = new List<Character>();
    [SerializeField] private NPC_Notion NPC_Noti;
    [SerializeField] private int amountCharacter;
    // [SerializeField] private Text txt;
    private Character characterBotPrefab;
    public Action<bool> actionStartGame;
    private Dictionary<Character, int> progressPlayer = new Dictionary<Character, int>(); // value la level hien tai tren map
    private bool isEnding;
    private GameObject LevelTaget;
    public WheelManager wheelManager = new WheelManager();
    public TimeCoolDownManager timeCoolDownManager = new TimeCoolDownManager();
    public override void Awake()
    {
        OnInIt();
    }
    public void OnInIt()
    {
        LoadLevelData(ProfileManager.Instance.levelData.GetLevel(), 0);
        CloneBot();
        if(NPC_Noti == null) 
        {
            NPC_Notion nPC_NotionPrefab = (NPC_Notion)Resources.Load(PathSettings.PathAssetPrefabNPC_Noti, typeof(NPC_Notion));
            NPC_Noti = Instantiate(nPC_NotionPrefab);
            NPC_Noti.SetActive(false);
        }
        isEnding = false;
        //StartCoroutine(IE_Singtelon.IE_DelayAction(() => {
        //    StartGame();
        //}, 2));

    }

    private void Update()
    {
        timeCoolDownManager.Update();
    }
    private void CloneBot()
    {
        if (characterBotPrefab == null)
        {
            characterBotPrefab = (Character)Resources.Load(PathSettings.PathAssetPrefabBot, typeof(Character));
        }

        while (characters_InGame.Count < amountCharacter)
        {
            Character characterBot = Instantiate(characterBotPrefab);
            characters_InGame.Add(characterBot);
        }
        List<Transform> transformsPointStart = StartingLineController.Instance.GetTransformsPointPlayerSpawn();
        for (int i = 0; i < characters_InGame.Count; i++)
        {
            characters_InGame[i].SetPosition(transformsPointStart[i].position);
            if (characters_InGame[i] != GetCharacter_Player())
            {
                characters_InGame[i].Active(false);
            }
        }
    }
    private void LoadColorInChacter()
    {
        nameColor_UseInPlayer_InGames.Clear();
        if (nameColor_UseInPlayer_InGames.Count > amountCharacter)
        {
            for (int i = 0; i < nameColor_UseInPlayer_InGames.Count - amountCharacter; i++)
            {
                nameColor_UseInPlayer_InGames.RemoveAt(0);
            }

        }
        else if (nameColor_UseInPlayer_InGames.Count < amountCharacter)
        {
            List<NameColor> nameColorFakes = new List<NameColor>();
            for (int j = 0; j < System.Enum.GetValues(typeof(NameColor)).Length; j++)
            {
                if (!nameColor_UseInPlayer_InGames.Contains((NameColor)System.Enum.GetValues(typeof(NameColor)).GetValue(j)))
                {
                    nameColorFakes.Add((NameColor)System.Enum.GetValues(typeof(NameColor)).GetValue(j));
                }
            }
            nameColorFakes.Remove(NameColor.None);
            nameColorFakes.Remove(GetCharacter_Player().nameColorThis);
            int amount = amountCharacter - nameColor_UseInPlayer_InGames.Count;
            for (int i = 0; i < amount; i++)
            {
                if (characters_InGame[i] != GetCharacter_Player())
                {
                    int valueRandomColor = UnityEngine.Random.Range(0, nameColorFakes.Count);
                    nameColor_UseInPlayer_InGames.Add(nameColorFakes[valueRandomColor]);
                    nameColorFakes.RemoveAt(valueRandomColor);
                    characters_InGame[i].ChangeSkinModel(nameColor_UseInPlayer_InGames[nameColor_UseInPlayer_InGames.Count - 1]);
                }
                else
                {
                    nameColor_UseInPlayer_InGames.Add(GetCharacter_Player().nameColorThis); 
                    //Load in data
                }
            }
        }
        progressPlayer.Clear();
        foreach (Character character in characters_InGame)
        {
            if (!progressPlayer.ContainsKey(character))
            {
                progressPlayer.Add(character, 0);
            }
        }
        for (int i = 0; i < characters_InGame.Count; i++)
        {
            characters_InGame[i].nameColorThis = nameColor_UseInPlayer_InGames[i];
        }
    }
    private void LoadLevelData(int level = 0, int mission = 0)
    {
        //LevelTaget = (GameObject)Resources.Load(PathSettings.PathAssetPrefabLevel + "1");
        Level levelData = ProfileManager.Instance.profileDataConfig.levelDataConfig.levels[level];
        LevelTaget = Instantiate(levelData.missionObjectMap, Vector3.zero, Quaternion.identity);
        NavMesh.AddNavMeshData(levelData.mapNavMesh);
    }
    private void Start()
    {//  txt.text = "â";
        LoadColorInChacter();
        EnventManager.AddListener(EventName.StartGame.ToString(), LoadColorInChacter);
        actionStartGame?.Invoke(true);
        //  txt.text = characters_InGame.Count.ToString();
        EnventManager.AddListener(EventName.StartGame.ToString(), () =>
        {
            foreach (Character character in characters_InGame)
            {
                character.Active(true);
            }
        });
    }
    public void Ending(TypeEnding typeEnding)
    {
        if (!isEnding)
        {
            isEnding = true;
            Canvas_JOYSTICK.Share_CANVASJOYSTICK.Close();
            switch (typeEnding)
            {
                case TypeEnding.Win:
                    StartCoroutine(IE_Singtelon.IE_DelayAction(() => {
                        UIManager.Instance.ShowWinGamePanel();
                    }, 3.8f));
                    ProfileManager.Instance.levelData.LevelUp();
                    break;
                case TypeEnding.Lose:
                    if ((GetCharacter_Player() as Player).statePlayer != StatePlayer.Die)
                    {
                        UIManager.Instance.ShowLosePanel();
                    }
                    else
                    {
                        UIManager.Instance.ShowRevivalPanel();
                    }
                    //StartCoroutine(IE_Singtelon.IE_DelayAction(() => {
                    //    revivalPlayer();
                    //}, 2));

                    break;
            }

            if ((GetCharacter_Player() as Player).statePlayer != StatePlayer.Die)
            {

                CameraManager.Instance.GetVcamController().SetModeTaget(VcamController.ModeTaget.Ending_Win_Lose);
            }
            else
            {
                CameraManager.Instance.GetVcamController().SetModeTaget(VcamController.ModeTaget.Ending_Player_Die);
            }
            EnventManager.TriggerEvent(EventName.EndGame.ToString());
            actionStartGame = null;
        }
    }
    public void StartGame()
    {
        actionStartGame?.Invoke(false);
        EnventManager.TriggerEvent(EventName.StartGame.ToString());
    }
    public void revivalPlayer()
    {
        isEnding = false;
        GetCharacter_Player().SetPosition(ToolFollowGroundCoordinates.Instance.GetPointInGround());
        GetCharacter_Player().Rivival();
        EnventManager.TriggerEvent(EventName.WatchRevivalDone.ToString());
        EnventManager.TriggerEvent(EventName.BotContinue.ToString());
    }
    public bool IsEnding()
    {
        return isEnding;
    }
    public void ReLoadGame()
    {
        LoadGameManager.Instance.ReloadGame();
        CameraManager.Instance.GetVcamController().SetModeTaget(VcamController.ModeTaget.LootAt_Follow_Player_InStartLine);
        UIManager.Instance.ShowStartPanel();
    }
    public List<NameColor> GetNameColors_UseInPlayer_InGame()
    {
        if (nameColor_UseInPlayer_InGames.Count > amountCharacter || nameColor_UseInPlayer_InGames.Count < amountCharacter)
        {
            OnInIt();
        }

        return nameColor_UseInPlayer_InGames;
    }
    public int GetAmountCharacter()
    {
        return amountCharacter;
    }
    public List<Character> GetCharacters_InGame()
    {
        return characters_InGame;
    }
    public Character GetCharacter_Player()
    {
        Character characterPlayer = null;
        foreach (Character character in characters_InGame)
        {
            if ((character as Player) != null)
            {
                characterPlayer = character;
                return characterPlayer;
            }
        }
        if (characterPlayer == null)
        {
            characterPlayer = FindObjectOfType<Player>();
        }
        return characterPlayer;
    }
    public void PlayNoti(Character characterCall, TypeCall_NPCNoti typeCall_NPCNoti, float m_timeCall)
    {
        if(characterCall == GetCharacter_Player())
        {
            NPC_Noti.SetActive(true);
            string str_Call = "";
            switch (typeCall_NPCNoti)
            {
                case TypeCall_NPCNoti.BagFull:
                    str_Call = NPC_Noti.NameNoti_BagFull;
                    break;
                case TypeCall_NPCNoti.NotEnoughPoints:
                    str_Call = NPC_Noti.NameNoti_NotEnoughPoints;
                    break;
            }
            NPC_Noti.PlayNoti(GetCharacter_Player().myTransform, str_Call, m_timeCall);
        }
    }
    public void SetLevelCharacterInGame(Character characterNext, int Level)
    {
        progressPlayer[characterNext] = Level;
        EnventManager.TriggerEvent(EventName.LevelUpPlayerInMap.ToString());
    }
    public int GetLevelCharacterInGame(Character characterCheck)
    {
        if (!progressPlayer.ContainsKey(characterCheck))
        {
            progressPlayer.Add(characterCheck, 0);
        }
        return progressPlayer[characterCheck];
    }
    public void SetActionStartGame(Action<bool> action)
    {
        actionStartGame += action;
    }
    public enum TypeEnding
    {
        Win,
        Lose
    }
    public enum TypeCall_NPCNoti
    {
        BagFull,
        NotEnoughPoints
    }
}