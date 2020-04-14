using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public SessionData sessionData;
    public GamePlay currentPlaymode = GamePlay.SinglePlayer;

    [Header("Prefabs")]
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public GameObject AiPlayerPrefab;
    public GameObject ballPrefab;

    [Header("Scene Components")]
    public HUDManager hud;

    [HideInInspector]
    public GameObject currentBall;  

    [Header("Respawn")]
    public Transform player1Spawn;
    public Transform player2Spawn;

    [Header("Powerup Variables")]
    //[Tooltip("This sets the spawn bound area for powerups on each respective side")]
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
    GameObject player1;
    GameObject player2;

    PlayerController player1Controller;
    PlayerController player2Controller;

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

        OnGameComplete.AddListener(ShowGameCompleted);
    }

    void Start()
    {
        OnRoundComplete.AddListener(StartNewRound);
        OnSetComplete.AddListener(StartNewMatch);
        OnSetComplete.AddListener(TriggerPowerupSpawner);

        Spawn(currentPlaymode == GamePlay.SinglePlayer ? false : true);
        StartNewMatch();
    }

    void SpawnBall(Vector3 pos) {
        if (currentBall != null)
            GameObject.Destroy(currentBall);

        currentBall = GameObject.Instantiate(ballPrefab);
        currentBall.transform.position = pos;
    }

    void Spawn(bool twoPlayer = false) {
        color1 = SelectColor();
        color2 = SelectColor();

        player1 = GameObject.Instantiate(player1Prefab, player1Spawn.position, player1Spawn.rotation);
        player1Controller = player1.GetComponent<PlayerController>();
        player1Controller.playerSelection = PlayerController.PlayerSelection.Player1;
        player1Controller.SetColor(color1);
        player1Controller.currentTeam = color1.team;

        if (twoPlayer) {
            player2 = GameObject.Instantiate(player2Prefab, player2Spawn.position, player2Spawn.rotation);
            player2Controller = player2.GetComponent<PlayerController>();
            player2Controller.playerSelection = PlayerController.PlayerSelection.Player2;
            player2Controller.SetColor(color2);
            player2Controller.currentTeam = color2.team;
        }
        else {
            player2 = GameObject.Instantiate(AiPlayerPrefab, player2Spawn.position, player2Spawn.rotation);
            AIController player2Controller = player2.GetComponent<AIController>();
            player2Controller.SetColor(color2);
            player2Controller.currentTeam = color2.team;
        }
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
        Team loosingTeam = sessionData.GetLosingTeam();

        if (currentPlaymode == GamePlay.SinglePlayer)
        {
            if (player1Controller.currentTeam == loosingTeam)
            {
                float xPos = Random.Range(courtCenter1.x, player1.transform.position.x + 0.5f);
                float zPos = Random.Range(courtCenter1.z, player1.transform.position.z + 0.5f);
                Vector3 spawnPos = new Vector3(xPos, courtCenter1.y, zPos);
                Instantiate(powerUpPrefab, spawnPos, Quaternion.identity);
            }
        }

        if (currentPlaymode == GamePlay.DoublePlayer)
        {
            if (player1Controller.currentTeam == loosingTeam)
            {
                float xPos = Random.Range(courtCenter1.x, player1.transform.position.x + 0.5f);
                float zPos = Random.Range(courtCenter1.z, player1.transform.position.z + 0.5f);
                Vector3 spawnPos = new Vector3(xPos, courtCenter1.y, zPos);
                Instantiate(powerUpPrefab, spawnPos, Quaternion.identity);
            }

            if (player2Controller.currentTeam == loosingTeam)
            {
                float xPos = Random.Range(courtCenter2.x, player2.transform.position.x + 0.5f);
                float zPos = Random.Range(courtCenter2.z, player2.transform.position.z + 0.5f);
                Vector3 spawnPos = new Vector3(xPos, courtCenter2.y, zPos);
                Instantiate(powerUpPrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    #endregion

    //Resets Scene for next match
    public void StartNewMatch()
    {
        player1.transform.position = player1Spawn.position;
        player1.transform.rotation = player1Spawn.rotation;

        player2.transform.position = player2Spawn.position;
        player2.transform.rotation = player2Spawn.rotation;

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

        sessionData.StartGame();
        StartNewRound();
    }

    void StartNewRound() {
        Team currentServer = sessionData.GetCurrentServer();
        Transform currentServerTransform = player1Controller.currentTeam == currentServer ? player1.transform : player2.transform;

        Vector3 ballSpawnPos = currentServerTransform.position + currentServerTransform.forward * gameSettings.ballSpawnDistance + new Vector3(0,1,0);
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