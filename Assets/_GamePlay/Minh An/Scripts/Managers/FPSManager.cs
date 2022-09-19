//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class FPSManager : GenerticSingleton<FPSManager>
{
    public override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;

    }
}
