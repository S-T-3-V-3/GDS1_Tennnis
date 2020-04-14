using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private Rigidbody ballRigidbody;

    //ballAngle Inspector Test Case: 400x, 400y, 800z
    public Vector3 ballForces;

    //Bounce detects the number of ball bounces, used in s
    private float bounce = 0;
    private bool stillScoring = true;

    //Stores the player number of the last ball hitter
    public PlayerController lastHitter;

    public enum PlayerPosition
    {
        TopPosition,
        BottomPosition
    }

    // Start is called before the first frame update
    void Start()
    {
        ballRigidbody = GetComponent<Rigidbody>();
        ballRigidbody.useGravity = false;
    }

    //Send the ball back to the other side. Detected in player hit collision
    public void ReturnBall(Vector3 dir, PlayerController player)
    {
        if (lastHitter == player) {
            ScoreOpposition();
            return;
        }

        if(!ballRigidbody.useGravity)
            ballRigidbody.useGravity = true;

        ballRigidbody.velocity = Vector3.zero;
        ResetBounceCounter();

        ballRigidbody.AddForce(new Vector3(ballForces.x * dir.x * -1, ballForces.y, ballForces.z * dir.z * -1));

        lastHitter = player;
    }

    // Once a point is scored, inform game manager of point update
    public void PointScored(Team pointScorer)
    {
        stillScoring = false;
        GameManager.Instance.OnPlayerScore.Invoke(pointScorer);

        GameManager.Instance.AudioManager.GetComponent<AudioManager>().PlaySound("Score");

        //TODO: RESET ROUND
        //TODO: SET stillScoring BACK TO TRUE
    }

    public void ResetBounceCounter()
    {
        if (stillScoring)
            bounce = 0;
    }

    public void UpdateHitter(PlayerController newHitter)
    {
        lastHitter = newHitter;
    }

    //Adds to bounce counter but also checks scoring criteria
    public void AddBounceCounter(bool insideCourt, float zBallPosition, PlayerController owningPlayer)
    {
        if (stillScoring)
        {
            bounce++;

            //If the ball lands outside the court without bouncing first, give opposition to the hitter the point
            if (!insideCourt && bounce == 1)
                ScoreOpposition();
            //If ball doesn't make it over the net, score the opposition
            else if (bounce == 1 && owningPlayer == lastHitter)
            {
                ScoreOpposition();
            }
            //If the ball bounces twice, give point to last hitter
            else if (bounce > 1)
            {
                PointScored(lastHitter.currentTeam);
            }
        }
    }

    //Changes the point scorer to the opposition
    private void ScoreOpposition()
    {
        PointScored(lastHitter.currentTeam == Team.GREEN ? Team.BLUE : Team.GREEN);
    }
}