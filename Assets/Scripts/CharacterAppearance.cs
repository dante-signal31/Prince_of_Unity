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
        [Tooltip("Needed to get if we are looking Rightwards and other conditions.")]
        [SerializeField] private CharacterStatus characterStatus;
        [Tooltip("Needed to modify sprites depending on conditions.")]
        [SerializeField] private Transform character;

        private bool _currentFacingIsRightWards = true;

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
            bool newFacing = characterStatus.LookingRightWards;
            if (_currentFacingIsRightWards != newFacing)
            {
                _currentFacingIsRightWards = newFacing;
                FlipCharacter(_currentFacingIsRightWards);
            }
        }
    }
}