using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Set up a wall brick to have desired behaviour.
/// </summary>
public class WallAppearance : MonoBehaviour, IBorder, IGround
{
    [SerializeField] private bool hasCutWall;
    [SerializeField] private bool hasGround;
    [SerializeField] private bool hasBorder;
    [SerializeField] private SpriteRenderer groundSpriteRenderer;
    [SerializeField] private SpriteRenderer ocluderSpriteRenderer;
    [SerializeField] private SpriteRenderer cutWallSpriteRenderer;
    [SerializeField] private SpriteRenderer thingsOverGroundSpriteRenderer;
    [SerializeField] private Sprite groundNoBorderSprite;
    [SerializeField] private Sprite groundBorderSprite;
    [SerializeField] private Sprite cutWallSprite;


    public bool IsBorderShown()
    {
        return hasBorder;
    }

    public void ShowBorder(bool showIt)
    {
        hasBorder = showIt;
    }
    
    private void UpdateAppearance()
    {
        SetGround();
        SetCutwall();
    }

    private void SetGround()
    {
        if (hasGround && hasBorder)
        {
            groundSpriteRenderer.sprite = groundBorderSprite;
        }
        else if (hasGround)
        {
            groundSpriteRenderer.sprite = groundNoBorderSprite;
        }
        groundSpriteRenderer.enabled = hasGround;
        ocluderSpriteRenderer.enabled = hasGround;
    }

    private void SetCutwall()
    {
        cutWallSpriteRenderer.enabled =  hasCutWall;
    }
    
    public void PlaceThingsOverGround(ThingsOverGround thing)
    {
        if (hasGround)
        {
            Sprite thingToPlace = thing switch
            {
                ThingsOverGround.Nothing => null,
                ThingsOverGround.Bones => 

            };
            thingsOverGroundSpriteRenderer.enabled = true;
        }
    }
}