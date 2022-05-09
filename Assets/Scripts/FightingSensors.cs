using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Prince;
using UnityEngine;

/// <summary>
/// While EnemySensors search for enemy in the long range, this component detects when an enemy enter in hitting range.
/// </summary>
public class FightingSensors : MonoBehaviour
{
    [Header("WIRING:")]
    [Tooltip("Needed to update when an enemy is at hitting range and we don't need to advance further.")]
    [SerializeField] private Animator stateMachine;
    [Tooltip("Needed to find out HittingRange.")]
    [SerializeField] private BoxCollider2D hittingZone;
    [Header("CONFIGURATION:")]
    [Tooltip("Is this component attached to a guard?")]
    [SerializeField] private bool imGuard;
    [Tooltip("How much space proportion we let enemy enter into our hitting zone.")]
    [Range(0,1)]
    [SerializeField] private float hittingZoneTolerance;
    
    /// <summary>
    /// How much space proportion we let enemy enter into our hitting zone.
    /// </summary>
    public float HittingZoneTolerance => hittingZoneTolerance;

    /// <summary>
    /// Distance from character where hitting range starts.
    /// </summary>
    public float HittingRange => hittingZone.size.x;
    
    /// <summary>
    /// Enemy currently in hitting range.
    /// </summary>
    public GameObject CurrentEnemy { get; private set;}

    /// <summary>
    /// Have we got an enemy inside hitting range?
    /// </summary>
    public bool EnemyAtHittingRange => (CurrentEnemy != null);

    /// <summary>
    /// Minimum distance this character can be from its enemy.
    /// </summary>
    public float MaximumApproachDistance
    {
        get
        {
            if (CurrentEnemy == null)
            {
                return HittingRange * (1 - HittingZoneTolerance);
            }
            else
            {
                return HittingRange * (1 - HittingZoneTolerance) +
                       CurrentEnemy.GetComponentInChildren<ColliderController>()
                           .CurrentColliderWidth;
            }
        }
    }

    /// <summary>
    /// Get distance to given enemy from this character.
    /// </summary>
    /// <param name="enemy">Enemy to which we want to measure distance.</param>
    /// <returns>Distance to enemy.</returns>
    private float DistanceToEnemy(GameObject enemy)
    {
        return Vector2.Distance(transform.position, enemy.transform.position);
    }

    public bool MaximumApproachToEnemyReached { get; private set; }
    
    /// <summary>
    /// Friend or foe identification.
    /// </summary>
    /// <param name="other">Other guy.</param>
    /// <returns>True if he is our enemy, false if not.</returns>
    private bool IsOurEnemy(Collider2D other)
    {
        // We want to detect character solid colliders, not sensors.
        if (other.transform.CompareTag("Sensor")) return false;
        
        string otherTag = GetRootGameObject(other).tag;
        if (imGuard && otherTag == "Player") return true;
        if (imGuard && otherTag == "Guard") return false;
        if (!imGuard && otherTag == "Guard") return true;
        if (!imGuard && otherTag == "Player") return false;
        
        // We aren't going to get here.
        return false; 
    }

    /// <summary>
    /// Get main GameObject this collider is attached to.
    /// </summary>
    /// <param name="col">Collider detected</param>
    /// <returns>Root GameObject this collider is attached to.</returns>
    private GameObject GetRootGameObject(Collider2D col)
    {
        // Non trigger collider are attached at a subtransform of a component of Physics subtransform so be must go up.
        return col.transform.root.gameObject;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (IsOurEnemy(col))
        {
            CurrentEnemy = GetRootGameObject(col);
            stateMachine.SetBool("enemyInHittingRange", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsOurEnemy(other))
        {
            GameObject otherEnemy = GetRootGameObject(other);
            if (CurrentEnemy.name == otherEnemy.name)
            {
                CurrentEnemy = null;
                stateMachine.SetBool("enemyInHittingRange", false);
            } 
        }
    }

    private void Awake()
    {
        MaximumApproachToEnemyReached = false;
    }

    private void FixedUpdate()
    {
        CheckIfMaximumApproachReached();
    }

    /// <summary>
    /// Check if we have reached the minimum distance to current enemy.
    /// </summary>
    private void CheckIfMaximumApproachReached()
    {
        // Enemy in hitting range and under MaximumApproachDistance.
        if (CurrentEnemy != null)
        {
            if (DistanceToEnemy(CurrentEnemy) <= MaximumApproachDistance)
            {
                if (!MaximumApproachToEnemyReached)
                {
                    stateMachine.SetBool("MaximumApproachToEnemyReached", true);
                    MaximumApproachToEnemyReached = true;
                }
                return;
            }
        }
        // Enemy out of hitting range or beyond MaximumApproachDistance.
        if (MaximumApproachToEnemyReached)
        {
            stateMachine.SetBool("MaximumApproachToEnemyReached", false);
            MaximumApproachToEnemyReached = false;
        }
        
    }
    

    /// <summary>
    /// Visual in-editor aid to show how much distance we can enter an enemy into our hitting range.
    /// </summary>
    private void DrawMaximumApproachBox(){
        Gizmos.color = Color.blue;
        Vector2 forwardVector = transform.root.gameObject.GetComponentInChildren<CharacterStatus>().ForwardVector;
        if (CurrentEnemy == null)
        {
            Gizmos.DrawWireCube(hittingZone.transform.position + (Vector3)forwardVector * MaximumApproachDistance / 2,
                new Vector3(MaximumApproachDistance * forwardVector.x, hittingZone.bounds.size.y, hittingZone.bounds.size.z));
        }
        else
        {
            float correctedMaximumApproachDistance = MaximumApproachDistance - CurrentEnemy
                .GetComponentInChildren<ColliderController>()
                .CurrentColliderWidth;
            Gizmos.DrawWireCube(hittingZone.transform.position + (Vector3) forwardVector*correctedMaximumApproachDistance/2, 
                new Vector3(correctedMaximumApproachDistance * forwardVector.x,
                    hittingZone.bounds.size.y,
                    hittingZone.bounds.size.z));
        }
        
    }
        
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        DrawMaximumApproachBox();
    }
#endif
}
