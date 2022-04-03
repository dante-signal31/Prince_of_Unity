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
    public class GravityController : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to alter gravity affection on character.")]
        [SerializeField] private Rigidbody2D rigidBody;
        [Tooltip("Needed to know current character state.")]
        [SerializeField] private CharacterStatus characterStatus;

        private float _enabledGravity;

        /// <summary>
        /// Whether gravity affects this character.
        /// </summary>
        public bool GravityEnabled => rigidBody.gravityScale > 0;

        private void Awake()
        {
            _enabledGravity = rigidBody.gravityScale;
        }

        private void FixedUpdate()
        {
            switch (characterStatus.CurrentState)
            {
                case CharacterStatus.States.Dead:
                case CharacterStatus.States.DeadByFall:
                    DisableGravity();
                    break;
                case CharacterStatus.States.RunningJump:
                case CharacterStatus.States.WalkingJump:
                    if (GravityEnabled) DisableGravity();
                    break;
                default:
                    if (!GravityEnabled) EnableGravity();
                    break;
            }
        }

        /// <summary>
        /// Deactivate gravity affection over this character.
        /// </summary>
        private void DisableGravity()
        {
            rigidBody.gravityScale = 0;
        }

        /// <summary>
        /// Activate gravity affection over this character.
        /// </summary>
        private void EnableGravity()
        {
            rigidBody.gravityScale = _enabledGravity;
        }
    }
}