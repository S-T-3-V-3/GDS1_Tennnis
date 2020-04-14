using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public SessionData sessionData;
    public GamePlay currentPlaymode = GamePlay.SinglePlayer;


    [Header("Prefabs")]
    public GameObject AudioPrefab;
    public GameObject playerPrefab;
    public GameObject ballPrefab;


    [Header("Scene Components")]
    public HUDManager hud;
    public GameObject AudioManager;
    [HideInInspector]
    public GameObject currentBall; 
    [HideInInspector]
    public PlayerController player1Controller;
    [HideInInspector]
    public PlayerController player2Controller; 


    [Header("Respawn")]
    public Transform player1Spawn;
    public Transform player2Spawn;


    [Header("Powerup Variables")]
    public GameObject powerUpPrefab;
    public Vector3 courtCenter1;
    public Vector3 courtCenter2;


    [Header("Camera")]
    public Camera mainCamera;
    public Transform cameraPosition1;
    public Transform cameraPosition2;


    [Header("Settings")]
    public GameSettings gameSettings;


    [Header("Events")]
    public UnityEvent OnLevelStart;
    public TeamEvent OnPlayerScore;
    public UnityEvent OnRoundComplete;
    public UnityEvent OnSetComplete;
    public UnityEvent OnGameComplete;

    public List<PlayerColors> playerColors;

    PlayerColors color1;
    PlayerColors color2;

    bool camViewInPos2 = false;

    float powerupSpawnCounter = 0;

    void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        sessionData = gameObject.AddComponent<SessionData>();

        playerColors = new List<PlayerColors>(gameSettings.colorList);

        AudioManager = GameObject.Instantiate(AudioPrefab);

        OnRoundComplete.AddListener(StartNewRound);
        OnSetComplete.AddListener(StartNewSet);
        OnSetComplete.AddListener(TriggerPowerupSpawner);
        OnGameComplete.AddListener(ShowGameCompleted);

        hud.scoreBoard.gameManager = this;
    }

    void Start()
    {
        SpawnPlayers();
        sessionData.StartGame();
        StartNewSet();
    }

    void SpawnBall(Vector3 pos) {
        if (currentBall != null)
            GameObject.Destroy(currentBall);

        currentBall = GameObject.Instantiate(ballPrefab);
        currentBall.transform.position = pos;
    }

    void SpawnPlayers() {
        player1Controller = CreateNewPlayer(PlayerSelection.Player1);
        player2Controller = CreateNewPlayer(PlayerSelection.Player2, currentPlaymode == GamePlay.SinglePlayer);
    }

    PlayerController CreateNewPlayer(PlayerSelection playerNumber, bool isAIControlled = false) {
        PlayerController controller = GameObject.Instantiate(playerPrefab, player1Spawn.position, player1Spawn.rotation).GetComponent<PlayerController>();
        PlayerColors color = SelectColor();
        
        controller.playerSelection = playerNumber;
        controller.SetColor(color);
        controller.currentTeam = color.team;
        
        if (isAIControlled)
            controller.SetState<PlayerAIState>();
        else
            controller.SetState<PlayerControlledState>();

        return controller;
    }

    private PlayerColors SelectColor()
    {
        int selectionindex = 0;
        PlayerColors selectedColor = playerColors[selectionindex];
        playerColors.RemoveAt(selectionindex);
        return selectedColor;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeCameraPositions();
        TestSpawnPowerup();
    }

    void ShowMainMenu()
    {
        hud.ShowMainMenu();
        sessionData.isStarted = false;
    }

    void ShowGameCompleted() {

    }

    #region SPAWN POWERUP

    void TriggerPowerupSpawner()
    {
        int randTime = Random.Range(1, 15);
        powerupSpawnCounter = randTime + Time.time;

        Invoke("SpawnPowerup", powerupSpawnCounter);
    }

    void SpawnPowerup() //Spawns once
    {
        Team losingTeam = sessionData.GetLosingTeam();

        if (currentPlaymode == GamePlay.SinglePlayer)
        {
            if (player1Controller.currentTeam == losingTeam)
            {
                float xPos = Random.Range(courtCenter1.x, player1Controller.transform.position.x + 0.5f);
                float zPos = Random.Range(courtCenter1.z, player1Controller.transform.position.z + 0.5f);
                Vector3 spawnPos = new Vector3(xPos, courtCenter1.y, zPos);
                Instantiate(powerUpPrefab, spawnPos, Quaternion.identity);
            }
        }

        if (currentPlaymode == GamePlay.DoublePlayer)
        {
            if (player1Controller.currentTeam == losingTeam)
            {
                float xPos = Random.Range(courtCenter1.x, player1Controller.transform.position.x + 0.5f);
                float zPos = Random.Range(courtCenter1.z, player1Controller.transform.position.z + 0.5f);
                Vector3 spawnPos = new Vector3(xPos, courtCenter1.y, zPos);
                Instantiate(powerUpPrefab, spawnPos, Quaternion.identity);
            }

            if (player2Controller.currentTeam == losingTeam)
            {
                float xPos = Random.Range(courtCenter2.x, player2Controller.transform.position.x + 0.5f);
                float zPos = Random.Range(courtCenter2.z, player2Controller.transform.position.z + 0.5f);
                Vector3 spawnPos = new Vector3(xPos, courtCenter2.y, zPos);
                Instantiate(powerUpPrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    #endregion

    //Resets Scene for next match
    public void StartNewSet()
    {
        if (!camViewInPos2)
        {
            mainCamera.transform.position = cameraPosition2.position;
            mainCamera.transform.rotation = cameraPosition2.rotation;

            camViewInPos2 = true;
        }
        else
        {
            mainCamera.transform.position = cameraPosition1.position;
            mainCamera.transform.rotation = cameraPosition1.rotation;

            camViewInPos2 = false;
        }

        StartNewRound();
    }

    void StartNewRound() {
        player1Controller.transform.position = player1Spawn.position;
        player1Controller.transform.rotation = player1Spawn.rotation;

        player2Controller.transform.position = player2Spawn.position;
        player2Controller.transform.rotation = player2Spawn.rotation;

        Team currentServer = sessionData.GetCurrentServer();
        Transform currentServerTransform = player1Controller.currentTeam == currentServer ? player1Controller.transform : player2Controller.transform;

        Vector3 ballSpawnPos = currentServerTransform.position + currentServerTransform.forward * gameSettings.ballSpawnDistance + new Vector3(0,0.4f,0);
        SpawnBall(ballSpawnPos);
    }

    //Test Methods
    void ChangeCameraPositions()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            mainCamera.transform.position = cameraPosition1.position;
            mainCamera.transform.rotation = cameraPosition1.rotation;

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            mainCamera.transform.position = cameraPosition2.position;
            mainCamera.transform.rotation = cameraPosition2.rotation;
        }
    }

    //Test Functions
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(courtCenter1, 0.5f);
    }

    private void TestSpawnPowerup()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnPowerup();
        }
    }

}

public enum GamePlay
{
    SinglePlayer,
    DoublePlayer
}

[System.Serializable]
public class TeamEvent : UnityEvent<Team> {};