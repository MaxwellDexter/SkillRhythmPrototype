using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A framework for timing options
/// </summary>
[System.Serializable]
public class TimingOption
{
    /// <summary>
    /// Constructor
    /// </summary>
    public TimingOption()
    {
        associatedColor = new Color(255, 255, 255, 255);
        optionName = "Nice";
        windows = new List<TimingWindow>();
        windows.Add(new TimingWindow(0.1f, 0));
    }

    public TimingOption(TimingOption other)
    {
        associatedColor = other.associatedColor;
        optionName = other.optionName + " (Copy)";
        breaksCombo = other.breaksCombo;
        windows = new List<TimingWindow>();
        foreach(TimingWindow window in other.windows)
        {
            windows.Add(new TimingWindow(window));
        }
    }

    /// <summary>
    /// the name of the timing option. has to be unique
    /// </summary>
    public string optionName;

    /// <summary>
    /// does achieving this timing option break the combo?
    /// </summary>
    public bool breaksCombo;

    /// <summary>
    /// The color that is associated with this timing option. Can be used to apply extra styling to the visualisation.
    /// </summary>
    public Color associatedColor;

    /// <summary>
    /// The windows in which the timing option is active.
    /// <br></br>
    /// Will be multiplied by the seconds per beat to get the actual seconds this is active for.
    /// <br></br>
    /// Has to be a percentage in 0-1 format. E.G. 0.59 for 59% or 0.1 for 10%.
    /// <br></br>
    /// When making windows with offsets be mindful that you can only have a range of -0.5 to 0.5.
    /// This covers the full range of a beat from halfway before it happens to halfway after it happens.
    /// <br></br>
    /// If there was a window of 0.2 and an offset of 0.3, the timing option would be active between the
    /// time equivalent of 30% through the beat through to 50% through the beat (halfway).
    /// </summary>
    public List<TimingWindow> windows;
}
