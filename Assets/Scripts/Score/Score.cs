using UnityEngine.Events;

[System.Serializable]
public class ScoreEvent : UnityEvent<Score> {};

public class Score {
    public int redGameScore;
    public int blueGameScore;
    public int redSetScore;
    public int blueSetScore;

    public string scoreStatusText;

    public ScoreEvent OnScoreUpdated;
    public UnityEvent OnSetCompleted;
    public UnityEvent OnGameCompleted;

    public void Reset()
    {
        OnScoreUpdated = new ScoreEvent();
        OnSetCompleted = new UnityEvent();
        OnGameCompleted = new UnityEvent();

        redGameScore = 0;
        blueGameScore = 0;
        redSetScore = 0;
        blueSetScore = 0;
    }

    public void NewSet()
    {
        redGameScore = 0;
        blueGameScore = 0;
        OnSetCompleted.Invoke();
    }

    public void AddScore(Team team)
    {
        if (team == Team.RED) {
            if (redGameScore < 3 || redGameScore <= blueGameScore) {
                redGameScore++;
            }
            else if (redGameScore > blueGameScore) {
                redSetScore++;
                if (redSetScore >= 6)
                    OnGameCompleted.Invoke();
                NewSet();
            }
        }
        else if (team == Team.BLUE) {
            if (blueGameScore < 3 || blueGameScore <= redGameScore) {
                blueGameScore++;
            }
            else if (blueGameScore > redGameScore) {
                blueSetScore++;
                if (blueSetScore >= 6)
                    OnGameCompleted.Invoke();
                NewSet();
            }
        }

        if (redGameScore >= 3 && blueGameScore >= 3) {
            if (redGameScore == blueGameScore)
                scoreStatusText = "Deuce";
            else if (redGameScore > blueGameScore)
                scoreStatusText = "<color=green>Advantage</color>";
            else if (blueGameScore > redGameScore)
                scoreStatusText = "<color=blue>Advantage</color>";
        }
        else
            scoreStatusText = "";

        OnScoreUpdated.Invoke(this);
    }
}
