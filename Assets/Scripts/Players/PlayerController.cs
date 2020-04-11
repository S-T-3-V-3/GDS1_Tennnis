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

    public PlayerSelection playerSelection;
    public float baseSpeed = 3f;

    public MeshRenderer playerRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        playerRenderer = GetComponent<MeshRenderer>();
    }

    void FixedUpdate()
    {
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
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            float movementX = baseSpeed * -1;
            transform.position += transform.right * movementX * Time.fixedDeltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            float movementX = baseSpeed * 1;
            transform.position += transform.right * movementX * Time.fixedDeltaTime;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            float movementY = baseSpeed * 1;
            transform.position += transform.forward * movementY * Time.fixedDeltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            float movementY = baseSpeed * -1;
            transform.position += transform.forward * movementY * Time.fixedDeltaTime;
        }
    }


    //WASD Movement
    void WASDMovenet()
    {
        if (Input.GetKey(KeyCode.A))
        {
            float movementX = baseSpeed * -1;
            transform.position += transform.right * movementX * Time.fixedDeltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            float movementX = baseSpeed * 1;
            transform.position += transform.right * movementX * Time.fixedDeltaTime;
        }

        if (Input.GetKey(KeyCode.W))
        {
            float movementY = baseSpeed * 1;
            transform.position += transform.forward * movementY * Time.fixedDeltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            float movementY = baseSpeed * -1;
            transform.position += transform.forward * movementY * Time.fixedDeltaTime;
        }

    }
}
