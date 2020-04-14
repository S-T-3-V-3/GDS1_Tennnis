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
    
    float xDirectionModifier;

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

        Vector3 difference = gameManager.currentBall.transform.position - playerModel.transform.position;
        float rotationY = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg;
        playerModel.transform.localRotation = Quaternion.Euler(0, rotationY, 0);
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
    }
    
    void DoMovement()
    {
        Vector3 newPos = Vector3.zero;

        if (forwardInput)
            newPos += mainCamera.forwardVector * playerController.baseSpeed * Time.fixedDeltaTime;
        else if (backwardInput)
            newPos += mainCamera.forwardVector * playerController.baseSpeed * Time.fixedDeltaTime * -1;

        if (leftInput)
            newPos += mainCamera.rightVector * playerController.baseSpeed * Time.fixedDeltaTime * -1;
        else if (rightInput)
            newPos += mainCamera.rightVector * playerController.baseSpeed * Time.fixedDeltaTime;

        this.transform.position += newPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BallBehaviour>() == null) return;
        {
            GameManager.Instance.AudioManager.GetComponent<AudioManager>().PlaySound("Hit");

            ballBehaviour = collision.gameObject.GetComponent<BallBehaviour>();

            if(transform.position.z < 0)
            {
                xDirectionModifier = playerModel.transform.localRotation.y;
            }
            else
            {
                xDirectionModifier = playerModel.transform.localRotation.y * 3.5f / 8.5f;
            }

            ballBehaviour.ReturnBall(this.transform, xDirectionModifier, GetComponent<PlayerController>().currentTeam);
        }
    }
}