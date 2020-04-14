using System;
using UnityEngine;

public class SessionData : MonoBehaviour
{
    public bool isStarted = false;

    GameManager gameManager;
    ScoreBoard scoreBoard;
    Score score;

    void Awake()
    {
        gameManager = GameManager.Instance;
        gameManager.OnPlayerScore.AddListener(OnPlayerScore);
        scoreBoard = gameManager.hud.scoreBoard;
    }

    public void StartGame() {
        if (score == null)
            score = new Score();
        
        Reset();
        isStarted = true;
        score.NewSet();
    }

    public void OnPlayerScore(Team team)
    {
        gameManager.OnRoundComplete.Invoke();
        score.AddScore(team);
    }

    public Team GetLosingTeam()
    {
        return score.greenGameScore > score.blueGameScore ? Team.GREEN : Team.BLUE;
    }

    public Team GetCurrentServer() {
        return score.currentServer;
    }

    void Reset()
    {
        score.Reset();

        score.OnScoreUpdated.RemoveAllListeners();
        score.OnScoreUpdated.AddListener(scoreBoard.OnScoreUpdate);

        score.OnGameCompleted.RemoveAllListeners();
        score.OnGameCompleted.AddListener(OnGameComplete);

        score.OnSetCompleted.RemoveAllListeners();
        score.OnSetCompleted.AddListener(OnSetComplete);

        isStarted = false;   
    }

    void OnGameComplete()
    {
        gameManager.OnGameComplete.Invoke();
        isStarted = false;
    }

    void OnSetComplete()
    {
        gameManager.OnSetComplete.Invoke();
    }
}