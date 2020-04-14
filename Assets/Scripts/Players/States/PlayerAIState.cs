using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAIState : State
{   
    PlayerController playerController;
    MeshRenderer meshRenderer;
    GameManager gameManager;
    GameObject playerModel;
    Rigidbody currentBallRB;
    Vector3 futurePosition;
    Team currentTeam;

    bool hasSetFuture = false;
    float maxZBound;
    float boundXLimit;
    float futureScalar = 0.7f;

    public override void BeginState()
    {
        playerController = this.gameObject.GetComponent<PlayerController>();

        currentTeam = playerController.currentTeam;
        playerModel = playerController.playerModel;
        meshRenderer = playerModel.GetComponent<MeshRenderer>();
        gameManager = GameManager.Instance;

        maxZBound = transform.position.z + 0.5f;
        boundXLimit = gameManager.gameSettings.aiBoundXLimit;
    }

    void FixedUpdate()
    {
        if (currentBallRB == null) {
            GetBallRef();
            return;
        }

        if (currentBallRB.velocity.z > 0)
            MoveToBall();
        else if (currentBallRB.velocity.z < 0)
            ReturnToCenter();
    }

    void GetBallRef() {
        if (gameManager.currentBall == null) return;

        currentBallRB = gameManager.currentBall.GetComponent<Rigidbody>();
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
