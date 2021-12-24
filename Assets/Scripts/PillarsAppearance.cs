using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PillarsAppearance : MonoBehaviour, IBorder
{
    [SerializeField] private bool hasBorder;
    
    [SerializeField] private SpriteRenderer groundSpriteRenderer;
    [SerializeField] private Sprite groundNoBorderSprite;
    [SerializeField] private Sprite groundBorderSprite;

    private bool _appearanceUpdateNeeded = false;
    
    private void Start()
    {
        UpdateAppearance();
    }

    /// <summary>
    /// When a value changes on Inspector, mark brick to update appearance.
    /// </summary>
    private void OnValidate()
    {
        _appearanceUpdateNeeded = true;
    }

    /// <summary>
    /// Update appearance if needed.
    /// </summary>
    private void LateUpdate()
    {
        #if UNITY_EDITOR
        if (_appearanceUpdateNeeded)
        {
            UpdateAppearance();
            _appearanceUpdateNeeded = false; 
        }
        #endif
    }


    /// <summary>
    /// Update this prefab sprites according to given fields.
    /// </summary>
    private void UpdateAppearance()
    {
        SetGround();
    }
    
    /// <summary>
    /// Show ground if this brick has one.
    /// </summary>
    private void SetGround()
    {
        if (hasBorder)
        {
            groundSpriteRenderer.sprite = groundBorderSprite;
        }
        else 
        {
            groundSpriteRenderer.sprite = groundNoBorderSprite;
        }
    }

    public bool IsBorderShown()
    {
        return hasBorder;
    }

    public void ShowBorder(bool showIt)
    {
        hasBorder = showIt;
    }
}
