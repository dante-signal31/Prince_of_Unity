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
        [SerializeField] private Transform character;
        [Tooltip("Needed to change sprite layer in certain conditions.")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Header("CONFIGURATION:")]
        [Tooltip("Order in layer when this sprite is translated to foreground layer.")]
        [SerializeField] private int foregroundOrderInLayer;
        [Tooltip("Corpse to show in stabbing traps.")] 
        [SerializeField] private Sprite stabbedCorpse;
        [Tooltip("Corpse to show in cutting traps.")] 
        [SerializeField] private Sprite cutCorpse;

        private bool _currentFacingIsRightWards;

        private bool SpriteVisible => spriteRenderer.enabled;

        private void FlipCharacter(bool rightWards)
        {
            Vector3 currentScale = character.localScale;
            float x = rightWards ? Math.Abs(currentScale.x) : Math.Abs(currentScale.x) * -1;
            currentScale = new Vector3( x,
                currentScale.y,
                currentScale.z);
            character.localScale = currentScale;
        }

        private void Awake()
        {
            _currentFacingIsRightWards = characterStatus.LookingRightWards;
            FlipCharacter(_currentFacingIsRightWards);
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
                case CharacterStatus.States.KilledByTrap:
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

        /// <summary>
        /// Get corpse to show for this character at traps.
        /// </summary>
        /// <param name="corpseType">Killing mode of trap.</param>
        /// <returns>Corpse to show at a trap of this mode.</returns>
        public Sprite GetCorpseSprite(TrapInteractions.CorpseTypes corpseType)
        {
            switch (corpseType)
            {
                case TrapInteractions.CorpseTypes.Stabbed:
                    return stabbedCorpse;
                case TrapInteractions.CorpseTypes.Cut:
                    return cutCorpse;
                default:
                    // We won't get here.
                    return null;
            }
        }
    }
}