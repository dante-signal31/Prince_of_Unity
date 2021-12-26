// Customize if any rubbish should appear over ground and its appearance.

using System;
using UnityEngine;

[ExecuteAlways]
public class GroundRubbishAppearance : MonoBehaviour
{


    [SerializeField] private ThingsOverGroundSprites placeableThingsSprites;

    [SerializeField] private SpriteRenderer thingsOverGroundSpriteRenderer;
    [SerializeField] private SpriteRenderer thingsOverGroundOccluderSpriteRenderer;

    [SerializeField] private bool _shown;

    public bool ShowRubbish
    {
        get => _shown;
        set
        {
            ShowSprites(value);
            _shown = value;
        }
    }

    /// <summary>
    /// Show (or not) rubbish sprites.
    /// </summary>
    /// <param name="show">True to shown them, false if not.</param>
    private void ShowSprites(bool show)
    {
        thingsOverGroundSpriteRenderer.enabled = show;
        thingsOverGroundOccluderSpriteRenderer.enabled = show;
    }
    
    /// <summary>
    /// Show rubbish (or nothing) over parent ground parent.
    /// </summary>
    /// <param name="thing">Rubbish type to show.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void PlaceThingsOverGround(ThingsOverGround thing)
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
}