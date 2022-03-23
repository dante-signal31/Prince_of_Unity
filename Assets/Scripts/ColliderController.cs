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
    [Tooltip("Collider used when character falls.")]
    [SerializeField] private Collider2D fallingCollider;

    private CharacterStatus.States _currentState;
    
    /// <summary>
    /// Collider currently enabled.
    /// </summary>
    private Collider2D CurrentCollider {
        get
        {
            if (usualCollider.enabled)
            {
                return usualCollider;
            }
            else if (fightingCollider.enabled)
            {
                return fightingCollider;
            }
            else
            {
                return fallingCollider;
            }
        }
    }
    
    /// <summary>
    /// <p>Total width of this character active collider.</p>
    ///
    /// <p>I don't use semi-width because this property is mainly used while fighting and then current
    /// colliders are mostly forward offset from gameobject center position, so entire width is more
    /// realistic.</p>
    /// </summary>
    public float CurrentColliderWidth => CurrentCollider.bounds.size.x;

    private enum ColliderTypes
    {
        Usual,
        Fighting,
        Falling
    }

    private ColliderTypes _enabledCollider;

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
        fallingCollider.enabled = false;
    }

    /// <summary>
    /// <p>Enable correct collider depending on current state.</p>
    ///
    /// <p>Every other collider is disabled.</p>
    /// </summary>
    /// <param name="currentState">State we have just switched to.</param>
    private void UpdateColliders(CharacterStatus.States currentState)
    {
        switch (currentState)
        {
            case CharacterStatus.States.Idle:
                EnableUsualCollider();
                break;
            case CharacterStatus.States.Unsheathe:
            case CharacterStatus.States.RunningStart:
                EnableFightingCollider();
                break;
            case CharacterStatus.States.FallStart:
            // case CharacterStatus.States.Falling:
                EnableFallingCollider();
                break;
            case CharacterStatus.States.Dead: 
                DisableColliders();
                break;
            case CharacterStatus.States.Crouch:
            case CharacterStatus.States.CrouchFromStand:
                EnableColliders();
                break;
        }
    }

    /// <summary>
    /// Enable UsualCollider and disable every other collider.
    /// </summary>
    private void EnableUsualCollider()
    {
        usualCollider.enabled = true;
        fightingCollider.enabled = false;
        fallingCollider.enabled = false;
        _enabledCollider = ColliderTypes.Usual;
    }

    /// <summary>
    /// Enable FightingCollider and disable every other collider.
    /// </summary>
    private void EnableFightingCollider()
    {
        usualCollider.enabled = false;
        fightingCollider.enabled = true;
        fallingCollider.enabled = false;
        _enabledCollider = ColliderTypes.Fighting;
    }

    /// <summary>
    /// Enable FallingCollider and disable every other collider.
    /// </summary>
    private void EnableFallingCollider()
    {
        usualCollider.enabled = false;
        fightingCollider.enabled = false;
        fallingCollider.enabled = true;
        // _enabledCollider = ColliderTypes.Falling;
    }

    /// <summary>
    /// Disable both colliders.
    ///
    /// Useful for falling cleanly through holes.
    /// </summary>
    private void DisableColliders()
    {
        usualCollider.enabled = false;
        fightingCollider.enabled = false;
        fallingCollider.enabled = false;
    }

    /// <summary>
    /// Enable colliders to its previous configuration.
    ///
    /// Useful to land cleanly when ground is detected again.
    /// </summary>
    private void EnableColliders()
    {
        switch (_enabledCollider)
        {
            case ColliderTypes.Usual:
                EnableUsualCollider();
                break;
            case ColliderTypes.Fighting:
                EnableFightingCollider();
                break;
            case ColliderTypes.Falling:
                EnableFallingCollider();
                break;
        }
    }
    
    
}
