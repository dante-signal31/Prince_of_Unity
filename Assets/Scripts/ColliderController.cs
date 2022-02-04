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
    private Collider2D CurrentCollider=> (usualCollider.enabled)? usualCollider: fightingCollider;
    
    /// <summary>
    /// Total width of this character active collider.
    ///
    /// I don't use semi-width because this property is mainly used while fighting and then current
    /// colliders are mostly forward offset from gameobject center position, so entire width is more
    /// realistic.
    /// </summary>
    public float CurrentColliderWidth => CurrentCollider.bounds.size.x;

    private enum ColliderTypes
    {
        Usual,
        Fighting,
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
                EnableUsualCollider();
                break;
            case CharacterStatus.States.Unsheathe:
                EnableFightingCollider();
                break;
            case CharacterStatus.States.FallStart:
            case CharacterStatus.States.Dead: 
                DisableColliders();
                break;
            case CharacterStatus.States.Landing:
            case CharacterStatus.States.Crouch:
            case CharacterStatus.States.CrouchFromStand:
                EnableColliders();
                break;
        }
    }

    /// <summary>
    /// Enable UsualCollider and disable FightingCollider.
    /// </summary>
    private void EnableUsualCollider()
    {
        usualCollider.enabled = true;
        fightingCollider.enabled = false;
        _enabledCollider = ColliderTypes.Usual;
    }

    /// <summary>
    /// Enable FightingCollider and disable UsualCollider.
    /// </summary>
    private void EnableFightingCollider()
    {
        usualCollider.enabled = false;
        fightingCollider.enabled = true;
        _enabledCollider = ColliderTypes.Fighting;
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
        }
    }
    
    
}
