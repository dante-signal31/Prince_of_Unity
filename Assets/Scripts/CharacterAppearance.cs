using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Prince
{
    /// <summary>
    /// Update Character appearance depending of status and environment conditions.
    /// </summary>
    [ExecuteAlways]
    public class CharacterAppearance : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to get if we are looking Rightwards and other conditions.")]
        [SerializeField] private CharacterStatus characterStatus;
        [Tooltip("Needed to modify sprites depending on conditions.")]
        [SerializeField] private Transform directionalComponents;
        [Tooltip("Needed to change sprite layer in certain conditions.")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Header("CONFIGURATION:")]
        [Tooltip("Order in layer when this sprite is translated to foreground layer.")]
        [SerializeField] private int foregroundOrderInLayer;

        private bool _currentFacingIsRightWards;

        private void FlipCharacter(bool rightWards)
        {
            // First versions flipped the entire Character transform. Problem was that at turning while running
            // a kind of ghosting appeared. First frame of running animation appeared looking apposite than
            // character current animation. It was just a frame but it was very apparent. I think it might be
            // caused by some sprite renderer buffer issue (it has that frame already buffered and rendered it
            // although the entire gameobject was looking the other way round).
            //
            // A partial workaround I've found is using SpriteRendered built-in flipX() method and flip by
            // transform only those gameobject components that really need it. So those component have been
            // moved to DirectionalComponents transform.
            //
            // This work around is partially effective. Frame ghosting still happens but it is less likely.
            // Hopefully this will be fixed in any further Unity version.
            FlipSpriteRenderer(rightWards);
            FlipDirectionalComponents(rightWards);
        }

        private void FlipDirectionalComponents(bool rightWards)
        {
            Vector3 currentScale = directionalComponents.localScale;
            float x = rightWards ? Math.Abs(currentScale.x) : Math.Abs(currentScale.x) * -1;
            currentScale = new Vector3(x,
                currentScale.y,
                currentScale.z);
            directionalComponents.localScale = currentScale;
        }

        private void FlipSpriteRenderer(bool rightWards)
        {
            // Sprite rendered disabling actually is not needed but I think it makes frame ghosting
            // less likely.
            spriteRenderer.enabled = false;
            spriteRenderer.flipX = !rightWards;
            spriteRenderer.enabled = true;
        }

        private void Awake()
        {
            _currentFacingIsRightWards = characterStatus.LookingRightWards;
            FlipCharacter(_currentFacingIsRightWards);
        }

        private void Update()
        {
            UpdateCharacterFacing();
            if (characterStatus.CurrentState == CharacterStatus.States.Dead)
            {
                SpriteToForeground();
            }
        }

        /// <summary>
        /// Flip character if its facing has changed.
        /// </summary>
        private void UpdateCharacterFacing()
        {
            bool newFacing = characterStatus.LookingRightWards;
            if (_currentFacingIsRightWards != newFacing)
            {
                FlipCharacter(newFacing);
                _currentFacingIsRightWards = newFacing;
            }
        }
        
        /// <summary>
        /// <p>Take sprite renderer to foreground layer.</p>
        ///
        /// <p>This method is used when character dies and its corpse should not be under ground occluders.</p>
        /// </summary>
        private void SpriteToForeground()
        {
            spriteRenderer.sortingLayerName = "Foreground";
            spriteRenderer.sortingOrder = foregroundOrderInLayer;
        }

        // private void SpriteToItsOriginalLayer()
        // {
        //     
        // }
    }
}