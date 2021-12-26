// Scriptable object to pack every possible rubbish name, and its respective sprite.

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just a data container with two fields:
/// * Main: Current occluded_sprite.
/// * Occluder: Sprite used as occluder of main one.
/// </summary>
[Serializable]
public class OccludedSprite
{
    public Sprite main;
    public Sprite occluder;

    public OccludedSprite(Sprite main, Sprite occluder)
    {
        this.main = main;
        this.occluder = occluder;
    }
}


[Serializable]
public class NamedSprite
{
    public string name;
    public OccludedSprite occluded_sprite;

    public NamedSprite(string name, OccludedSprite occludedSprite)
    {
        this.name = name;
        this.occluded_sprite = occludedSprite;
    }
}

/// <summary>
/// Dict cannot be serialized so be cannot edit them at inspector. We have to create a dict alike
/// structure.
///
/// Programatically this scriptable object behaves like a dict.
/// </summary>
[CreateAssetMenu(fileName = "ThingsSpritesOverGround", menuName = "ScriptableObjects/ThingsOverGroundSprites",
    order = 0)]
public class ThingsOverGroundSprites : ScriptableObject
{
    public List<NamedSprite> ThingsSprites;

    public OccludedSprite this[string key]
    {
        get => GetValue(key);

        set => SetValue(key, value);
    }

    private OccludedSprite GetValue(string key)
    {
        foreach (var namedSprite in ThingsSprites)
        {
            if (namedSprite.name == key) return namedSprite.occluded_sprite;
        }

        return null;
    }

    private void SetValue(string key, OccludedSprite value)
    {
        NamedSprite namedSpriteFound = null;
        foreach (var namedSprite in ThingsSprites)
        {
            if (namedSprite.name == key)
            {
                namedSpriteFound = namedSprite;
                break;
            };
        }

        if (namedSpriteFound == null)
        {
            ThingsSprites.Add(new NamedSprite(key, value));
        }
        else
        {
            namedSpriteFound.occluded_sprite = value;
        }
        
    }
}