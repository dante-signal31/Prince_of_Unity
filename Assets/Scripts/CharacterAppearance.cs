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
        private float _spriteRendererOffset;
        
        private bool SpriteVisible => spriteRenderer.enabled;

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
            CorrectSpriteRendererOffset(rightWards);
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

        /// <summary>
        /// Sprite flips using SpriteRenderer transform as it axis instead of current gameobject
        /// transform. So when flipped an offset appears that must be corrected.
        /// </summary>
        /// <param name="rightWards"></param>
        private void CorrectSpriteRendererOffset(bool rightWards)
        {
            if (rightWards) transform.position = new Vector3(transform.root.position.x + _spriteRendererOffset,
                transform.position.y,
                transform.position.z);
            else transform.position = new Vector3(transform.root.position.x - _spriteRendererOffset,
                transform.position.y,
                transform.position.z);
        }

        private void Awake()
        {
            _currentFacingIsRightWards = characterStatus.LookingRightWards;
            GetInitialSpriteRendererOffset(_currentFacingIsRightWards);
            FlipCharacter(_currentFacingIsRightWards);
        }

        private void GetInitialSpriteRendererOffset(bool rightWards)
        {
            _spriteRendererOffset = rightWards ? transform.position.x - transform.root.position.x : transform.root.position.x - transform.position.x;
        }

        private void Update()
        {
            UpdateCharacterFacing();
            switch (characterStatus.CurrentState)
            {
                case CharacterStatus.States.Dead:
                case CharacterStatus.States.DeadByFall:
                    SpriteToForeground();
                    break;
                case CharacterStatus.States.Climbing:
                case CharacterStatus.States.Descending:
                    MakeSpriteInvisible();
                    break;
                default:
                    MakeSpriteVisible();
                    break;
            }
            // if (characterStatus.CurrentState == CharacterStatus.States.Dead ||
            //     characterStatus.CurrentState == CharacterStatus.States.DeadByFall)
            // {
            //     SpriteToForeground();
            // }
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
        
        /// <summary>
        /// <p>Make character sprite invisible</p>.
        ///
        /// <p>This is useful when other props animations of character (like climbing a brick or drinking a portion) are playing.</p>
        /// </summary>
        private void MakeSpriteInvisible()
        {
            if (SpriteVisible) spriteRenderer.enabled = false;
        }
        
        /// <summary>
        /// <p>Make character sprite visible</p>.
        ///
        /// <p>This is useful when other props animations of character (like climbing a brick or drinking a portion) have finished.</p>
        /// </summary>
        private void MakeSpriteVisible()
        {
            if (!SpriteVisible) spriteRenderer.enabled = true;
        }
    }
}