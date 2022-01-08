using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Prince
{
    /// <summary>
    /// This component interacts with Physics component to make character move.   
    /// </summary>
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private CharacterStatus characterStatus;
        [SerializeField] private MovementProfile characterMovementProfile;
        [SerializeField] private Rigidbody2D rigidBody2D;

        private CharacterStatus.States _currentState;
        private Vector2 _currentForwardVector;
        private float _currentSpeed;

        private void Awake()
        {
            _currentState = characterStatus.CurrentState;
            _currentForwardVector = characterStatus.ForwardVector;
            
        }
        
        /// <summary>
        /// Get speed depending on state.
        /// </summary>
        /// <param name="state">State to get speed.</param>
        /// <returns>Speed for given state.</returns>
        private float GetCurrentSpeed(CharacterStatus.States state)
        {
            float speed = state switch
            {
                CharacterStatus.States.AdvanceSword => characterMovementProfile.AdvanceWithSwordSpeed,
                CharacterStatus.States.Retreat => characterMovementProfile.RetreatSpeed,
                CharacterStatus.States.HitBySword => characterMovementProfile.HitBySwordSpeed,
                _ => 0
            };
            return speed;
        }

        /// <summary>
        /// Update current speed only if state has changed.
        /// </summary>
        private void UpdateCurrentSpeed()
        {
            CharacterStatus.States characterState = characterStatus.CurrentState;
            if (_currentState != characterState)
            {
                _currentSpeed = GetCurrentSpeed(characterState);
                _currentState = characterState;
            }
        }

        /// <summary>
        /// Update position depending on speed and fixed delta time.
        /// </summary>
        private void UpdatePosition()
        {
            Vector2 currentPosition = gameObject.transform.position;
            Vector2 newPosition = currentPosition + _currentForwardVector * _currentSpeed * Time.fixedDeltaTime;
            rigidBody2D.MovePosition(newPosition);
        }

        private void FixedUpdate()
        {
            UpdateCurrentSpeed();
            if (Math.Abs(_currentSpeed) > 0.01f) UpdatePosition();
        }
    }
}