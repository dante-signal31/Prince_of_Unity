﻿using System;
using UnityEngine;

namespace Prince
{
    public class GravityController : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to alter gravity affection on character.")]
        [SerializeField] private Rigidbody2D rigidBody;
        [Tooltip("Needed to know current character state.")]
        [SerializeField] private CharacterStatus characterStatus;

        private float _enabledGravity;

        private void Awake()
        {
            _enabledGravity = rigidBody.gravityScale;
        }

        private void FixedUpdate()
        {
            switch (characterStatus.CurrentState)
            {
                case CharacterStatus.States.Dead:
                    DisableGravity();
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