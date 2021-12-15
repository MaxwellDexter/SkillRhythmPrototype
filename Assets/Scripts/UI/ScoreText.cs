using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    private Text text;

    void Start()
    {
        text = GetComponent<Text>();
        GameObject.Find("Score Manager").GetComponent<ScoreManager>().ScoreChangedEvent += UpdateText;
    }

    private void UpdateText(int score)
    {
        text.text = score.ToString();
    }
}