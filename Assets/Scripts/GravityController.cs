﻿using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component controls how gravity affects game object.
    ///
    /// It is used mainly to deactivate gravity for this character in special moments.
    ///
    /// As it is designed to be used across multiple game objects it is generic. So, it should not
    /// be directly attached to a game object. Instead you must subclass it, specifying its generic type and
    /// that subclass can then be directly attached to a game object.
    /// </summary>
    public abstract class GravityController: MonoBehaviour
    {
        [Header("BASE-WIRING:")]
        [Tooltip("Needed to alter gravity affection on character.")]
        [SerializeField] protected Rigidbody2D rigidBody;

        //[Tooltip("Needed to know current character state.")]
        //[SerializeField] private CharacterStatus characterStatus;
        // [Tooltip("Needed to know current game object state.")]
        // [SerializeReference] protected IStateMachineStatus<T> stateMachineStatus;

        [Tooltip("Needed to signal if gravity is enabled.")]
        [SerializeField] private Animator stateMachine;

        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;
        
        private float _enabledGravity;
        private RigidbodyConstraints2D _currentRigidbodyConstraints;
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

        // private abstract void FixedUpdate();
        
        /// <summary>
        /// Deactivate gravity affection over this character.
        /// </summary>
        protected virtual void DisableGravity()
        {
            rigidBody.gravityScale = 0;
            _currentRigidbodyConstraints = rigidBody.constraints;
            rigidBody.velocity = Vector2.zero;
            stateMachine.SetBool("GravityEnabled", false);
            _isGravityEnabled = false;
            this.Log($"(Gravity controller - {transform.root.name}) Gravity disabled for this game object.", showLogs);
        }

        /// <summary>
        /// Activate gravity affection over this character.
        /// </summary>
        protected void EnableGravity()
        {
            rigidBody.gravityScale = _enabledGravity;
            rigidBody.constraints = _currentRigidbodyConstraints;
            stateMachine.SetBool("GravityEnabled", true);
            _isGravityEnabled = true;
            this.Log($"(Gravity controller - {transform.root.name}) Gravity enabled for this game object.", showLogs);
        }
    }
}