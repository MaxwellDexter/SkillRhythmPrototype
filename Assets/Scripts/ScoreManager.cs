using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int regularScoreAddition;
    [SerializeField] private int bonusScoreAddition;

    public GameObject scorePrefab;
    public GameObject bonusPrefab;

    private int score;
    private int streak;
    private int hiStreak;

    public delegate void ScoreChanged(int score);
    public delegate void StreakChanged(int streak);
    public delegate void HiStreakChanged(int hiStreak);

    public ScoreChanged ScoreChangedEvent;
    public StreakChanged StreakChangedEvent;
    public HiStreakChanged HiStreakChangedEvent;

    public void Add()
    {
        Add(regularScoreAddition);
        SpawnScoreText(regularScoreAddition, scorePrefab);
    }

    public void AddBonus()
    {
        Add(bonusScoreAddition);
        SpawnScoreText(bonusScoreAddition, bonusPrefab);
    }

    private void Add(int score)
    {
        this.score += score;
        ScoreChangedEvent.Invoke(this.score);

        streak += 1;
        StreakChangedEvent.Invoke(streak);

        if (streak > hiStreak)
        {
            hiStreak = streak;
            HiStreakChangedEvent(hiStreak);
        }
    }

    public void ResetStreak()
    {
        streak = 0;
        StreakChangedEvent.Invoke(streak);
    }

    public void SpawnScoreText(int scoreAmount, GameObject prefab)
    {
        GameObject text = Instantiate(prefab);
        text.GetComponent<TextMesh>().text = $"+{scoreAmount}";
    }
}
