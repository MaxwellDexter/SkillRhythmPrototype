using System.Collections.Generic;
using UnityEngine;

public enum TimingCalculations
{
    Beat,
    Subdivision,
    Pattern,
    Song
}

[CreateAssetMenu(menuName = "Tempo/Config")]
public class TempoConfig : ScriptableObject
{
    public TimingCalculations timingCalculation;

    [HideInInspector]
    public int subdivisionsPerBeat;

    [HideInInspector]
    public bool[] pattern;

    [HideInInspector]
    public List<TimingOption> timingOptions;

    public TempoConfig()
    {
        if (timingOptions == null)
        {
            timingOptions = new List<TimingOption>();
        }
    }
}
