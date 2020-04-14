using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private Rigidbody ballRigidbody;

    //Sets the standard forces for the ball
    public Vector3 ballForces = new Vector3(400, 400, 800);

    //Bounce detects the number of ball bounces, used in s
    private float bounce = 0;

    //Stores the player number of the last ball hitter
    private Team lastHitter;

    //Bool to check if the player has a power shot power up
    private bool powerShotEnabled = false;

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

            if (powerShotEnabled)
                StartCoroutine(SmackDown(ballForces, playerTransform.position.z));

            //Changes the x, y and z angle forces based on the player's z position and xDirectionalModifier
            switch (playerTransform.position.z)
            {
                case float z when (z < -14):
                    ballRigidbody.AddForce(new Vector3(CheckPositionX(playerTransform.position, ballForces.x, xDirectionModifier),
                        ballForces.y,
                        ballForces.z * playerTransform.forward.z));
                    break;

                case float z when (z >= -14 && z < -7):
                    ballRigidbody.AddForce(new Vector3(CheckPositionX(playerTransform.position, ballForces.x, xDirectionModifier),
                        ballForces.y,
                        ballForces.z * playerTransform.forward.z / 1.5f));
                    break;

                case float z when (z >= -7 && z < 0):
                    ballRigidbody.AddForce(new Vector3(CheckPositionX(playerTransform.position, ballForces.x, xDirectionModifier),
                        ballForces.y,
                        ballForces.z * playerTransform.forward.z / 2));
                    break;

                case float z when (z >= 0 && z < 7):
                    ballRigidbody.AddForce(new Vector3(CheckPositionX(playerTransform.position, ballForces.x, xDirectionModifier),
                        ballForces.y,
                        -ballForces.z * playerTransform.forward.z / 2));
                    break;

                case float z when (z >= 7 && z < 14):
                    ballRigidbody.AddForce(new Vector3(CheckPositionX(playerTransform.position, ballForces.x, xDirectionModifier),
                        ballForces.y,
                        -ballForces.z * playerTransform.forward.z / 1.5f));
                    break;

                case float z when (z >= 14):
                    ballRigidbody.AddForce(new Vector3(CheckPositionX(playerTransform.position, ballForces.x, xDirectionModifier),
                        ballForces.y,
                        -ballForces.z * playerTransform.forward.z));
                    break;
            }
            lastHitter = currentTeam;
        }

    }

    //Called by another class to activate the powershot
    public void ActivatePowerShot()
    {
        powerShotEnabled = true;
    }

    //Used as a part of the power shot to smack the ball down
    IEnumerator SmackDown(Vector3 originalBallForces, float zPosition)
    {
        ballForces = new Vector3(0, 300, 0);
        ballRigidbody.AddForce(ballForces);
        GameManager.Instance.AudioManager.GetComponent<AudioManager>().PlaySound("Charging");

        yield return new WaitForSeconds(0.4f);

        if (zPosition < 0)
            ballForces = new Vector3(0, -900, 2500);
        else
            ballForces = new Vector3(0, -900, -2500);

        ballRigidbody.AddForce(ballForces);
        GameManager.Instance.AudioManager.GetComponent<AudioManager>().PlaySound("PowerShot");
        powerShotEnabled = false;
        ballForces = originalBallForces;
    }

    //Changes the x-force to ensure the ball wont get sent out of the court
    private float CheckPositionX(Vector3 playerPosition, float ballForceX, float xDirectionModifier)
    {
        float returnedForce = ballForceX * xDirectionModifier;

        switch (playerPosition.z)
        {
            //Mid z-field scenario
            case float z when (z >= -14 && z < -7 || z >= 7 && z < 14):
                returnedForce = ballForceX * xDirectionModifier * 1.5f;
                break;
            
            //Close z-field scenario
            case float z when (z >= -7 && z < 7):
                returnedForce = ballForceX * xDirectionModifier * 2;
                break;

            //Far z-field scenario
            default:
                break;
        }

        //If player is on the edge and about to hit it out, set force to 0 instead
        //If they're within the 6 range, reduce the force
        switch (playerPosition.x)
        {
            case float x when (x <= -8 && xDirectionModifier < 0 && playerPosition.z < 0 ||
            x >= 8 && xDirectionModifier > 0 && playerPosition.z < 0 ||
            x <= -8 && xDirectionModifier < 0 && playerPosition.z > 0 ||
            x >= 8 && xDirectionModifier > 0 && playerPosition.z > 0):
                returnedForce = 0;
                break;
            case float x when (x <= -6 && xDirectionModifier < 0 && playerPosition.z < 0 ||
            x >= 6 && xDirectionModifier > 0 && playerPosition.z < 0 ||
            x <= -6 && xDirectionModifier < 0 && playerPosition.z > 0 ||
            x >= 6 && xDirectionModifier > 0 && playerPosition.z > 0):
                returnedForce = returnedForce / 1.5f;
                break;
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
