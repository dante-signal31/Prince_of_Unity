using System;
using UnityEngine;

public class GroundRubbishAppearance : MonoBehaviour
{


    [SerializeField] private ThingsOverGroundSprites placeableThingsSprites;

    [SerializeField] private SpriteRenderer thingsOverGroundSpriteRenderer;
    [SerializeField] private SpriteRenderer thingsOverGroundOccluderSpriteRenderer;

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