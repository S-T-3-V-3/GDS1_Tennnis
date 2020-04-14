using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    public TextMeshProUGUI GameScoreText;
    public TextMeshProUGUI ScoreStatusText;
    public TextMeshProUGUI TeamText;
    public TextMeshProUGUI SetScoresText;

    GameManager gameManager;

    public void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void OnScoreUpdate(Score score)
    {
        string greenScore = gameManager.gameSettings.scoreValues[Mathf.Clamp(score.greenGameScore,0,3)];
        string blueScore = gameManager.gameSettings.scoreValues[Mathf.Clamp(score.blueGameScore,0,3)];
        GameScoreText.text = $"<color=green>{greenScore}</color> - <color=blue>{blueScore}</color>";
        SetScoresText.text = $"{score.greenSetScore}\n{score.blueSetScore}";
        ScoreStatusText.text = score.scoreStatusText;
    }
}

