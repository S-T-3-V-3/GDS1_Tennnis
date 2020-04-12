using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    public enum GamePlay
    {
        SinglePlayer,
        DoublePlayer
    }

    public GamePlay currentPlaymode = GamePlay.SinglePlayer;

    //Test temporary variables
    [HideInInspector] public int scoreP1;
    [HideInInspector] public int setWinsP1;

    [HideInInspector] public int scoreP2;
    [HideInInspector] public int setWinsP2;

    public GameObject ballPrefab; //Replace with real ball

    PlayerColors color1;
    PlayerColors color2;

    //Additional Temporary Variables
    //Make Sure to insert these variables
    [Header("Respawn")]
    public Transform player1Spawn; //This can be stored into a scriptable object
    public Transform player2Spawn;
    public Transform ballSpawnPos;

    [Header("Camera")]
    public Camera mainCamera;

    [Header("Camera Positions")]
    public Transform cameraPosition1;
    public Transform cameraPosition2;

    [Header("Playable Prefabs")]
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public GameObject AiPlayerPrefab;

    [Header("Game Settings")]
    public GameSettings gameSettings;

    [Header("Game Events")]
    public UnityEvent OnLevelStart;
    public UnityEvent OnPlayerScore;
    public UnityEvent OnRoundEnd;

    public List<PlayerColors> playerColors;

    GameObject player1;
    GameObject player2;

    PlayerController player1Controller;
    PlayerController player2Controller;

    bool camViewInPos2 = false;

    void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else
            instance = this;

        playerColors = new List<PlayerColors>(gameSettings.colorList);
    }

    void Start()
    {
        if (currentPlaymode == GamePlay.SinglePlayer)
            SinglePlayerSpawn();
        else
            TwoPlayerSpawn();
    }

    private void SinglePlayerSpawn()
    {
        color1 = SelectColor();
        color2 = SelectColor();
        GameObject ballInstance = Instantiate(ballPrefab) as GameObject;

        player1 = Instantiate(player1Prefab, player1Spawn.position, player1Spawn.rotation) as GameObject;
        player1Controller = player1.GetComponent<PlayerController>();
        player1Controller.playerSelection = PlayerController.PlayerSelection.Player1;
        player1Controller.SetColor(color1);
        player1Controller.ballTarget = ballInstance;

        player2 = Instantiate(AiPlayerPrefab, player2Spawn.position, player2Spawn.rotation) as GameObject;
        AIController player2Controller = player2.GetComponent<AIController>();
        player2Controller.SetColor(color2);
        player2Controller.ballTarget = ballInstance;
    }

    private void TwoPlayerSpawn()
    {
        color1 = SelectColor();
        color2 = SelectColor();
        GameObject ballInstance = Instantiate(ballPrefab) as GameObject;

        player1 = Instantiate(player1Prefab, player1Spawn.position, player1Spawn.rotation) as GameObject;
        player1Controller = player1.GetComponent<PlayerController>();
        player1Controller.playerSelection = PlayerController.PlayerSelection.Player1;
        player1Controller.SetColor(color1);
        player1Controller.ballTarget = ballInstance;

        player2 = Instantiate(player2Prefab, player2Spawn.position, player2Spawn.rotation) as GameObject;
        player2Controller = player2.GetComponent<PlayerController>();
        player2Controller.playerSelection = PlayerController.PlayerSelection.Player2;
        player2Controller.SetColor(color2);
        player2Controller.ballTarget = ballInstance;
    }

    private PlayerColors SelectColor()
    {
        int selectionindex = UnityEngine.Random.Range(0, playerColors.Count);
        PlayerColors selectedColor = playerColors[selectionindex];
        playerColors.RemoveAt(selectionindex);
        return selectedColor;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeCameraPositions();
    }

    void ShowMainMenu()
    {

    }

    void LoadHud()
    {

    }

    //Resets Scene for next match
    void ResetNextMatch()
    {
        player1.transform.position = player1Spawn.position;
        player1.transform.rotation = player1Spawn.rotation;

        player2.transform.position = player2Spawn.position;
        player2.transform.rotation = player2Spawn.rotation;

        if (!camViewInPos2)
        {
            mainCamera.transform.position = cameraPosition2.position;
            mainCamera.transform.rotation = cameraPosition2.rotation;
            InvertSingleController();
            InvertingDoubleController();
        }
        else
        {
            mainCamera.transform.position = cameraPosition1.position;
            mainCamera.transform.rotation = cameraPosition1.rotation;
            InvertSingleController();
            InvertingDoubleController();
        }
    }

    private void InvertSingleController()
    {
        if (currentPlaymode == GamePlay.SinglePlayer)
        {
            player1Controller.controlModifier *= -1;
        }
    }

    private void InvertingDoubleController()
    {
        if (currentPlaymode == GamePlay.DoublePlayer)
        {
            player1Controller.controlModifier *= -1;
            player2Controller.controlModifier *= -1;
        }
    }

    //Test Methods
    void ChangeCameraPositions()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mainCamera.transform.position = cameraPosition1.position;
            mainCamera.transform.rotation = cameraPosition1.rotation;

            InvertSingleController();
            InvertingDoubleController();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mainCamera.transform.position = cameraPosition2.position;
            mainCamera.transform.rotation = cameraPosition2.rotation;

            InvertSingleController();
            InvertingDoubleController();
        }
    }
}
