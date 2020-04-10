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


    //Additional Temporary Variables
    //Make Sure to insert these variables
    public Transform nearestRespawn; //This can be stored into a scriptable object
    public Transform farthestRespawn;

    public GameObject player1Prefab;
    public GameObject player2Prefab;

    [Header("Game Events")]
    public UnityEvent OnLevelStart;
    public UnityEvent OnPlayerScore;
    public UnityEvent OnRoundEnd;

    void Awake()
    {
        if(instance != null)
        {
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject player1 = Instantiate(player1Prefab, nearestRespawn.position, nearestRespawn.rotation) as GameObject;
        PlayerController player1Controller = player1.GetComponent<PlayerController>();
        player1Controller.playerSelection = PlayerController.PlayerSelection.Player1;

        GameObject player2 = Instantiate(player2Prefab, farthestRespawn.position, farthestRespawn.rotation) as GameObject;
        PlayerController player2Controller = player1.GetComponent<PlayerController>();
        player2Controller.playerSelection = PlayerController.PlayerSelection.Player2;
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
