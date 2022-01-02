using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Update Character appearance depending of status and environment conditions.
    /// </summary>
    [ExecuteAlways]
    public class CharacterAppearance : MonoBehaviour
    {
        [Tooltip("Needed to get if we are looking Rightwards and other conditions.")]
        [SerializeField] private CharacterStatus characterStatus;
        [Tooltip("Needed to modify sprites depending on conditions.")]
        [SerializeField] private Transform character;

        private bool _currentFacingIsRightWards = true;

        private void FlipCharacter()
        {
            var currentScale = character.localScale;
            currentScale = new Vector3(currentScale.x * -1,
                                        currentScale.y,
                                        currentScale.z);
            character.localScale = currentScale;
        }
        
        private void Awake()
        {
            _currentFacingIsRightWards = characterStatus.LookingRightWards;
            if (!_currentFacingIsRightWards) FlipCharacter();
        }

        private void Update()
        {
            bool newFacing = characterStatus.LookingRightWards;
            if (_currentFacingIsRightWards != newFacing)
            {
                _currentFacingIsRightWards = newFacing;
                FlipCharacter();
            }
        }
    }
}