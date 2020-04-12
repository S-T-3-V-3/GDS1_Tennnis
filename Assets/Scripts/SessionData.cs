using System;
using UnityEngine;

public class SessionData : MonoBehaviour
{
    public bool isStarted = false;

    GameManager gameManager;
    ScoreBoard scoreBoard;
    Score score;

    public void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnPlayerScore.AddListener(OnPlayerScore);
        scoreBoard = gameManager.hud.scoreBoard;

        score = new Score();
        Reset();
    }

    public void OnPlayerScore(Team team)
    {
        score.AddScore(team);
    }

    void Reset()
    {
        score.Reset();

        score.OnScoreUpdated.RemoveAllListeners();
        score.OnScoreUpdated.AddListener(scoreBoard.OnScoreUpdate);

        score.OnGameCompleted.RemoveAllListeners();
        score.OnGameCompleted.AddListener(OnGameComplete);

        isStarted = false;   
    }

    void OnGameComplete()
    {
        gameManager.OnGameComplete.Invoke();
        isStarted = false;
    }
}