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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)){
            SendUpwards();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            SendDownwards();
        }
    }

    private void SendUpwards()
    {
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.AddForce(ballForces);
    }

    private void SendDownwards()
    {
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.AddForce(new Vector3(ballForces.x, ballForces.y, -ballForces.z));
    }
}
