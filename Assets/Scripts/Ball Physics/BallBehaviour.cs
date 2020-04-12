using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private Rigidbody ballRigidbody;

    //ballAngle Inspector Test Case: 0x, 400y, 800z
    public Vector3 ballForces;

    // Start is called before the first frame update
    void Start()
    {
        ballRigidbody = GetComponent<Rigidbody>();
    }

    public void ReturnBall(Transform playerTransform, float xDirectionModifier)
    {
        ballRigidbody.velocity = Vector3.zero;
        switch (playerTransform.position.z)
        {
            case float z when (z < -14):
                ballRigidbody.AddForce(new Vector3(ballForces.x * xDirectionModifier, ballForces.y, ballForces.z * playerTransform.forward.z));
                break;
            case float z when (z >= -14 && z < -7):
                ballRigidbody.AddForce(new Vector3(ballForces.x * xDirectionModifier, ballForces.y / 1.5f, ballForces.z * playerTransform.forward.z));
                break;
            case float z when (z >= -7 && z < 0):
                ballRigidbody.AddForce(new Vector3(ballForces.x * xDirectionModifier, ballForces.y / 2, ballForces.z * playerTransform.forward.z));
                break;
            case float z when (z >= 0 && z < 7):
                ballRigidbody.AddForce(new Vector3(ballForces.x * xDirectionModifier, ballForces.y / 2, -ballForces.z * playerTransform.forward.z));
                break;
            case float z when (z >= 7 && z < 14):
                ballRigidbody.AddForce(new Vector3(ballForces.x * xDirectionModifier, ballForces.y / 1.5f, -ballForces.z * playerTransform.forward.z));
                break;
            case float z when (z >= 14):
                ballRigidbody.AddForce(new Vector3(ballForces.x * xDirectionModifier, ballForces.y, -ballForces.z * playerTransform.forward.z));
                break;
        }
    }

}
