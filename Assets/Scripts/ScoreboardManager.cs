﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour
{
    public AudioSource audioSource;
    private float bounce = 0;
    private bool stillScoring = true;
    
    //Stores the player number of the last ball hitter
    private int lastHitter = 2;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PointScored(int lastHitter)
    {
        // ===================================
        // This should be the param passed in
        // instead of an integer
        Team team = Team.BLUE;
        // The code below will then handle the
        // scoring/rest of game based on the
        // team that scored.
        // ===================================

        GameManager.Instance.OnPlayerScore.Invoke(team);

        // ========================================
        //TODO: CHECK IF GAME POINT IS MADE
        //TODO: CHECK IF DUECE OCCURS
        // The score class handles all of this! :) 
        // Go take a look and see if you need to make any changes
        // ========================================

        audioSource.Play();

        //TODO: RESET ROUND
        //TODO: SET stillScoring BACK TO TRUE
    }

    public void ResetBounceCounter()
    {
        if(stillScoring)
            bounce = 0;
    }

    //Updates
    public void PlayerOneHit(bool playerOneHit)
    {
        if (playerOneHit)
            lastHitter = 1;
        else
            lastHitter = 2;
    }

    public void AddBounceCounter(bool insideCourt, float zBallPosition)
    {
        if (stillScoring)
        {
            bounce++;
            //If the ball lands outside the court without bouncing first, give opposition to the hitter the point
            if (!insideCourt && bounce < 2)
            {
                if (lastHitter == 1)
                    PointScored(lastHitter + 1);
                else
                    PointScored(lastHitter - 1);

                stillScoring = false;
            }

            //If ball doesn't make it over the net
            if (insideCourt && bounce < 2)
            {
                if (lastHitter == 2 && zBallPosition > 0)
                {
                    PointScored(lastHitter - 1);
                    stillScoring = false;
                }
                    
                else if (lastHitter == 1 && zBallPosition < 0)
                {
                    PointScored(lastHitter + 1);
                    stillScoring = false;
                }
            }

            //If the ball bounces twice on a player's side, give point to opposition
            if (bounce == 2)
            {
                PointScored(lastHitter);
                stillScoring = false;
            }
        } 
    }
}