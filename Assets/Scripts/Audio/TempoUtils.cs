using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Static class for doing math calculations and grunt work related to the Tempo class' operations.
/// </summary>
public static class TempoUtils
{
    /// <summary>
    /// Flips the input from BPM to time interval (in seconds), and time interval (in seconds) to BPM.
    /// <br></br>
    /// Who knew the relationship was that similar?
    /// <br></br>
    /// E.G. if you passed in 160 (BPM) it would return 0.375 (interval)
    /// <br></br>
    /// or if you passed in 0.375 (interval) it would return 160 (BPM).
    /// </summary>
    /// <param name="toFlip">A BPM value or an interval value in seconds.</param>
    /// <returns></returns>
    public static double FlipBpmInterval(double toFlip)
    {
        return 60.0 / toFlip;
    }

    /// <summary>
    /// converts milliseconds to seconds
    /// </summary>
    /// <param name="seconds">seconds</param>
    /// <returns>milliseconds conversion</returns>
    public static double GetMillisecondsFromSeconds(double seconds)
    {
        return seconds * 1000;
    }

    /// <summary>
    /// converts seconds to milliseconds
    /// </summary>
    /// <param name="milliseconds">milliseconds</param>
    /// <returns>seconds</returns>
    public static double GetSecondsFromMilliseconds(double milliseconds)
    {
        return milliseconds / 1000;
    }

    /// <summary>
    /// Will calculate the multiplier value for speeding a track up to meet a certain BPM.
    /// <br></br>
    /// Say you want to speed a track up to 154.32 BPM but it's recorded at 120 BPM.
    /// Pass those suckers in and set the pitch modifier on the audio source to the return value.
    /// </summary>
    /// <param name="newBpm">The value you want to speed up or slown down to</param>
    /// <param name="referenceBpm">The original BPM of the track</param>
    /// <returns>the pitch modifier value. e.g. 1.23</returns>
    public static double GetPitchModifierFromBpm(double newBpm, double referenceBpm)
    {
        return newBpm / referenceBpm;
    }

    /// <summary>
    /// Gets the subdivision window in seconds.
    /// </summary>
    /// <param name="secondsPerBeat">the seconds per beat, usually like 0.454676323876</param>
    /// <param name="subdivisionsPerbeat">the subdivisions per beat, just a whole number thanks</param>
    /// <returns>the time length of a subdivision</returns>
    public static double GetSubdivisionWindow(double secondsPerBeat, int subdivisionsPerbeat)
    {
        return secondsPerBeat / subdivisionsPerbeat;
    }

    /// <summary>
    /// Gets all of the times where are subdivision of a beat will take place. Includes the beat and the following beat.
    /// </summary>
    /// <param name="startingTime">The time you want to start getting the subdivisions from. Usually the current beat.</param>
    /// <param name="secsPerBeat">the seconds per beat</param>
    /// <param name="subdivisionsPerbeat">the subdivisions per beat. e.g. 2</param>
    /// <returns>A list of times as doubles</returns>
    public static List<double> GetSubdivisionTimes(double startingTime, double secsPerBeat, int subdivisionsPerbeat)
    {
        // assumes it is straight not swung
        List<double> times = new List<double>();
        double division = secsPerBeat / subdivisionsPerbeat;

        double currentTime = startingTime;
        // plus 1 to add the first beat of next bar
        for (int i = 0; i < subdivisionsPerbeat + 1; i++)
        {
            times.Add(currentTime);
            currentTime += division;
        }

        return times.OrderBy(d => d).ToList();
    }

    /// <summary>
    /// If the given input time is between the two given boundaries.
    /// </summary>
    /// <param name="inputTime">The input time that you want to compare to.</param>
    /// <param name="lowerBoundary">The lower time boundary that the input time should be above.</param>
    /// <param name="higherBoundary">The upper time boundary that the input time should be below.</param>
    /// <returns>true if it is in the window, false if it is not</returns>
    public static bool IsInWindow(double inputTime, double lowerBoundary, double higherBoundary)
    {
        return inputTime >= lowerBoundary && inputTime <= higherBoundary;
    }

    /// <summary>
    /// Calculates if the inputTime is within the window provided.
    /// </summary>
    /// <param name="inputTime">The time of the input</param>
    /// <param name="compareTime">The time to compare the input time to</param>
    /// <param name="window">The window in seconds</param>
    /// <param name="offset">The offset of the window from the compareTime</param>
    /// <returns>true if it is in the window, false if it is not</returns>
    public static bool IsInWindow(double inputTime, double compareTime, double window, double offset)
    {
        return IsInWindow(inputTime, compareTime + offset, compareTime + offset + window);
    }

    /// <summary>
    /// For the tempo to check whether an input is 'in time' with the beat, it uses this method to calculate which beat
    /// it has to compare to.
    /// <br></br>
    /// This is because if we're calculating 'early' values, the beat won't swap over to the next beat
    /// until the beat happens.
    /// <br></br>
    /// So we have to use the next beat as a reference if it's early.
    /// </summary>
    /// <param name="inputTime">The input time you want to compare to.</param>
    /// <param name="currentBeatTime">The time of the current beat.</param>
    /// <param name="secondsPerBeat">The number of seconds per beat. Usualy less than 1</param>
    /// <returns>The time that the Tempo should use as a reference as to what is 'in time'.</returns>
    public static double GetBeatToCompareTo(double inputTime, double currentBeatTime, double secondsPerBeat)
    {
        // if it's past the halfway point of the beat
        if (inputTime >= currentBeatTime + (secondsPerBeat / 2))
        {
            return currentBeatTime + secondsPerBeat;
        }
        return currentBeatTime;
    }
}
