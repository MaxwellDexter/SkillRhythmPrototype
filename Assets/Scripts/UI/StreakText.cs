using UnityEngine;
using UnityEngine.UI;

public class StreakText : MonoBehaviour
{
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
        GameObject.Find("Score Manager").GetComponent<ScoreManager>().StreakChangedEvent += UpdateText;
    }

    private void UpdateText(int streak)
    {
        text.text = streak.ToString();
    }
}
