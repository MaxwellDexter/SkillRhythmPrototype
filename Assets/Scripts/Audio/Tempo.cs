using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tempo class for starting and stopping tempo. Keeps the beat.
/// </summary>
public class Tempo : MonoBehaviour
{
    private bool hasStarted;
    private double secsPerBeat;
    private double startTime;
    private double currentBeatTime;
    private double nextBeatTime;

    public delegate void OnBeat();
    public OnBeat OnTempoBeat;

    /// <summary>
    /// The latency, stored in seconds.
    /// </summary>
    private double latency;

    private List<TimingOption> timingOptions;
    private TimingCalculations timingCalculation;
    private int subdivisionsPerBeat;

    public TempoConfig config;

    public double CurrentBeatTime
    {
        get { return currentBeatTime; }
    }

    /// <summary>
    /// Sets the tempo. Please call StartTempo() afterwards to start it.
    /// </summary>
    /// <param name="secondInterval">the interval between each beat in seconds</param>
    public void SetTempo(double secondInterval)
    {
        secsPerBeat = secondInterval;
    }

    /// <summary>
    /// Sets the latency of the tempo in milliseconds. For calculating the timing options.
    /// </summary>
    /// <param name="theLatency">the latency in milliseconds</param>
    public void SetLatencyMilliseconds(double theLatency)
    {
        latency = TempoUtils.GetSecondsFromMilliseconds(theLatency);
    }

    /// <summary>
    /// Sets the latency of the tempo in seconds. For calculating the timing options.
    /// </summary>
    /// <param name="theLatency">the latency in seconds</param>
    public void SetLatencySeconds(double theLatency)
    {
        latency = theLatency;
    }

    /// <summary>
    /// Set the tempo configuration that you would like. Will take effect once the tempo starts.
    /// </summary>
    /// <param name="tempoConfig">The tempo config you would like to set</param>
    public void SetTempoConfig(TempoConfig tempoConfig)
    {
        config = tempoConfig;
    }

    /// <summary>
    /// Starts the tempo. Will throw an exception if you haven't called SetTempo() yet.
    /// </summary>
    public void StartTempo()
    {
        if (secsPerBeat == 0)
        {
            throw new System.Exception("You can't start the tempo without a tempo! Please set the tempo prior to starting!");
        }
        else
        {
            startTime = AudioSettings.dspTime;
            currentBeatTime = startTime;
            hasStarted = true;
            timingOptions = config.timingOptions;
            timingCalculation = config.timingCalculation;
            subdivisionsPerBeat = config.subdivisionsPerBeat;
        }
    }

    /// <summary>
    /// Stops the tempo from doing it's thang and resets some values.
    /// </summary>
    public void StopTempo()
    {
        hasStarted = false;
        startTime = 0;
        currentBeatTime = 0;
    }

    private void Update()
    {
        if (hasStarted)
        {
            double currentTime = AudioSettings.dspTime;
            bool hasGonePastBeat = false;
            while (currentTime > currentBeatTime + secsPerBeat)
            {
                hasGonePastBeat = true;
                currentBeatTime += secsPerBeat;
                nextBeatTime = currentBeatTime + secsPerBeat;
            }
            if (hasGonePastBeat)
            {
                OnTempoBeat.Invoke();
            }
        }
    }

    /// <summary>
    /// Is the time you have in time with the beat? Passes in the current time retrieved from AudioSettings.
    /// </summary>
    /// <returns>The timing option for the beat</returns>
    public TimingOption IsInBeat()
    {
        return IsInBeat(AudioSettings.dspTime);
    }

    /// <summary>
    /// Is the time you have in time with the beat?
    /// </summary>
    /// <param name="time">Your time that you have calculated when the user presses a button or something.</param>
    /// <returns>The timing option for the beat</returns>
    public TimingOption IsInBeat(double time)
    {
        if (!hasStarted)
        {
            return null;
        }
        else return GetTimingOption(time, currentBeatTime);
    }

    /// <summary>
    /// What timing option for the current beat/subdivision is valid for the input time?
    /// Will throw an exception if the timing option couldn't be found.
    /// Ya gotta configure your options correctly bud.
    /// </summary>
    /// <param name="theTime">The input time that you want to get the option for.</param>
    /// <param name="currentBeat">The current time of the beat</param>
    /// <returns>the timing option that is for the input time</returns>
    private TimingOption GetTimingOption(double theTime, double currentBeat)
    {
        double timeMinusLatency = GetLatencyTime(theTime);
		double beatToCompareTo;
        double beatTimeFrame = secsPerBeat;

        if (timingCalculation.Equals(TimingCalculations.Subdivision) && subdivisionsPerBeat > 1)
        {
            // if we should use subdivisions
			beatToCompareTo = GetCurrentSubdivision(timeMinusLatency, currentBeat, secsPerBeat);
            beatTimeFrame = TempoUtils.GetSubdivisionWindow(secsPerBeat, subdivisionsPerBeat);
        }
        else if (timingCalculation.Equals(TimingCalculations.Pattern))
        {
            // if we should use the pattern given
            beatToCompareTo = TempoUtils.GetBeatToCompareTo(timeMinusLatency, currentBeat, secsPerBeat);
            /*
             * so we need to find out the subdivisions per beat
             * then we can map that to the beat of the song
             */
        }
        else if (timingCalculation.Equals(TimingCalculations.Song))
        {
            // if we should return the timing option of the song
            // man this means we'll have to keep track of where we are in the song
            beatToCompareTo = TempoUtils.GetBeatToCompareTo(timeMinusLatency, currentBeat, secsPerBeat);
        }
        else
		{
            // if we should use the beat
			beatToCompareTo = TempoUtils.GetBeatToCompareTo(timeMinusLatency, currentBeat, secsPerBeat);
		}

        foreach (TimingOption option in timingOptions)
        {
            foreach(TimingWindow window in option.windows)
            {
                if (TempoUtils.IsInWindow(timeMinusLatency, beatToCompareTo,
                    window.window * beatTimeFrame,
                    window.beatOffset * beatTimeFrame))
                {
                    return option;
                }
            }
            
        }

        throw new System.Exception("Timing Option was not found for beat! Please ensure that your timing options cover the entire beat!");
    }

    /// <summary>
    /// Subtracts the stored latency from the give time
    /// </summary>
    /// <param name="theTime">the time you want to adjust for latency</param>
    /// <returns>the time minus the latency</returns>
    private double GetLatencyTime(double theTime)
    {
        return theTime - latency;
    }

    /// <summary>
	/// Will retrieve all of the times of the subdivision of the current beat,
	/// and loop through them to find the one that is closest to the input time.
	/// </summary>
	/// <param name="currentBeat">The current beat to retrieve subdivisions from</param>
	/// <returns>the time of the subdivision that the input time is closest to</returns>
    private double GetCurrentSubdivision(double inputTime, double currentBeat, double secondsPerBeat)
    {
        List<double> subdivTimes = TempoUtils.GetSubdivisionTimes(currentBeat, secsPerBeat, subdivisionsPerBeat);

        for (int i = 0; i < subdivTimes.Count - 1; i++)
        {
            if (TempoUtils.IsInWindow(inputTime, subdivTimes[i], subdivTimes[i+1]))
            {
                return TempoUtils.GetBeatToCompareTo(inputTime, subdivTimes[i], TempoUtils.GetSubdivisionWindow(secondsPerBeat, subdivisionsPerBeat));
            }
        }

        string errorMsg = "Can't find the subdivision! Input Time was: " + inputTime;
        errorMsg += "\n subdivisions were: ";
        foreach (double subdiv in subdivTimes)
        {
            errorMsg += "\n" + subdiv;
        }

        Debug.LogError(errorMsg);
        return 0;
        // throw new System.Exception(errorMsg);
    }
}
