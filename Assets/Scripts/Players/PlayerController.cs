using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerSelection
{
    Player1,
    Player2
}

public class PlayerController : MonoBehaviour
{
    public GameObject playerModel;
    public PlayerSelection playerSelection;
    public Team currentTeam;
    public StateManager stateManager;
    public CameraController mainCamera;

    public float baseSpeed = 3f;

    MeshRenderer playerRenderer;
    GameManager gameManager;

    void Awake()
    {
        stateManager = this.gameObject.AddComponent<StateManager>();
        stateManager.AddState<PlayerInactiveState>();

        playerRenderer = playerModel.GetComponent<MeshRenderer>();
        gameManager = GameManager.Instance;
        mainCamera = gameManager.mainCamera.GetComponent<CameraController>();
    }

    public void SetColor(PlayerColors color)
    {
        playerRenderer.material = color.playerColor;
    }

    public void SetState<T>() where T : State
    {
        stateManager.AddState<T>();
    }
}
