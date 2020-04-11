using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    //Test temporary variables
    [HideInInspector] public int scoreP1;
    [HideInInspector] public int setWinsP1;

    [HideInInspector] public int scoreP2;
    [HideInInspector] public int setWinsP2;

    PlayerColors color1;
    PlayerColors color2;

    //Additional Temporary Variables
    //Make Sure to insert these variables
    public Transform nearestRespawn; //This can be stored into a scriptable object
    public Transform farthestRespawn;

    public GameObject player1Prefab;
    public GameObject player2Prefab;

    [Header("Game Settings")]
    public GameSettings gameSettings;

    [Header("Game Events")]
    public UnityEvent OnLevelStart;
    public UnityEvent OnPlayerScore;
    public UnityEvent OnRoundEnd;

    public List<PlayerColors> playerColors;

    void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else
            instance = this;

        playerColors = new List<PlayerColors>(gameSettings.colorList);
    }

    // Start is called before the first frame update
    void Start()
    {
        color1 = SelectColor();
        color2 = SelectColor();
        
        GameObject player1 = Instantiate(player1Prefab, nearestRespawn.position, nearestRespawn.rotation) as GameObject;
        PlayerController player1Controller = player1.GetComponent<PlayerController>();
        player1Controller.playerSelection = PlayerController.PlayerSelection.Player1;
        player1Controller.SetColor(color1);

        GameObject player2 = Instantiate(player2Prefab, farthestRespawn.position, farthestRespawn.rotation) as GameObject;
        PlayerController player2Controller = player2.GetComponent<PlayerController>();
        player2Controller.playerSelection = PlayerController.PlayerSelection.Player2;
        player2Controller.SetColor(color2);
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
        
    }

    void ShowMainMenu()
    {

    }

    void LoadHud()
    {

    }

    void ResetMatch()
    {

    }
}
