// Customize wall bricks appearance.

using UnityEngine;
using Random = System.Random;

/// <summary>
/// Set up a wall brick to have desired behaviour.
/// </summary>
[ExecuteAlways]
public class WallAppearance : MonoBehaviour, IBorder
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
    [SerializeField] private Sprite groundNoBorderSprite;
    [SerializeField] private Sprite groundBorderSprite;

    [SerializeField] private Sprite[] frontWallOptions;
    
    private GroundRubbishAppearance _rubbishAppearance;
    private bool _appearanceUpdatedNeeded = false;

    private void Awake()
    {
        _rubbishAppearance = gameObject.GetComponentInChildren<GroundRubbishAppearance>();
    }

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
        if (_hasGround) _rubbishAppearance.PlaceThingsOverGround(_thingsOverGround);
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
        SetEnabledSpriteRenderer(groundSpriteRenderer, _hasGround);
        _rubbishAppearance.ShowRubbish = _hasGround;
        groundOccluderSpriteRenderer.enabled = _hasGround;
    }

    /// <summary>
    /// Show a cutwall if this brick has one.
    /// </summary>
    private void SetCutwall()
    {
        SetEnabledSpriteRenderer(cutWallSpriteRenderer, _hasCutWall);
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

    /// <summary>
    /// Use this method to enable or disable sprite renderers with collider associated.
    /// </summary>
    /// <param name="spriteRenderer"></param>
    private void SetEnabledSpriteRenderer(SpriteRenderer spriteRenderer, bool enabled)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = enabled;
            EdgeCollider2D collider = spriteRenderer.transform.GetComponent<EdgeCollider2D>();
            if (collider != null) collider.enabled = enabled;
        }
    }

    private void Start()
    {
        UpdateAppearance(randomizeFrontWall);
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
            UpdateAppearance(randomizeFrontWall);
            _appearanceUpdatedNeeded = false;
        }
        #endif
    }
}