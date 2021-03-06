﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Object Settings/ Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Colour Selection")]
    public List<PlayerColors> colorList;

    [Header("Game Variables")]
    public List<string> scoreValues;
    [Range(0,100)]
    public float predictionDistanceScalar = 2f;
    public float baseHitPower = 5f;
    public float powerupSpeedMultiplier = 1.5f;
    public float maxBallVelocity = 100f;
    public float ballSpawnDistance = 3f;

    [Header("AI Settings")]
    public float aiBoundXLimit = 13.5f;
}

[System.Serializable]
public struct PlayerColors
{
    public string colorName;
    public Material playerColor;
    public Material groundColor;
    public Color textColour;
    public Team team;
}

public enum Team {
    GREEN,
    BLUE
}