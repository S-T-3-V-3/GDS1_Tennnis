using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerSelection
    {
        Player1,
        Player2
    }

    public GameObject playerModel;
    public PlayerSelection playerSelection;
    public Team currentTeam;
    public float baseSpeed = 3f;

    MeshRenderer playerRenderer;
    GameManager gameManager;
    CameraController mainCamera;

    public Rigidbody rigidBody;

    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerRenderer = playerModel.GetComponent<MeshRenderer>();
        gameManager = GameManager.Instance;
        mainCamera = gameManager.mainCamera.GetComponent<CameraController>();
    }

    void FixedUpdate()
    {
        LookAtBall();
        MovementController();
    }

    public void SetColor(PlayerColors color)
    {
        playerRenderer.material = color.playerColor;
    }

    void MovementController()
    {
        switch (playerSelection)
        {
            case PlayerSelection.Player1:
                WASDMovenet();
                break;
            case PlayerSelection.Player2:
                KeyMovement();
                break;
        }
    }

    //Temporary Controller
    // Update is called once per frame

    //Key Movement
    void KeyMovement()
    {
        Vector3 force = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //transform.position += mainCamera.rightVector * baseSpeed * Time.fixedDeltaTime * -1;
            force += transform.right * baseSpeed * Time.fixedDeltaTime * -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            //transform.position += mainCamera.rightVector * baseSpeed * Time.fixedDeltaTime;
            force += transform.right * baseSpeed * Time.fixedDeltaTime;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            //transform.position += mainCamera.forwardVector * baseSpeed * Time.fixedDeltaTime;
            force += mainCamera.forwardVector * baseSpeed * Time.fixedDeltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            //transform.position += mainCamera.forwardVector * baseSpeed * Time.fixedDeltaTime * -1;
            force += mainCamera.forwardVector * baseSpeed * Time.fixedDeltaTime * -1;
        }

        force = Vector3.ClampMagnitude(force, baseSpeed);
        rigidBody.velocity = force;
    }


    //WASD Movement
    void WASDMovenet()
    {
        Vector3 force = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            //transform.position += mainCamera.rightVector * baseSpeed * Time.fixedDeltaTime * -1;
            force += mainCamera.rightVector * baseSpeed * 0.5f * -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //transform.position += mainCamera.rightVector * baseSpeed * Time.fixedDeltaTime;
            force += mainCamera.rightVector * baseSpeed * 0.5f;
        }

        if (Input.GetKey(KeyCode.W))
        {
            //transform.position += mainCamera.forwardVector * baseSpeed * Time.fixedDeltaTime;
            force += mainCamera.forwardVector * baseSpeed * 0.5f;
            //force += new Vector3(0, 10, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //transform.position += mainCamera.forwardVector * baseSpeed * Time.fixedDeltaTime * -1;
            force += mainCamera.forwardVector * baseSpeed * 0.5f * -1;
        }

        force = Vector3.ClampMagnitude(force, baseSpeed);
        rigidBody.velocity = force;
        Debug.Log(rigidBody.velocity);
    }

    void LookAtBall()
    {
        if (gameManager.currentBall == null) return;

        Vector3 difference = gameManager.currentBall.transform.position - playerModel.transform.position;
        float rotationY = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg;
        playerModel.transform.localRotation = Quaternion.Euler(0, rotationY, 0);
    }
}
