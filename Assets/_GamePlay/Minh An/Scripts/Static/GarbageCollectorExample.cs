using System;
using UnityEngine;
using UnityEngine.Scripting;

public class GarbageCollectorExample
{
    static void ListenForGCModeChange()
    {
        // Listen on garbage collector mode changes.
        GarbageCollector.GCModeChanged += (GarbageCollector.Mode mode) =>
        {
            Debug.Log("GCModeChanged: " + mode);
        };
    }

    static void LogMode()
    {
        Debug.Log("GCMode: " + GarbageCollector.GCMode);
    }

    static void EnableGC()
    {
        GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
        // Trigger a collection to free memory.
        GC.Collect();
    }

    static void DisableGC()
    {
        GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
    }
}