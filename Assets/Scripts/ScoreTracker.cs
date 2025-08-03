using TMPro;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    private static ScoreTracker _instance;

    public static ScoreTracker Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("ScoreTracker is null");
            return _instance;
        }
    }


    public TextMeshProUGUI scoreLabel;
    public int score = 0;

    private void Awake()
    {
        _instance = this;
    }

    public void IncreaseScore()
    {
        score++;
        scoreLabel.text = "Score: " + score.ToString();
    }
}
