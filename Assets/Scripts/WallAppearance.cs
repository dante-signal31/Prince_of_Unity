using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// Set up a wall brick to have desired behaviour.
/// </summary>
[ExecuteAlways]
public class WallAppearance : MonoBehaviour, IBorder, IGround
{
    [SerializeField] private bool _hasCutWall;
    [SerializeField] private bool _hasGround;
    [SerializeField] private bool _hasBorder;
    [SerializeField] private bool randomizeFrontWall = true;
    [SerializeField] private ThingsOverGround _thingsOverGround;

    [SerializeField] private SpriteRenderer frontWallSpriteRenderer;
    [SerializeField] private SpriteRenderer groundSpriteRenderer;
    [SerializeField] private SpriteRenderer groundOccluderSpriteRenderer;
    [SerializeField] private SpriteRenderer cutWallSpriteRenderer;
    [SerializeField] private SpriteRenderer thingsOverGroundSpriteRenderer;
    [SerializeField] private SpriteRenderer thingsOverGroundOccluderSpriteRenderer;
    [SerializeField] private Sprite groundNoBorderSprite;
    [SerializeField] private Sprite groundBorderSprite;
    [SerializeField] private ThingsOverGroundSprites placeableThingsSprites;
    
    [SerializeField] private Sprite[] frontWallOptions;
    
    public bool IsBorderShown()
    {
        return _hasBorder;
    }

    public void ShowBorder(bool showIt)
    {
        _hasBorder = showIt;
    }
    
    /// <summary>
    /// Update this prefab sprites according to given fields.
    /// </summary>
    /// <param name="randomize">If true front wall is a random sprite from frontWallOptions. If false default sprite is used.</param>
    private void UpdateAppearance(bool randomize=false)
    {
        SetGround();
        SetCutwall();
        PlaceThingsOverGround(_thingsOverGround);
        if (randomize) RandomizeFrontWall();
    }

    /// <summary>
    /// Show ground if this brick has one.
    /// </summary>
    private void SetGround()
    {
        if (_hasGround && _hasBorder)
        {
            groundSpriteRenderer.sprite = groundBorderSprite;
        }
        else if (_hasGround)
        {
            groundSpriteRenderer.sprite = groundNoBorderSprite;
        }
        groundSpriteRenderer.enabled = _hasGround;
        groundOccluderSpriteRenderer.enabled = _hasGround;
        
    }

    /// <summary>
    /// Show a cutwall if this brick has one.
    /// </summary>
    private void SetCutwall()
    {
        cutWallSpriteRenderer.enabled =  _hasCutWall;
    }
    
    public void PlaceThingsOverGround(ThingsOverGround thing)
    {
        if (_hasGround)
        {
            OccludedSprite occludedThingSprite = thing switch
            {
                ThingsOverGround.Nothing => placeableThingsSprites["Nothing"],
                ThingsOverGround.Bones => placeableThingsSprites["Bones"],
                ThingsOverGround.Garbage => placeableThingsSprites["Garbage"],
                _ => throw new ArgumentOutOfRangeException(nameof(thing), thing, null)
            };
            if (occludedThingSprite.main != null)
            {
                thingsOverGroundSpriteRenderer.sprite = occludedThingSprite.main;
                thingsOverGroundSpriteRenderer.enabled = true;
            }
            else
            {
                thingsOverGroundSpriteRenderer.enabled = false;
            }

            if (occludedThingSprite.occluder != null)
            {
                thingsOverGroundOccluderSpriteRenderer.sprite = occludedThingSprite.occluder;
                thingsOverGroundOccluderSpriteRenderer.enabled = true;
            }
            else
            {
                thingsOverGroundOccluderSpriteRenderer.enabled = false;
            }
        }
        else
        {
            thingsOverGroundSpriteRenderer.enabled = false;
            thingsOverGroundOccluderSpriteRenderer.enabled = false;
        }
    }

    /// <summary>
    /// Set a random front wall.
    /// </summary>
    private void RandomizeFrontWall()
    {
        Random rnd = new Random();
        int selected_index = rnd.Next(0, frontWallOptions.Length);
        frontWallSpriteRenderer.sprite = frontWallOptions[selected_index];
    }

    private void Start()
    {
        UpdateAppearance(randomizeFrontWall);
    }

    /// <summary>
    /// When a value changes on Inspector, update brick appearance.
    /// </summary>
    private void OnValidate()
    {
        UpdateAppearance(randomizeFrontWall);
    }
}