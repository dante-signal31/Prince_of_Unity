// Customize wall cracks appearance.

using System;
using UnityEngine;

[ExecuteAlways]
public class WallCrackAppearance : MonoBehaviour
{
    [SerializeField] private SpriteRenderer wallCrackSpriteRenderer;
    [SerializeField] private WallCracks wallCrackType;
    [SerializeField] private WallCrackSprites wallCrackSprites;
    private bool _appearanceUpdatedNeeded = false;
    
    /// <summary>
    /// Update this prefab sprites according to given fields.
    /// </summary>
    private void UpdateAppearance()
    {
        SetCrack();
    }

    /// <summary>
    /// Update this crack sprite depending of WallCrackTypeSet.
    /// </summary>
    private void SetCrack()
    {
        Sprite crackSprite = wallCrackType switch
        {
            WallCracks.Type1 => wallCrackSprites["Type1"],
            WallCracks.Type2 => wallCrackSprites["Type2"],
            WallCracks.Type3 => wallCrackSprites["Type3"],
            WallCracks.Type4 => wallCrackSprites["Type4"],
            _ => throw new ArgumentOutOfRangeException(nameof(wallCrackType), wallCrackType, null)
        };
        wallCrackSpriteRenderer.sprite = crackSprite;
    }

    private void Start()
    {
        UpdateAppearance();
    }

    /// <summary>
    /// When a value changes on Inspector, mark brick to update appearance.
    /// </summary>
    private void OnValidate()
    {
        _appearanceUpdatedNeeded = true;
    }

    /// <summary>
    /// When a value changes on Inspector, update brick appearance.
    /// </summary>
    private void LateUpdate()
    {
        #if UNITY_EDITOR
        if (_appearanceUpdatedNeeded)
        {
            UpdateAppearance();
            _appearanceUpdatedNeeded = false;
        }
        #endif
    }
    
    
}