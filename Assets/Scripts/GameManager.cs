using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;

    public SessionData sessionData;

    [Header("Scene Components")]
    public HUDManager hud;

    [Space]
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
    public Transform player1Spawn;
    public Transform player2Spawn;
    public Transform ballSpawnPos;

    [Header("Powerup Variables")]
    //[Tooltip("This sets the spawn bound area for powerups on each respective side")]
    public GameObject powerUpPrefab;
    public Vector3 courtCenter1;
    public Vector3 courtCenter2;
    //public Vector3 boundSize;

    [Header("Camera")]
    public Camera mainCamera;

    [Header("Camera Positions")]
    public Transform cameraPosition1;
    public Transform cameraPosition2;

    [Header("Playable Prefabs")]
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public GameObject AiPlayerPrefab;

    [Header("Audio Prefab")]
    public GameObject AudioPrefab;
    public GameObject AudioManager;

    [Header("Game Settings")]
    public GameSettings gameSettings;

    [Header("Game Events")]
    public UnityEvent OnLevelStart;
    public TeamEvent OnPlayerScore;
    public UnityEvent OnSetComplete;
    public UnityEvent OnGameComplete;

    public List<PlayerColors> playerColors;

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
        AudioManager = Instantiate(AudioPrefab) as GameObject;

        if (currentPlaymode == GamePlay.SinglePlayer)
            SinglePlayerSpawn();
        else
            TwoPlayerSpawn();

        OnSetComplete.AddListener(ResetNextMatch);
        OnSetComplete.AddListener(TriggerPowerupSpawner);
    }

    private void SinglePlayerSpawn()
    {
        color1 = SelectColor();
        color2 = SelectColor();
        GameObject ballInstance = Instantiate(ballPrefab, ballSpawnPos) as GameObject;

        player1 = Instantiate(player1Prefab, player1Spawn.position, player1Spawn.rotation) as GameObject;
        player1Controller = player1.GetComponent<PlayerController>();
        player1Controller.playerSelection = PlayerController.PlayerSelection.Player1;
        player1Controller.SetColor(color1);
        player1Controller.ballTarget = ballInstance;
        player1Controller.currentTeam = color1.team;

        player2 = Instantiate(AiPlayerPrefab, player2Spawn.position, player2Spawn.rotation) as GameObject;
        AIController player2Controller = player2.GetComponent<AIController>();
        player2Controller.SetColor(color2);
        player2Controller.ballTarget = ballInstance;
        player2Controller.currentTeam = color2.team;

        sessionData.isStarted = true;
    }

    private void TwoPlayerSpawn()
    {
        color1 = SelectColor();
        color2 = SelectColor();
        GameObject ballInstance = Instantiate(ballPrefab, ballSpawnPos) as GameObject;

        player1 = Instantiate(player1Prefab, player1Spawn.position, player1Spawn.rotation) as GameObject;
        player1Controller = player1.GetComponent<PlayerController>();
        player1Controller.playerSelection = PlayerController.PlayerSelection.Player1;
        player1Controller.SetColor(color1);
        player1Controller.ballTarget = ballInstance;
        player1Controller.currentTeam = color1.team;

        player2 = Instantiate(player2Prefab, player2Spawn.position, player2Spawn.rotation) as GameObject;
        player2Controller = player2.GetComponent<PlayerController>();
        player2Controller.playerSelection = PlayerController.PlayerSelection.Player2;
        player2Controller.SetColor(color2);
        player2Controller.ballTarget = ballInstance;
        player2Controller.currentTeam = color2.team;

        sessionData.isStarted = true;
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
        Team loosingTeam = sessionData.GetLoosingTeam();

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
    public void ResetNextMatch()
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

            InvertSingleController();
            InvertingDoubleController();
        }
        else
        {
            mainCamera.transform.position = cameraPosition1.position;
            mainCamera.transform.rotation = cameraPosition1.rotation;

            camViewInPos2 = false;

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