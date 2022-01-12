using System;
using System.Collections;
using System.Collections.Generic;
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

    /// <summary>
    /// Distance from character where hitting range starts.
    /// </summary>
    public float HittingRange => hittingZone.size.x;
    
    /// <summary>
    /// Enemy currently in hitting range.
    /// </summary>
    public GameObject CurrentEnemy { get; private set;}
    
    /// <summary>
    /// Friend or foe identification.
    /// </summary>
    /// <param name="other">Other guy.</param>
    /// <returns>True if he is our enemy, false if not.</returns>
    private bool IsOurEnemy(Collider2D other)
    {
        string otherTag = other.gameObject.tag;
        if (imGuard && otherTag == "Player") return true;
        if (imGuard && otherTag == "Guard") return false;
        if (!imGuard && otherTag == "Guard") return true;
        if (!imGuard && otherTag == "Player") return false;
        // We aren't going to get here.
        return false; 
    }

    /// <summary>
    /// Get main GameObject this collider is attached.
    /// </summary>
    /// <param name="col">Collider detected</param>
    /// <returns>Root GameObject this collider is attached to.</returns>
    private GameObject GetRootGameObject(Collider2D col)
    {
        // Non trigger collider are attached at a subtransform of Physics subtransform so be must go up twice.
        return col.transform.parent.transform.parent.gameObject;
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

}
