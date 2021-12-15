using UnityEngine;
using UnityEngine.UI;

public class HiStreakText : MonoBehaviour
{
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
        GameObject.Find("Score Manager").GetComponent<ScoreManager>().HiStreakChangedEvent += UpdateText;
    }

    private void UpdateText(int streak)
    {
        text.text = streak.ToString();
    }
}