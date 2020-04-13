using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private Rigidbody ballRigidbody;

    //Used for the scoreboard to reset bounces and detect who's scoring
    private ScoreboardManager scoreboardManager;

    //ballAngle Inspector Test Case: 400x, 400y, 800z
    public Vector3 ballForces;

    // Start is called before the first frame update
    void Start()
    {
        ballRigidbody = GetComponent<Rigidbody>();
        scoreboardManager = GameObject.FindObjectOfType<ScoreboardManager>();
        ballRigidbody.useGravity = false;
    }

    public void ReturnBall(Transform playerTransform, float xDirectionModifier)
    {
        if(!ballRigidbody.useGravity)
            ballRigidbody.useGravity = true;

        ballRigidbody.velocity = Vector3.zero;
        scoreboardManager.ResetBounceCounter();

        switch (playerTransform.position.z)
        {
            case float z when (z < -14):
                ballRigidbody.AddForce(new Vector3(ballForces.x * xDirectionModifier, ballForces.y, ballForces.z * playerTransform.forward.z));
                scoreboardManager.PlayerOneHit(true);
                break;
            case float z when (z >= -14 && z < -7):
                ballRigidbody.AddForce(new Vector3(ballForces.x * xDirectionModifier, ballForces.y / 1.5f, ballForces.z * playerTransform.forward.z));
                scoreboardManager.PlayerOneHit(true);
                break;
            case float z when (z >= -7 && z < 0):
                ballRigidbody.AddForce(new Vector3(ballForces.x * xDirectionModifier, ballForces.y / 2, ballForces.z * playerTransform.forward.z));
                scoreboardManager.PlayerOneHit(true);
                break;
            case float z when (z >= 0 && z < 7):
                ballRigidbody.AddForce(new Vector3(ballForces.x * xDirectionModifier, ballForces.y / 2, -ballForces.z * playerTransform.forward.z));
                scoreboardManager.PlayerOneHit(false);
                break;
            case float z when (z >= 7 && z < 14):
                ballRigidbody.AddForce(new Vector3(ballForces.x * xDirectionModifier, ballForces.y / 1.5f, -ballForces.z * playerTransform.forward.z));
                scoreboardManager.PlayerOneHit(false);
                break;
            case float z when (z >= 14):
                ballRigidbody.AddForce(new Vector3(ballForces.x * xDirectionModifier, ballForces.y, -ballForces.z * playerTransform.forward.z));
                scoreboardManager.PlayerOneHit(false);
                break;
        }
    }

}
