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
    [Tooltip(("Collider used when character is jumping horizontally."))] 
    [SerializeField] private Collider2D horizontalJumpingCollider;

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
            else if (horizontalJumpingCollider.enabled)
            {
                return horizontalJumpingCollider;
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
        Falling,
        HorizontalJumping
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
        horizontalJumpingCollider.enabled = false;
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
            // case CharacterStatus.States.Idle:
            //     EnableUsualCollider();
            //     break;
            case CharacterStatus.States.RunningStart:
            case CharacterStatus.States.Running:
            case CharacterStatus.States.RunningEnd:
            case CharacterStatus.States.Unsheathe:
            case CharacterStatus.States.RunningJumpImpulse:
            // case CharacterStatus.States.RunningJump:
            // case CharacterStatus.States.WalkingJumpStart:
            // case CharacterStatus.States.WalkingJump:
            case CharacterStatus.States.WalkingJumpEnd:
            case CharacterStatus.States.AdvanceSword:
            case CharacterStatus.States.Retreat:
            case CharacterStatus.States.AttackWithSword:
            case CharacterStatus.States.BlockSword:
            case CharacterStatus.States.BlockedSword:
            case CharacterStatus.States.CounterBlockSword:
            case CharacterStatus.States.CounterAttackWithSword:
            case CharacterStatus.States.IdleSword:
                EnableFightingCollider();
                break;
            case CharacterStatus.States.FallStart:
            case CharacterStatus.States.Falling:
            case CharacterStatus.States.VerticalFall:
            case CharacterStatus.States.FallingSliding:
                EnableFallingCollider();
                break;
            case CharacterStatus.States.Dead:
                DisableColliders();
                break;
            case CharacterStatus.States.RunningJump:
            case CharacterStatus.States.WalkingJump:
                EnableHorizontalJumpCollider();
                break;
            default:
                EnableUsualCollider();
                break;
        }
    }

    /// <summary>
    /// Enable UsualCollider and disable every other collider.
    /// </summary>
    private void EnableUsualCollider()
    {
        if (_enabledCollider == ColliderTypes.Usual) return;
        usualCollider.enabled = true;
        fightingCollider.enabled = false;
        fallingCollider.enabled = false;
        horizontalJumpingCollider.enabled = false;
        _enabledCollider = ColliderTypes.Usual;
    }

    /// <summary>
    /// Enable FightingCollider and disable every other collider.
    /// </summary>
    private void EnableFightingCollider()
    {
        if (_enabledCollider == ColliderTypes.Fighting) return;
        usualCollider.enabled = false;
        fightingCollider.enabled = true;
        fallingCollider.enabled = false;
        horizontalJumpingCollider.enabled = false;
        _enabledCollider = ColliderTypes.Fighting;
    }

    /// <summary>
    /// Enable FallingCollider and disable every other collider.
    /// </summary>
    private void EnableFallingCollider()
    {
        if (_enabledCollider == ColliderTypes.Falling) return;
        usualCollider.enabled = false;
        fightingCollider.enabled = false;
        fallingCollider.enabled = true;
        horizontalJumpingCollider.enabled = false;
        _enabledCollider = ColliderTypes.Falling;
    }
    
    /// <summary>
    /// Enable horizontalJumpingCollider and disable every other collider.
    /// </summary>
    private void EnableHorizontalJumpCollider()
    {
        if (_enabledCollider == ColliderTypes.HorizontalJumping) return;
        usualCollider.enabled = false;
        fightingCollider.enabled = false;
        fallingCollider.enabled = false;
        horizontalJumpingCollider.enabled = true;
        _enabledCollider = ColliderTypes.HorizontalJumping;
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
        horizontalJumpingCollider.enabled = false;
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
            case ColliderTypes.HorizontalJumping:
                EnableHorizontalJumpCollider();
                break;
        }
    }
}
