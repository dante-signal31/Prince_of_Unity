using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThingsSpritesOverGround", menuName = "ScriptableObjects/ThingsOverGroundSprites", order = 0)]
public class ThingsOverGroundSprites : ScriptableObject
{
    public Dictionary<string, Sprite> ThingsSprites;
}