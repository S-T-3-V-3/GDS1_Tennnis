using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Object Settings/ Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Colour Selection")]
    public List<PlayerColors> colorList;
}

[System.Serializable]
public struct PlayerColors
{
    public string colorName;
    public Material playerColor;
    public Material groundColor;
    public Color textColour;
}