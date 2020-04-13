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

    public Team GetLoosingTeam()
    {
        if(score.redGameScore > score.blueGameScore)
        {
            return Team.RED;
        }
        else
        {
            return Team.BLUE;
        }
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