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
        [Tooltip("Needed to signal if gravity is enabled.")]
        [SerializeField] private Animator stateMachine;

        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;
        
        private float _enabledGravity;
        private bool _isGravityEnabled;

        /// <summary>
        /// Whether gravity affects this character.
        /// </summary>
        public bool GravityEnabled => rigidBody.gravityScale > 0;

        private void Awake()
        {
            _enabledGravity = rigidBody.gravityScale;
        }

        private void Start()
        {
            _isGravityEnabled = stateMachine.GetBool("GravityEnabled");
        }

        private void FixedUpdate()
        {

            switch (characterStatus.CurrentState)
            {
                // TODO: Try to unify this first bunch of conditions with second one.
                case CharacterStatus.States.Dead:
                case CharacterStatus.States.DeadByFall:
                    DisableGravity();
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
        /// Deactivate gravity affection over this character.
        /// </summary>
        private void DisableGravity()
        {
            rigidBody.gravityScale = 0;
            rigidBody.velocity = Vector2.zero;
            stateMachine.SetBool("GravityEnabled", false);
            _isGravityEnabled = false;
            this.Log($"(Gravity controller - {transform.root.name}) Gravity disabled for this game object.", showLogs);
        }

        /// <summary>
        /// Activate gravity affection over this character.
        /// </summary>
        private void EnableGravity()
        {
            rigidBody.gravityScale = _enabledGravity;
            stateMachine.SetBool("GravityEnabled", true);
            _isGravityEnabled = true;
            this.Log($"(Gravity controller - {transform.root.name}) Gravity enabled for this game object.", showLogs);
        }
    }
}