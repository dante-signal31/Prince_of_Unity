// Customize ground bricks appearance.

using UnityEngine;

[ExecuteAlways]
public class GroundAppearance : MonoBehaviour, IBorder
{
    [SerializeField] private bool hasBorder=true;
    [SerializeField] private ThingsOverGround thingsOverGround;
    
    [SerializeField] private SpriteRenderer groundSpriteRenderer;
    [SerializeField] private Sprite groundNoBorderSprite;
    [SerializeField] private Sprite groundBorderSprite;

    private GroundRubbishAppearance _rubbishAppearance;
    private bool _appearanceUpdateNeeded = false;

    private void Awake()
    {
        _rubbishAppearance = gameObject.GetComponentInChildren<GroundRubbishAppearance>();
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
        _rubbishAppearance.PlaceThingsOverGround(thingsOverGround);
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

    public void HideGround()
    {
        // TODO: implement.
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
