using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public GameObject playerModel;

    public bool isServing = false;
    public float boundXLimit;
    public float boundZLimit;
    public float maxZBound;
    public float movementSpeed;

    MeshRenderer meshRenderer;
    private float futureScalar = 0.8f;
    private Vector3 futurePosition;
    public Team currentTeam;

    public GameManager gameManager;
    public bool hasSetFuture = false;
    public bool hasArrivedFuture = false;

    public Rigidbody currentBallRB;

    private BallBehaviour ballBehaviour;
    private float xDirectionModifier;

    // Start is called before the first frame update
    void Awake()
    {
        meshRenderer = playerModel.GetComponent<MeshRenderer>();
        gameManager = GameManager.Instance;
    }

    void Start()
    {
        maxZBound = transform.position.z + 0.5f;
    }

    void GetBallRef() {
        if (gameManager.currentBall == null) return;

        currentBallRB = gameManager.currentBall.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (currentBallRB == null) {
            GetBallRef();
            return;
        }

        ServeBall();

        if (currentBallRB.velocity.z > 0)
            MoveToBall();
        else if (currentBallRB.velocity.z < 0)
            ReturnToCenter();
    }

    public void SetColor(PlayerColors color)
    {
        meshRenderer.material = color.playerColor;
    }

    void ServeBall()
    {
        if (isServing)
        {
            FindFuturePosition();
            futurePosition.y = transform.position.y;
            transform.position = Vector3.Lerp(transform.position, futurePosition, 0.1f);
        }
    }

    void MoveToBall()
    {
        if(transform.position.x < boundXLimit && transform.position.x > -boundXLimit && transform.position.z < maxZBound)
        {
            if (!hasArrivedFuture)
            {
                FindFuturePosition();
                futurePosition.y = transform.position.y;
                transform.position = Vector3.Lerp(transform.position, futurePosition, 0.1f);

                if (transform.position == futurePosition)
                {
                    hasArrivedFuture = true;
                }
            }
            
        }
    }

    //Remove until Final
    private void PongMovement()
    {
        Vector3 ballPos = currentBallRB.transform.localPosition;
        Vector3 ballPosX = new Vector3(ballPos.x, 0, 0);
        Vector3 AIPosX = new Vector3(transform.position.x, 0, 0);
        transform.position = new Vector3(ballPos.x * movementSpeed * 0.5f, transform.position.y, transform.position.z);
    }

    private void FindFuturePosition()
    {
        futurePosition = currentBallRB.transform.position + currentBallRB.velocity * futureScalar;
        futurePosition.y = transform.position.y;
        hasSetFuture = true;
    }

    void ReturnToCenter()
    {
        if(transform.position.x != 0)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0,transform.position.y, transform.position.z), 0.05f);
        }
        else
        {
            hasArrivedFuture = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BallBehaviour>() == null) return;
        {
            GameManager.Instance.AudioManager.GetComponent<AudioManager>().PlaySound("Hit");

            ballBehaviour = collision.gameObject.GetComponent<BallBehaviour>();

            if (transform.position.z > 0)
            {
                xDirectionModifier = playerModel.transform.rotation.y;
            }
            else
            {
                xDirectionModifier = playerModel.transform.rotation.y * 3.5f / 8.5f;
                Debug.Log("Reverse Hit");
            }

            isServing = false;
            ballBehaviour.ReturnBall(this.transform, 0, currentTeam);
        }
    }
}
