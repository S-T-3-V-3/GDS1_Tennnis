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
    private Team lastHitter = Team.BLUE;

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
    public void ReturnBall(Vector3 dir, Team currentTeam)
    {
        if(!ballRigidbody.useGravity)
            ballRigidbody.useGravity = true;

        ballRigidbody.velocity = Vector3.zero;
        ResetBounceCounter();

        ballRigidbody.AddForce(new Vector3(ballForces.x * dir.x * -1, ballForces.y, ballForces.z * dir.z * -1));

        lastHitter = currentTeam;
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

    public void UpdateHitter(Team newHitter)
    {
        lastHitter = newHitter;
    }

    //Adds to bounce counter but also checks scoring criteria
    public void AddBounceCounter(bool insideCourt, float zBallPosition)
    {
        if (stillScoring)
        {
            bounce++;
            //If the ball lands outside the court without bouncing first, give opposition to the hitter the point
            if (!insideCourt && bounce < 2)
                ScoreOpposition();
            

            //If ball doesn't make it over the net, score the opposition
            if (insideCourt && bounce < 2)
            {
                if (zBallPosition > 0 && GameManager.Instance.player1Controller.transform.position.z > 0 ||
                    zBallPosition < 0 && GameManager.Instance.player2Controller.transform.position.z < 0)
                {
                    ScoreOpposition();
                }
            }

            //If the ball bounces twice on a player's side, give point to opposition
            if (bounce == 2)
            {
                PointScored(lastHitter);
            }
        }
    }

    //Changes the point scorer to the opposition
    private void ScoreOpposition()
    {
        PointScored(lastHitter == Team.GREEN ? Team.BLUE : Team.GREEN);
    }
}
