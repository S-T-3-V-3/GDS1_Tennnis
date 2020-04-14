using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAIState : State
{   
    PlayerController playerController;
    MeshRenderer meshRenderer;
    GameManager gameManager;
    GameObject playerModel;
    BallBehaviour ballBehaviour;
    Rigidbody currentBallRB;
    BallBehaviour currentBall;
    Rigidbody playerRB;
    Team currentTeam;
    PlayerController otherPlayer;

    public bool hasArrivedTarget = false;
    public bool hasTarget = false;
    public bool seekingBall = false;
    public bool hasServed = false;
    float maxZBound;
    float boundXLimit;
    float futureScalar = 2f;
    float xDirectionModifier;

    [SerializeField]
    Vector3 targetPos;
    float spawnDelay = 0.75f;

    public override void BeginState()
    {
        playerController = this.gameObject.GetComponent<PlayerController>();

        currentTeam = playerController.currentTeam;
        playerModel = playerController.playerModel;
        meshRenderer = playerModel.GetComponent<MeshRenderer>();
        gameManager = GameManager.Instance;

        maxZBound = transform.position.z + 0.5f;
        boundXLimit = gameManager.gameSettings.aiBoundXLimit;

        gameManager.OnRoundBegin.AddListener(() => {
            hasTarget = false;
            seekingBall = false;
            hasServed = false;
            spawnDelay = 0.75f;
        });

        playerRB = this.GetComponent<Rigidbody>();
    }

void FixedUpdate()
    {
        if (spawnDelay > 0) {
            spawnDelay -= Time.deltaTime;
            return;
        }

        if (currentBallRB == null)
        {
            GetBallRef();
            return;
        }

        ServeBall();
        LookAtBall();
        MoveToTarget();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BallBehaviour>() == null) return;

        GameManager.Instance.AudioManager.GetComponent<AudioManager>().PlaySound("Hit");

        ballBehaviour = collision.gameObject.GetComponent<BallBehaviour>();
        Vector3 dir = Vector3.Normalize(this.transform.position - gameManager.currentBall.transform.position);
        ballBehaviour.ReturnBall(dir, this.playerController);

        hasTarget = false;
        Invoke("FindTargetPosition",1.5f);

        if (playerController.isServing && hasServed == false)
            hasServed = true;

        this.playerRB.velocity = this.playerRB.velocity * 0.1f;
    }

    void GetBallRef() {
        if (gameManager.currentBall == null) return;

        currentBallRB = gameManager.currentBall.GetComponent<Rigidbody>();
        currentBall = currentBallRB.GetComponent<BallBehaviour>();
    }

    void LookAtBall()
    {
        if (gameManager.currentBall == null) return;

        playerModel.transform.LookAt(gameManager.currentBall.transform.position);
        playerModel.transform.rotation = Quaternion.Euler(0,playerModel.transform.rotation.eulerAngles.y,0);
    }

    void MoveToTarget()
    {
        if (!hasArrivedTarget)
        {
            if (hasTarget == false)
                FindTargetPosition();

            if (hasTarget == false)
                return;

            Vector3 dir = (targetPos - transform.position).normalized;
            playerRB.MovePosition(this.transform.position + dir * playerController.baseSpeed * Time.fixedDeltaTime);

            if ((transform.position - targetPos).magnitude < 0.3f)
            {
                hasArrivedTarget = true;
            }
        }
        else if (seekingBall == false) {
            FindTargetPosition();
        }
    }

    void ServeBall()
    {
        if (playerController.isServing && hasServed == false)
        {
            targetPos = currentBallRB.transform.position;
            targetPos.x += Random.Range(-1.5f,1.5f);
            targetPos.y = transform.position.y;
            hasTarget = true;
            seekingBall = true;
            hasArrivedTarget = false;
        }
    }

    void FindTargetPosition()
    {
        if (currentBall.lastHitter == null) return;
        
        hasArrivedTarget = false;
        
        if (seekingBall) {
            seekingBall = false;
            targetPos = new Vector3(0,transform.position.y, transform.position.z);
            hasTarget = true;
        }
        else if (currentBall.lastHitter == this.playerController) {
            hasTarget = false;
            return;
        }
        else if (currentBallRB.velocity.magnitude > 1) {
            targetPos = currentBallRB.transform.position + currentBallRB.velocity * futureScalar;
            targetPos.y = transform.position.y;
            hasTarget = true;
            seekingBall = true;
        }
    }

    //Remove until Final
    private void PongMovement()
    {
        Vector3 ballPos = currentBallRB.transform.localPosition;
        Vector3 ballPosX = new Vector3(ballPos.x, 0, 0);
        Vector3 AIPosX = new Vector3(transform.position.x, 0, 0);
        transform.position = new Vector3(ballPos.x * playerController.baseSpeed * 0.5f, transform.position.y, transform.position.z);
    }
}
