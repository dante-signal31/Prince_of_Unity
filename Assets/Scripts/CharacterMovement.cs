using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.TextCore.Text;

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
        [Tooltip("Needed to know combat conditions that may change movements.")]
        [SerializeField] private FightingInteractions fightingInteractions;

        [Header("CONFIGURATION:")]
        [Tooltip("Needed to know this character speed in every state.")]
        [SerializeField] private MovementProfile characterMovementProfile;

        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;
        
        private CharacterStatus.States _currentState;
        private Vector2 _currentForwardVector;
        private float _currentSpeed;
        private float _oldSpeed;
        
        // Needed To get a reference to characterMovementProfile from SpeedForwarder script to 
        // animate speed values at variable speed stated.
        public MovementProfile CharacterMovementProfile => characterMovementProfile;

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
        private float GetCurrentSpeed()
        {
            float speed = characterStatus.CurrentState switch
            {
                CharacterStatus.States.AdvanceSword => characterMovementProfile.AdvanceWithSwordSpeed,
                CharacterStatus.States.Retreat => characterMovementProfile.RetreatSpeed,
                CharacterStatus.States.HitBySword => characterMovementProfile.HitBySwordSpeed,
                // Character only retreats when blocks while he is receiving an attack. Otherwise he 
                // performs a useless block but stays in his place.
                CharacterStatus.States.BlockSword when fightingInteractions.BlockingStrikePossible => characterMovementProfile.BlockSwordSpeed,
                // Transitional running states get running speeds from movement profiles after being pondered by animation
                // curves.
                CharacterStatus.States.RunningStart or
                    CharacterStatus.States.RunningEnd or 
                    CharacterStatus.States.TurnBackRunning => characterMovementProfile.CurrentRunningSpeed,
                CharacterStatus.States.Running => characterMovementProfile.MaximumRunningSpeed,
                CharacterStatus.States.Walk => characterMovementProfile.CurrentWalkingSpeed,
                CharacterStatus.States.StandFromCrouch => characterMovementProfile.CurrentStandingSpeed,
                CharacterStatus.States.CrouchWalking => characterMovementProfile.CurrentCrouchWalkingSpeed,
                CharacterStatus.States.FallStart or CharacterStatus.States.Falling when 
                    characterStatus.CurrentJumpingSequence == CharacterStatus.JumpingTypes.RunningJumping => CharacterMovementProfile.FallingHorizontalSpeedAfterRunningJump,
                CharacterStatus.States.FallStart or CharacterStatus.States.Falling when 
                    characterStatus.CurrentJumpingSequence == CharacterStatus.JumpingTypes.WalkingJumping => CharacterMovementProfile.FallingHorizontalSpeedAfterWalkingJump,
                CharacterStatus.States.FallStart or CharacterStatus.States.Falling when 
                    characterStatus.CurrentJumpingSequence == CharacterStatus.JumpingTypes.None => CharacterMovementProfile.FallingHorizontalSpeed,
                CharacterStatus.States.RunningJumpImpulse => CharacterMovementProfile.RunningJumpingImpulseSpeed,
                CharacterStatus.States.RunningJump => CharacterMovementProfile.RunningJumpingSpeed,
                CharacterStatus.States.WalkingJump => CharacterMovementProfile.WalkingJumpingSpeed,
                CharacterStatus.States.WalkingJumpEnd => CharacterMovementProfile.WalkingJumpingEndSpeed,
                    _ => 0
            };
            // return characterStatus.LookingRightWards? speed: speed * -1;
            return speed;
        }

        /// <summary>
        /// Update current speed only if state has changed or if we are in a non constant speed state.
        /// </summary>
        private void UpdateCurrentSpeed()
        {
            CharacterStatus.States characterState = characterStatus.CurrentState;
            if ((_currentState != characterState) || (IsVariableSpeedState()))
            {
                _currentSpeed = GetCurrentSpeed();
                _currentState = characterState;
                _currentForwardVector = GetCurrentForwardVector();
                this.Log($"(CharacterMovement - {gameObject.name}) We are in a variable speed state ({_currentState}) with speed {_currentSpeed}", showLogs);
            }
            this.Log($"(CharacterMovement - {gameObject.name}) Current speed {_currentSpeed}", showLogs);
        }

        /// <summary>
        /// Whether this state's movement speed is variable or not.
        /// </summary>
        /// <returns>
        /// True if current state's movement speed follows an animation curve instead. False if current state's
        /// movement speed has a constant speed.
        /// </returns>
        private bool IsVariableSpeedState()
        {
            switch (characterStatus.CurrentState)
            {
                case CharacterStatus.States.RunningStart:
                case CharacterStatus.States.RunningEnd:
                case CharacterStatus.States.TurnBackRunning: 
                case CharacterStatus.States.Walk:
                case CharacterStatus.States.StandFromCrouch:
                case CharacterStatus.States.CrouchWalking:
                    return true;
                default:
                    return false;
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
            Vector2 yComponent = characterStatus.CurrentState switch
            {
                // When jumping, gravity is disabled so any residual Y velocity makes character float away a bit.
                // So when jumping I make sure Y velocity is 0 to make jump movement keep its Y position.
                CharacterStatus.States.RunningJump or
                    CharacterStatus.States.WalkingJump => Vector2.zero,
                // In any other case we only change speed on horizontal axis (X). Vertical axis (Y) is changed only by gravity forces
                // so we must keep that axis to restore it when changing horizontal velocity.
                _ => new Vector2(0, rigidBody2D.velocity.y)
            };
            rigidBody2D.velocity = _currentForwardVector * _currentSpeed + yComponent;
            this.Log($"(CharacterMovement - {gameObject.name}) Moving with speed {_currentSpeed} and forward vector {_currentForwardVector}", showLogs);
        }

        private void FixedUpdate()
        {
            UpdateCurrentSpeed();
            UpdatePosition();
        }
    }
}