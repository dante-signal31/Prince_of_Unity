using System;
using System.Collections;
using System.Collections.Generic;
using Prince;
using UnityEngine;

/// <summary>
/// Component to switch character collider depending of its states.
/// </summary>
public class ColliderController : MonoBehaviour
{
    [Header("WIRING:")]
    [Tooltip("Needed to follow character states progress.")]
    [SerializeField] private CharacterStatus characterStatus;
    [Tooltip("Collider used when standing without a sword unsheathed.")]
    [SerializeField] private Collider2D usualCollider;
    [Tooltip("Collider used when character moves with sword unsheathed.")]
    [SerializeField] private Collider2D fightingCollider;

    private CharacterStatus.States _currentState;

    private void Awake()
    {
        ResetColliders();
    }

    private void FixedUpdate()
    {
        if (characterStatus.CurrentState != _currentState)
        {
            _currentState = characterStatus.CurrentState;
           UpdateColliders(_currentState);
        }
    }

    /// <summary>
    /// Set colliders at its default state.
    /// </summary>
    private void ResetColliders()
    {
        usualCollider.enabled = true;
        fightingCollider.enabled = false;
    }

    /// <summary>
    /// Enable correct collider depending on current state.
    ///
    /// Every other collider is disabled.
    /// </summary>
    /// <param name="currentState">State we have just switched to.</param>
    private void UpdateColliders(CharacterStatus.States currentState)
    {
        switch (currentState)
        {
            case CharacterStatus.States.Idle:
                usualCollider.enabled = true;
                fightingCollider.enabled = false;
                break;
            case CharacterStatus.States.Unsheathe:
                usualCollider.enabled = false;
                fightingCollider.enabled = true;
                break;
        }
    }
}
