using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InCharacter : MonoBehaviour
{
    [SerializeField] IsActive isActiveNamePlayer_Ending;
    [SerializeField] IsActive isActiveUICurrent_Start;
    [SerializeField] IsActive isActiveScorePlayer_Start;
    [SerializeField] private Transform transUIPlayer;
    [SerializeField] private UI_NamePlayer UI_NamePlayer;
    [SerializeField] private UI_CurrentPlayer UI_CurrentPlayer;
    [SerializeField] private UI_ScorePlayer UI_ScorePlayer;
    [SerializeField] private UI_FullPlayer UI_FullPlayer;
    [SerializeField] private UI_ThrillingPlayer UI_ThrillingPlayer;
    [SerializeField] private UI_ScoreSpiralRollPlayer UI_ScoreSpiralRollPlayer;

    private void Awake()
    {
        if (UI_CurrentPlayer == null)
        {
            UI_CurrentPlayer = (UI_CurrentPlayer)Instantiate(Resources.Load(PathSettings.PathAssetPrefab_UI_CurrentPlayer, typeof(UI_CurrentPlayer)), transUIPlayer);
            UI_CurrentPlayer.Close();
        }
        if (UI_NamePlayer == null)
        {
            UI_NamePlayer = (UI_NamePlayer)Instantiate(Resources.Load(PathSettings.PathAssetPrefab_UINamePlayer, typeof(UI_NamePlayer)), transUIPlayer);
            UI_NamePlayer.Close();
        }
        if (UI_ScorePlayer == null)
        {
            UI_ScorePlayer = (UI_ScorePlayer)Instantiate(Resources.Load(PathSettings.PathAssetPrefab_UI_ScorePlayer, typeof(UI_ScorePlayer)), transUIPlayer);
            UI_ScorePlayer.Close();
        }
        if(UI_ScoreSpiralRollPlayer == null)
        {
            UI_ScoreSpiralRollPlayer = (UI_ScoreSpiralRollPlayer)Instantiate(Resources.Load(PathSettings.PathAssetPrefab_UI_ScoreSpiralRollPlayer, typeof(UI_ScoreSpiralRollPlayer)), transUIPlayer);
            UI_ScoreSpiralRollPlayer.Close();
        }
        if(UI_FullPlayer == null)
        {
            UI_FullPlayer = (UI_FullPlayer)Instantiate(Resources.Load(PathSettings.PathAssetPrefab_UI_FullPlayer, typeof(UI_FullPlayer)), transUIPlayer);
            UI_FullPlayer.Close();
        }
        if (UI_ThrillingPlayer == null)
        {
            UI_ThrillingPlayer = (UI_ThrillingPlayer)Instantiate(Resources.Load(PathSettings.PathAssetPrefab_UIThrilling, typeof(UI_ThrillingPlayer)), transUIPlayer);
            UI_ThrillingPlayer.Close();
        }
        //switch (isActiveNamePlayer_Start)
        //{
        //    case IsActive.True:
        //        Open_NamePlayer();
        //        break;
        //    case IsActive.False:
        //        Close_NamePlayer();
        //        break;
        //}
        switch (isActiveUICurrent_Start)
        {
            case IsActive.True:
                Open_UICurrent();
                break;
            case IsActive.False:
                Close_UICurrent();
                break;
        }
        switch (isActiveScorePlayer_Start)
        {
            case IsActive.True:
                Open_ScorePlayer();
                break;
            case IsActive.False:
                Close_ScorePlayer();
                break;
        }
    }
    private void Start()
    {
        EnventManager.AddListener(EventName.StartGame.ToString(), Open_ScorePlayer);
        if (isActiveNamePlayer_Ending == IsActive.True)
        {
            EnventManager.AddListener(EventName.EndGame.ToString(),
         () =>
         {
             Open_NamePlayer();
             UI_NamePlayer.LoadName(UI_NamePlayer.ModeLoad.Load_NamePlayerMain);
         }
         );
            EnventManager.AddListener(EventName.WatchRevivalDone.ToString(), () => { Close_NamePlayer(); });
        }

    }
    public void Open_NamePlayer()
    {
        UI_NamePlayer.Open();
    }
    public void Open_UICurrent()
    {
        UI_CurrentPlayer.Open();
    }
    public void Open_ScorePlayer()
    {
        UI_ScorePlayer.Open();
    }
    public void Close_NamePlayer()
    {
        UI_NamePlayer.Close();
    }
    public void Close_UICurrent()
    {
        UI_CurrentPlayer.Close();
    }
    public void Close_ScorePlayer()
    {
        UI_ScorePlayer.Close();
    }
    public void Open_UIFull()
    {
        UI_FullPlayer.Open();
    }
    public void Close_UIFull()
    {
        UI_FullPlayer.Close();
    }
    public void Open_Thrilling(ThrillingManager.NameThrilling nameThrilling)
    {
        UI_ThrillingPlayer.Open();
        UI_ThrillingPlayer.PlayThrilling(nameThrilling);
    }
    public void Close_Thrilling()
    {
        UI_ThrillingPlayer.Close();
    }
    public UI_ScoreSpiralRollPlayer Open_UIScoreSpiralTerrainRoll()
    {
        UI_ScoreSpiralRollPlayer.Open();
        return UI_ScoreSpiralRollPlayer;
    }
    public void Close_UIScoreSpiralTerrainRoll()
    {
        UI_ScoreSpiralRollPlayer.Close();
    }

}
public enum IsActive
{
    True,
    False
}
