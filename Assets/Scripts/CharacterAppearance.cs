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

        private bool _currentFacingIsRightWards;

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
                _currentFacingIsRightWards = newFacing;
                FlipCharacter(_currentFacingIsRightWards);
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