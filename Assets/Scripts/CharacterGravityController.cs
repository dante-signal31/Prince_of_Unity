using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component controls how gravity affects character.
    ///
    /// It is used mainly to deactivate gravity for this character in special moments
    /// like jumps or death.
    /// </summary>
    public class CharacterGravityController: GravityController
    {
        [Header("WIRING:")]
        [Tooltip("Needed to know current character state.")]
        [SerializeField] private CharacterStatus stateMachineStatus;
        
        private void FixedUpdate()
        {

            switch (stateMachineStatus.CurrentState)
            {
                case CharacterStatus.States.Dead:
                case CharacterStatus.States.DeadByFall:
                    if (GravityEnabled) DisableGravityAndMovement();
                    break;
                case CharacterStatus.States.RunningJump:
                case CharacterStatus.States.WalkingJump:
                case CharacterStatus.States.Climbing:
                    if (GravityEnabled) DisableGravity();
                    break;
                default:
                    if (!GravityEnabled) EnableGravity();
                    break;
            }
        }
        
        /// <summary>
        /// Disables gravity for this character.
        /// </summary>
        protected void DisableGravity()
        {
            base.DisableGravity();
            // Rigid body constraints must be set. Otherwise when this component is used with
            // character it can float away while gravity is disabled..
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        }
        
        /// <summary>
        /// Disables gravity and movement for this character.
        /// </summary>
        protected void DisableGravityAndMovement()
        {
            base.DisableGravity();
            // Rigid body constraints must be set. Otherwise when this component is used with
            // character it can float away while gravity is disabled.
            rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}