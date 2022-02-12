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
        [Header("WIRING:")]
        [Tooltip("Needed to know where is this character looking to.")]
        [SerializeField] private CharacterStatus characterStatus;
        [Tooltip("Needed to change this character position depending on its speed.")]
        [SerializeField] private Rigidbody2D rigidBody2D;

        [Header("CONFIGURATION:")]
        [Tooltip("Needed to know this character speed in every state.")]
        [SerializeField] private MovementProfile characterMovementProfile;
        
        private CharacterStatus.States _currentState;
        private Vector2 _currentForwardVector;
        private float _currentSpeed;

        private void Awake()
        {
            _currentState = characterStatus.CurrentState;
            _currentForwardVector = GetCurrentForwardVector();
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
                CharacterStatus.States.BlockSword => characterMovementProfile.BlockSwordSpeed,
                _ => 0
            };
            // return characterStatus.LookingRightWards? speed: speed * -1;
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
                _currentForwardVector = GetCurrentForwardVector();
            }
        }

        /// <summary>
        /// Get forward vector currently active for this character.
        /// </summary>
        /// <returns>Forward vector currently active.</returns>
        private Vector2 GetCurrentForwardVector()
        {
            return characterStatus.ForwardVector;
        }

        /// <summary>
        /// Update position depending on speed and fixed delta time.
        /// </summary>
        private void UpdatePosition()
        {
            // Vector2 currentPosition = gameObject.transform.position;
            // Vector2 newPosition = currentPosition + _currentForwardVector * _currentSpeed * Time.fixedDeltaTime;
            // rigidBody2D.MovePosition(newPosition);
            rigidBody2D.velocity = _currentForwardVector * _currentSpeed;
            Debug.Log($"(CharacterMovement - {gameObject.name}) Moving with speed {_currentSpeed} and forward vector {_currentForwardVector}");
        }

        private void FixedUpdate()
        {
            UpdateCurrentSpeed();
            if (Math.Abs(_currentSpeed) > 0.01f) UpdatePosition();
        }
    }
}