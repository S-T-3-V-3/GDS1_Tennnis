using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public bool isServing = false;
    public float baseSpeed = 0.3f;

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

    public void SetCourtOwner() {
        RaycastHit[] raycastHits = Physics.RaycastAll(this.transform.position,this.transform.up * -1).Where(x => x.collider.GetComponent<FloorCollision>() != null).ToArray();
        
        if (raycastHits.Length > 0) {
            raycastHits.First().collider.GetComponent<FloorCollision>().owningPlayer = this;
        }
    }

    public void SetState<T>() where T : State
    {
        stateManager.AddState<T>();
    }
}
