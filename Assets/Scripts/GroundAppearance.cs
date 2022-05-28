// Customize ground bricks appearance.

using UnityEngine;

[ExecuteAlways]
public class GroundAppearance : MonoBehaviour, IBorder
{
    [Header("WIRING:")]
    [Tooltip("Needed to show garbage over the ground.")]
    [SerializeField] protected GroundRubbishAppearance rubbishAppearance; 
    [Tooltip("Needed to operate over main sprite.")]
    [SerializeField] protected SpriteRenderer groundSpriteRenderer;
    [Tooltip("Needed to operate over occluder sprite.")]
    [SerializeField] protected SpriteRenderer occluderSpriteRenderer;

    [Header("CONFIGURATION:")]
    [Tooltip("Data about garbage placeable over ground.")]
    [SerializeField] private ThingsOverGround thingsOverGround;
    [Tooltip("Sprite to use as ground with no border.")]
    [SerializeField] private Sprite groundNoBorderSprite;
    [Tooltip("Sprite to use as ground with border.")]
    [SerializeField] private Sprite groundBorderSprite;
    [Tooltip("Whether brick show border or not. ")]
    [SerializeField] private bool hasBorder=true;
    
    // private GroundRubbishAppearance rubbishAppearance;
    private bool _appearanceUpdateNeeded = false;
    protected bool _hidden = false;

    // private void Awake()
    // {
    //     rubbishAppearance = gameObject.GetComponentInChildren<GroundRubbishAppearance>();
    // }

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
        rubbishAppearance.PlaceThingsOverGround(thingsOverGround);
    }
    
    /// <summary>
    /// Show ground border if this brick has one.
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

    // /// <summary>
    // /// Make this ground invisible.
    // ///
    // /// Used in falling ground to hide ground when it crashes against below brick.
    // /// </summary>
    // public void HideGround()
    // {
    //     if (!_hidden)
    //     {
    //         groundSpriteRenderer.enabled = false;
    //         occluderSpriteRenderer.enabled = false;
    //         rubbishAppearance.ShowRubbish = false;
    //         _hidden = true;
    //     }
    // }
    //
    // /// <summary>
    // /// Make this ground visible.
    // /// </summary>
    // public void ShowGround()
    // {
    //     if (_hidden)
    //     {
    //         groundSpriteRenderer.enabled = true;
    //         occluderSpriteRenderer.enabled = true;
    //         rubbishAppearance.ShowRubbish = true;
    //         _hidden = false;
    //     }
    // }

    public bool IsBorderShown()
    {
        return hasBorder;
    }

    public void ShowBorder(bool showIt)
    {
        hasBorder = showIt;
    }
}
