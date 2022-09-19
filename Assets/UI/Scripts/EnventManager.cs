using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum EventName { 
    OutFitOnCustom,
    PetOnCustom, 
    SpiralStartCount,
    WatchRevivalDone,
    BotContinue,
    LoadNamePlayer,
    StartGame,
    IncreaseCountSpinWatchVideo,
    SaveTimeWatchVideo,
    CanGetReward,
    OffTimePlayReward,
    ChangeSkinOnPlayer,
    ChangeShovelOnPlayer,
    ChangeBackPackPlayer,
    LevelUpPlayerInMap,
    DistanceSetNewColor,
    DisableBtnSelect,
    ActivateBtnSelect,
    UpdateModelRootPreview,
    ChangeAnimModel,
    UpdateTextScore,
    ChangeGem,
    ChangeLevel,
    EndGame
}
public class EnventManager : GenerticSingleton<EnventManager>
{
    //public static EnventManager instance;
    //private void Awake()
    //{
    //    if (instance != null)
    //        Destroy(gameObject);
    //    else instance = this;
    //}

    private Dictionary<string, UnityEvent> eventDic = new Dictionary<string, UnityEvent>();

    public static void AddListener(string eventName, UnityAction action) {
        UnityEvent thisEvent = null;
        if (Instance.eventDic.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(action);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(action);
            Instance.eventDic.Add(eventName, thisEvent);
        }
    }
    public static void TriggerEvent(string name)
    {
        UnityEvent thisEvent = null;
        if (Instance)
        {
            if (Instance.eventDic.TryGetValue(name, out thisEvent))
            {
                thisEvent.Invoke();
            }
        }
    }

}
