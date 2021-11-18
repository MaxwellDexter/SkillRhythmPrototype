[System.Serializable]
public class TimingWindow
{
    public float window;

    public float beatOffset;

    public TimingWindow(float window, float beatOffset)
    {
        this.window = window;
        this.beatOffset = beatOffset;
    }

    public TimingWindow(TimingWindow other)
    {
        this.window = other.window;
        this.beatOffset = other.beatOffset;
    }
}
