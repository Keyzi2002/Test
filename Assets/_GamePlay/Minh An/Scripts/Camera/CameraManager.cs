using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : GenerticSingleton<CameraManager>
{
    [SerializeField] private VcamController Vcam;
    private void Start()
    {
        EnventManager.AddListener(EventName.WatchRevivalDone.ToString(), () => { GetVcamController().SetModeTaget(VcamController.ModeTaget.LootAt_Follow_Player_InGame); });
    }
    public VcamController GetVcamController()
    {
        return Vcam;
    }

}
