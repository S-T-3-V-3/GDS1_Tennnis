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

    //Stores the player number of the last ball hitter
    private Team lastHitter;

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
    public void ReturnBall(Transform playerTransform, float xDirectionModifier, Team currentTeam)
    {
        //Prevents the player from double hitting the ball by making it only hittable by the opposition
        //If the ball is being server, useGravity will be disabled and the last hitter is irrelevant
        if (lastHitter != currentTeam || !ballRigidbody.useGravity)
        {
            if (!ballRigidbody.useGravity)
                ballRigidbody.useGravity = true;

            ballRigidbody.velocity = Vector3.zero;
            ResetBounceCounter();

            //Changes the x, y and z angle forces based on the player's z position and xDirectionalModifier
            switch (playerTransform.position.z)
            {
                case float z when (z < -14):
                    ballRigidbody.AddForce(new Vector3(CheckPositionX(playerTransform.position, ballForces.x * xDirectionModifier),
                        ballForces.y,
                        ballForces.z * playerTransform.forward.z));
                    break;

                case float z when (z >= -14 && z < -7):
                    ballRigidbody.AddForce(new Vector3(CheckPositionX(playerTransform.position, ballForces.x * xDirectionModifier),
                        ballForces.y,
                        ballForces.z * playerTransform.forward.z / 1.5f));
                    break;

                case float z when (z >= -7 && z < 0):
                    ballRigidbody.AddForce(new Vector3(CheckPositionX(playerTransform.position, ballForces.x * xDirectionModifier),
                        ballForces.y,
                        ballForces.z * playerTransform.forward.z / 2));
                    break;

                case float z when (z >= 0 && z < 7):
                    ballRigidbody.AddForce(new Vector3(CheckPositionX(playerTransform.position, ballForces.x * xDirectionModifier),
                        ballForces.y,
                        -ballForces.z * playerTransform.forward.z / 2));
                    break;

                case float z when (z >= 7 && z < 14):
                    ballRigidbody.AddForce(new Vector3(CheckPositionX(playerTransform.position, ballForces.x * xDirectionModifier),
                        ballForces.y,
                        -ballForces.z * playerTransform.forward.z / 1.5f));
                    break;

                case float z when (z >= 14):
                    ballRigidbody.AddForce(new Vector3(CheckPositionX(playerTransform.position, ballForces.x * xDirectionModifier),
                        ballForces.y,
                        -ballForces.z * playerTransform.forward.z));
                    break;
            }
            lastHitter = currentTeam;
        }

    }

    //Changes the x-force to ensure the ball wont get sent out of the court
    private float CheckPositionX(Vector3 playerPosition, float standardForceX)
    {
        float returnedForce = standardForceX;

        switch (playerPosition.z)
        {
            //Mid z-field scenario
            case float z when (z >= -14 && z < -7 || z >= 7 && z < 14):
                returnedForce = standardForceX * 1.5f;
                break;
            
            //Close z-field scenario
            case float z when (z >= -7 && z < 7):
                returnedForce = standardForceX * 2;
                break;

            //Far z-field scenario
            default:
                break;
        }

        return returnedForce;
    }

    // Once a point is scored, inform game manager of point update
    public void PointScored(Team pointScorer)
    {
        GameManager.Instance.OnPlayerScore.Invoke(pointScorer);

        GameManager.Instance.AudioManager.GetComponent<AudioManager>().PlaySound("Score");
    }

    public void ResetBounceCounter()
    {
        bounce = 0;
    }

    public void UpdateHitter(Team newHitter)
    {
        lastHitter = newHitter;
    }

    //Adds to bounce counter but also checks scoring criteria
    public void AddBounceCounter(bool insideCourt, float zBallPosition)
    {
        bounce++;
        //If the ball lands outside the court without bouncing first, give opposition to the hitter the point
        if (!insideCourt && bounce < 2)
            ScoreOpposition();
            

        //If ball doesn't make it over the net, score the opposition
        if (insideCourt && bounce < 2)
        {
            if (zBallPosition > 0 && GameManager.Instance.player1Prefab.transform.position.z > 0 ||
                zBallPosition < 0 && GameManager.Instance.player1Prefab.transform.position.z < 0)
            {
                ScoreOpposition();
            }
        }

        //If the ball bounces twice on a player's side, give point to opposition
        if (bounce == 2)
        {
            ScoreOpposition();
        }
    }

    //Changes the point scorer to the opposition
    private void ScoreOpposition()
    {
        PointScored(lastHitter == Team.GREEN ? Team.BLUE : Team.GREEN);
    }

}
