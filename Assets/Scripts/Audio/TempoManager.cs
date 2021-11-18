using UnityEngine;

public class TempoManager : MonoBehaviour
{
    private bool tempoFound;
    public bool useTapTempo;
    public Tempo tempo;

    public TempoManager()
    {
        tempoFound = false;
    }

    public void ReceiveTempoInMiliseconds(double interval)
    {
        tempoFound = true;
        tempo.SetTempo(interval);
        tempo.SetLatencyMilliseconds(50);
        tempo.StartTempo();
        Debug.Log("BPM: " + TempoUtils.FlipBpmInterval(interval));
	}

	public TimingOption IsInTime(double time)
    {
        return tempo.IsInBeat(time);
    }

    public TimingOption IsInTime()
    {
        return tempo.IsInBeat();
    }

    public double GetCurrentBeatTime()
    {
        return tempo.CurrentBeatTime;
    }

    public void ReceiveTempo(double secondsPerBeat)
    {
        // nothing
    }

    public void StopTempo()
    {
        tempo.StopTempo();
        tempoFound = false;
    }
}
