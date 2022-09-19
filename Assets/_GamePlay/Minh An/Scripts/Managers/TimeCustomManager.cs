using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TimeCustomManager
{
    public static string FormatTimeToString(float timeConvert)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeConvert);
        if (timeSpan.TotalDays >= 1)
        {
            return string.Format("{0:D2}d:{1:D2}h", timeSpan.Days, timeSpan.Hours);
        }
        if (timeSpan.TotalHours >= 1)
        {
            return string.Format("{0:D2}h:{1:D2}m", timeSpan.Hours, timeSpan.Minutes);
        }
        return string.Format("{0:D2}m:{1:D2}s", timeSpan.Minutes, timeSpan.Seconds);
    }
}
