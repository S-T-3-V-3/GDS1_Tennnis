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
        string redScore = gameManager.gameSettings.scoreValues[Mathf.Clamp(score.redGameScore,0,3)];
        string blueScore = gameManager.gameSettings.scoreValues[Mathf.Clamp(score.blueGameScore,0,3)];
        GameScoreText.text = $"<color=red>{redScore}</color> - <color=blue>{blueScore}</color>";
        SetScoresText.text = $"{score.redSetScore}\n{score.blueSetScore}";
        ScoreStatusText.text = score.scoreStatusText;
    }
}

