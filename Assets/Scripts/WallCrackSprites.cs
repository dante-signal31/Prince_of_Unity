// // Scriptable object to pack every possible wall crack name, and its respective sprite.

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WallCrackNamedSprite
{
    public string name;
    public Sprite crackSprite;

    public WallCrackNamedSprite(string name, Sprite crackSprite)
    {
        this.name = name;
        this.crackSprite = crackSprite;
    }
}


[CreateAssetMenu(fileName = "WallCrackSprites", menuName = "ScriptableObjects/WallCrackSprites", order = 1)]
public class WallCrackSprites : ScriptableObject
{
    public List<WallCrackNamedSprite> crackSprites;

    public Sprite this[string key]
    {
        get => GetValue(key);

        set => SetValue(key, value);
    }

    private Sprite GetValue(string key)
    {
        foreach (var namedSprite in crackSprites)
        {
            if (namedSprite.name == key) return namedSprite.crackSprite;
        }

        return null;
    }

    private void SetValue(string key, Sprite value)
    {
        WallCrackNamedSprite namedSpriteFound = null;
        foreach (var namedSprite in crackSprites)
        {
            if (namedSprite.name == key)
            {
                namedSpriteFound = namedSprite;
                break;
            };
        }

        if (namedSpriteFound == null)
        {
            crackSprites.Add(new WallCrackNamedSprite(key, value));
        }
        else
        {
            namedSpriteFound.crackSprite = value;
        }
    } 
}