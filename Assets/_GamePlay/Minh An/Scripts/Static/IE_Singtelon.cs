using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class IE_Singtelon
{
    public static IEnumerator IE_DelayAction(Action actionComplete, float timeDelay)
    {
        yield return Cache.GetWaiforSecond(timeDelay);
        actionComplete?.Invoke();
    }
}
