using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlledState : State
{
    PlayerController playerController;
    GameObject playerModel;
    GameManager gameManager;
    BallBehaviour ballBehaviour;
    CameraController mainCamera;
    PlayerSelection playerSelection;
    PlayerController otherPlayer;
    Rigidbody playerRB;

    bool forwardInput;
    bool backwardInput;
    bool leftInput;
    bool rightInput;

    public override void BeginState()
    {
        gameManager = GameManager.Instance;
        playerController = this.gameObject.GetComponent<PlayerController>();
        playerSelection = playerController.playerSelection;
        playerModel = playerController.playerModel;
        mainCamera = playerController.mainCamera;

        playerRB = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        LookAtBall();
        GetInputs();
        DoMovement();
    }

    void LookAtBall()
    {
        if (gameManager.currentBall == null) return;

        playerModel.transform.LookAt(gameManager.currentBall.transform.position);
        playerModel.transform.rotation = Quaternion.Euler(0,playerModel.transform.rotation.eulerAngles.y,0);
    }

    void GetInputs() {
        if (playerSelection == PlayerSelection.Player1) {
            forwardInput = Input.GetKey(KeyCode.W);
            backwardInput = Input.GetKey(KeyCode.S);
            leftInput = Input.GetKey(KeyCode.A);
            rightInput = Input.GetKey(KeyCode.D);
        }
        else {
            forwardInput = Input.GetKey(KeyCode.UpArrow);
            backwardInput = Input.GetKey(KeyCode.DownArrow);
            leftInput = Input.GetKey(KeyCode.LeftArrow);
            rightInput = Input.GetKey(KeyCode.RightArrow);
        }

        if (Input.GetKey(KeyCode.Alpha1)) {
            if (gameManager.currentPlaymode == GamePlay.SinglePlayer) return;
            gameManager.currentPlaymode = GamePlay.SinglePlayer;
            gameManager.Start();
        }

        if (Input.GetKey(KeyCode.Alpha2)) {
            if (gameManager.currentPlaymode == GamePlay.DoublePlayer) return;
            gameManager.currentPlaymode = GamePlay.DoublePlayer;
            gameManager.Start();
        }
    }
    
    void DoMovement()
    {
        Vector3 movementDirection = Vector3.zero;

        if (forwardInput)
            movementDirection += mainCamera.forwardVector;
        else if (backwardInput)
            movementDirection += mainCamera.forwardVector * -1;

        if (leftInput)
            movementDirection += mainCamera.rightVector * -1;
        else if (rightInput)
            movementDirection += mainCamera.rightVector;

        movementDirection = movementDirection.normalized;

        playerRB.MovePosition(this.transform.position + movementDirection * playerController.baseSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BallBehaviour>() == null) return;
        {
            GameManager.Instance.AudioManager.GetComponent<AudioManager>().PlaySound("Hit");

            ballBehaviour = collision.gameObject.GetComponent<BallBehaviour>();
            Vector3 dir = Vector3.Normalize(this.transform.position - gameManager.currentBall.transform.position);
            ballBehaviour.ReturnBall(dir, this.playerController);

            this.playerRB.velocity = this.playerRB.velocity * 0.1f;
        }
    }
}