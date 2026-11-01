using System.Collections.Generic;
using UnityEngine;

public static class BodyPartInventory
{
    private static readonly List<BodyPartEntry> collected = new List<BodyPartEntry>();

    public static IReadOnlyList<BodyPartEntry> Collected => collected;
    public static bool RunFinished { get; private set; }

    public static void Add(BodyPartEntry part)
    {
        if (part != null && part.sprite != null) collected.Add(part);
    }

    public static void StartRun()
    {
        collected.Clear();
        RunFinished = false;
    }

    public static void FinishRun()
    {
        RunFinished = true;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        collected.Clear();
        RunFinished = false;
    }
}
