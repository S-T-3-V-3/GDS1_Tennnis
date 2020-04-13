using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public GameObject playerModel;

    public GameObject ballTarget;
    Rigidbody ballRB;

    public float boundXLimit;
    public float maxZBound;
    public float movementSpeed;

    MeshRenderer meshRenderer;
    private float futureScalar = 0.7f;
    private Vector3 futurePosition;
    public Team currentTeam;

    bool hasSetFuture = false;

    // Start is called before the first frame update
    void Awake()
    {
        meshRenderer = playerModel.GetComponent<MeshRenderer>();
    }

    void Start()
    {
        maxZBound = transform.position.z + 0.5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ballRB == null) ballRB = ballTarget.GetComponent<Rigidbody>();

        //Vector3 ballDirection = transform.position - ballTarget.transform.position;
        //ballDirection = ballDirection.normalized;

        if (ballRB.velocity.z > 0)
            MoveToBall();
        else if (ballRB.velocity.z < 0)
            ReturnToCenter();
    }

    public void SetColor(PlayerColors color)
    {
        meshRenderer.material = color.playerColor;
    }

    void MoveToBall()
    {
        if(transform.position.x < boundXLimit && transform.position.x > -boundXLimit && transform.position.z < maxZBound)
        {
            if (!hasSetFuture) FindFuturePosition();
            futurePosition.y = transform.position.y;
            transform.position = Vector3.Lerp(transform.position, futurePosition, 0.1f);
        }
    }

    //Remove until Final
    private void PongMovement()
    {
        Vector3 ballPos = ballTarget.transform.localPosition;
        Vector3 ballPosX = new Vector3(ballPos.x, 0, 0);
        Vector3 AIPosX = new Vector3(transform.position.x, 0, 0);
        transform.position = new Vector3(ballPos.x * movementSpeed * 0.5f, transform.position.y, transform.position.z);
    }

    private void FindFuturePosition()
    {
        futurePosition = ballTarget.transform.position + ballRB.velocity * futureScalar;
        futurePosition.y = transform.position.y;
        hasSetFuture = true;
        //transform.position = Vector3.Lerp(transform.position, futurePosition, 0.1f);
    }

    void ReturnToCenter()
    {
        if(transform.position.x != 0)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0,transform.position.y, transform.position.z), 0.05f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Ball_Launch>() != null)
        {
            //hasHit = true;
        }
    }

    /*private bool GroundCheck()
    {
        //return Physics.
    }*/
}
