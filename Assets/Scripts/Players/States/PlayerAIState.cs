using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAIState : State
{   
    PlayerController playerController;
    MeshRenderer meshRenderer;
    GameManager gameManager;
    GameObject playerModel;
    BallBehaviour ballBehaviour;
    Rigidbody currentBallRB;
    Vector3 futurePosition;
    Team currentTeam;
    PlayerController otherPlayer;

    bool hasArrivedFuture = false;
    bool hasSetFuture = false;
    float maxZBound;
    float boundXLimit;
    float futureScalar = 2f;
    float xDirectionModifier;

    public override void BeginState()
    {
        playerController = this.gameObject.GetComponent<PlayerController>();

        currentTeam = playerController.currentTeam;
        playerModel = playerController.playerModel;
        meshRenderer = playerModel.GetComponent<MeshRenderer>();
        gameManager = GameManager.Instance;

        maxZBound = transform.position.z + 0.5f;
        boundXLimit = gameManager.gameSettings.aiBoundXLimit;

        gameManager.OnRoundComplete.AddListener(() => {
            hasSetFuture = false;
        });
    }

    void FixedUpdate()
    {
        if (currentBallRB == null) {
            GetBallRef();
            return;
        }

        LookAtBall();

        if (currentBallRB.velocity.z > 0)
            MoveToBall();
        else if (currentBallRB.velocity.z < 0)
            ReturnToCenter();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BallBehaviour>() == null) return;
        {
            GameManager.Instance.AudioManager.GetComponent<AudioManager>().PlaySound("Hit");

            ballBehaviour = collision.gameObject.GetComponent<BallBehaviour>();
            Vector3 dir = Vector3.Normalize(this.transform.position - gameManager.currentBall.transform.position);
            ballBehaviour.ReturnBall(dir, GetComponent<PlayerController>().currentTeam);

            hasSetFuture = false;
        }
    }

    void GetBallRef() {
        if (gameManager.currentBall == null) return;

        currentBallRB = gameManager.currentBall.GetComponent<Rigidbody>();
    }

    void LookAtBall()
    {
        if (gameManager.currentBall == null) return;

        playerModel.transform.LookAt(gameManager.currentBall.transform.position);
        playerModel.transform.rotation = Quaternion.Euler(0,playerModel.transform.rotation.eulerAngles.y,0);
    }

    void MoveToBall()
    {
        if (!hasArrivedFuture)
        {
            if (hasSetFuture == false)
                FindFuturePosition();

            futurePosition.y = transform.position.y;
            transform.position = Vector3.Lerp(transform.position, futurePosition, 0.1f);

            if (transform.position == futurePosition)
            {
                hasArrivedFuture = true;
            }
        }
    }

    void FindFuturePosition()
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
    }

    //Remove until Final
    private void PongMovement()
    {
        Vector3 ballPos = currentBallRB.transform.localPosition;
        Vector3 ballPosX = new Vector3(ballPos.x, 0, 0);
        Vector3 AIPosX = new Vector3(transform.position.x, 0, 0);
        transform.position = new Vector3(ballPos.x * playerController.baseSpeed * 0.5f, transform.position.y, transform.position.z);
    }
}
