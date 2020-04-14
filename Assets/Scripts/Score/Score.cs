using UnityEngine.Events;

[System.Serializable]
public class ScoreEvent : UnityEvent<Score> {};

public class Score {
    public int greenGameScore;
    public int blueGameScore;
    public int greenSetScore;
    public int blueSetScore;

    public string scoreStatusText;

    public Team currentServer = Team.BLUE;

    public ScoreEvent OnScoreUpdated;
    public UnityEvent OnSetCompleted;
    public UnityEvent OnGameCompleted;

    public void Reset()
    {
        OnScoreUpdated = new ScoreEvent();
        OnSetCompleted = new UnityEvent();
        OnGameCompleted = new UnityEvent();

        greenSetScore = 0;
        blueSetScore = 0;
        greenGameScore = 0;
        blueGameScore = 0;
    }

    public void NewSet()
    {
        greenGameScore = 0;
        blueGameScore = 0;

        currentServer = currentServer == Team.GREEN ? Team.BLUE : Team.GREEN;   
    }

    public void AddScore(Team team)
    {
        if (team == Team.GREEN) {
            if (greenGameScore < 3 || greenGameScore <= blueGameScore) {
                greenGameScore++;
            }
            else if (greenGameScore > blueGameScore) {
                greenSetScore++;

                if (greenSetScore >= 6) {
                    OnGameCompleted.Invoke();
                }
                else {
                    NewSet();
                    OnSetCompleted.Invoke();
                }
            }
        }
        else if (team == Team.BLUE) {
            if (blueGameScore < 3 || blueGameScore <= greenGameScore) {
                blueGameScore++;
            }
            else if (blueGameScore > greenGameScore) {
                blueSetScore++;

                if (blueSetScore >= 6)
                    OnGameCompleted.Invoke();

                NewSet();
                OnSetCompleted.Invoke();
            }
        }

        if (greenGameScore >= 3 && blueGameScore >= 3) {
            if (greenGameScore == blueGameScore)
                scoreStatusText = "Deuce";
            else if (greenGameScore > blueGameScore)
                scoreStatusText = "<color=green>Advantage</color>";
            else if (blueGameScore > greenGameScore)
                scoreStatusText = "<color=blue>Advantage</color>";
        }
        else
            scoreStatusText = "";

        OnScoreUpdated.Invoke(this);
    }
}
