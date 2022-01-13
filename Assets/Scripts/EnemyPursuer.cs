using System;
using System.Collections;
using System.Collections.Generic;
using Prince;
using UnityEngine;

/// <summary>
/// Component to chase enemies if any is present.
///
/// This component is intended for guards. For prince character the player controls who to pursue.
///
/// This component offers in its NextPursuingCommand the command needed to get nearer of its most
/// recent enemy.
/// </summary>
public class EnemyPursuer : MonoBehaviour
{
    [Header("WIRING:")]
    [Tooltip("Needed to find enemies to pursue to.")]
    [SerializeField] private EnemySensors enemySensor;
    [Tooltip("Needed to get hitting range and use it as an approaching threshold.")]
    [SerializeField] private FightingSensors fightingSensors;
    [Tooltip("Needed to know where we are looking to.")]
    [SerializeField] private CharacterStatus characterStatus;
    [Tooltip("Needed to avoid holes when pursuing enemies in the same level.")]
    [SerializeField] private GroundSensors groundSensors;
    [Header("CONFIGURATION:")]
    [Tooltip("Once sighted, enemy will be pursued until going further than this range.")]
    [SerializeField] private float pursuingRange;
    
    public CharacterStatus PursuedEnemyStatus { get; private set; }
    
    private GameObject _pursuedEnemy;
    private float _hittingRange;
    
    private const float YTolerance = 0.30f;

    /// <summary>
    /// Enemy selected to be pursued.
    /// </summary>
    public GameObject PursuedEnemy
    {
        get => _pursuedEnemy;
        set
        {
            if (value != _pursuedEnemy)
            {
                _pursuedEnemy = value;
                if (value != null) PursuedEnemyStatus = value.GetComponentInChildren<CharacterStatus>();
            }
        }
    }
    
    /// <summary>
    /// Enemy is at hit range?
    /// </summary>
    public bool PursuedEnemyHittable
    {
        get
        {
            if ((PursuedEnemy != null) && (DistanceToEnemy(PursuedEnemy) < _hittingRange)) return true;
            return false;
        }
    }

    /// <summary>
    /// Next command to run to get nearer pursued enemy.
    /// </summary>
    public Command.CommandType NextPursuingCommand { get; private set; }

    /// <summary>
    /// Get distance to pursued enemy.
    /// </summary>
    /// <param name="enemy">Pursued enemy.</param>
    /// <returns>Distance to pursued enemy.</returns>
    private float DistanceToEnemy(GameObject enemy)
    {
        return Vector2.Distance(this.transform.position, enemy.transform.position);
    }

    /// <summary>
    /// Get next enemy to pursue.
    ///
    /// if forward and rear enemies are detected, this method returns the forward one.
    /// </summary>
    /// <returns>Enemy detected to pursue. Null if there is no enemy to pursue.</returns>
    private GameObject GetEnemyToPursue()
    {
        if (enemySensor.EnemySeen)
        {
            return enemySensor.EnemySeenForward ? enemySensor.ForwardEnemy : enemySensor.RearEnemy;
        }
        else
        {
            if ((PursuedEnemy != null) && (!PursuedEnemyStatus.IsDead))
            {
                // Enemy not seen any longer but can still be at range. He may have fallen to a lower level.
                if (DistanceToEnemy(_pursuedEnemy) > pursuingRange)
                {
                    // Enemy already out of range.
                    return null;
                }
                else
                {
                    // Current enemy is still at range.
                    return PursuedEnemy;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Get command to move forward depending on character facing.
    /// </summary>
    /// <returns>Command to move forward.</returns>
    private Command.CommandType GetForwardCommand()
    {
        if (characterStatus.LookingRightWards)
        {
            return Command.CommandType.WalkRightWithSword;
        }
        else
        {
            return Command.CommandType.WalkLeftWithSword;
        }
    }

    /// <summary>
    /// Is there a hole just at right of character?
    /// </summary>
    /// <returns>True if there is a hole.</returns>
    private bool HoleAtRight()
    {
        return characterStatus.LookingRightWards ? !groundSensors.GroundAhead : !groundSensors.GroundBehind;
    }

    /// <summary>
    /// Is there a hole just at left of character?
    /// </summary>
    /// <returns>True if there is a hole.</returns>
    private bool HoleAtLeft()
    {
        return characterStatus.LookingRightWards ? !groundSensors.GroundBehind : !groundSensors.GroundAhead;
    }

    /// <summary>
    /// Get best command to get nearer to pursued enemy.
    ///
    /// Be aware that "nearer" means up to hitting range. When enemy is within hitting range this method
    /// returns stop.
    /// </summary>
    /// <param name="pursuedEnemy">Enemy to get nearer to.</param>
    /// <returns>Next command to execute to get nearer.</returns>
    private Command.CommandType GetNextPursuingCommand(GameObject pursuedEnemy)
    {
        if (pursuedEnemy == null) return Command.CommandType.Stop;
        
        Debug.Log($"(EnemyPursuer - {gameObject.transform.parent.name}) EnemyPursuer chasing Prince.");
        Vector2 pursuedPosition = pursuedEnemy.transform.position;
        Vector2 currentPosition = this.transform.position;
        
        // Enemy cannot climb so stop.
        if (pursuedPosition.y > currentPosition.y && Math.Abs(pursuedPosition.y - currentPosition.y) > YTolerance)
        {
            Debug.Log($"(EnemyPursuer - {gameObject.transform.parent.name}) Prince is higher. As cannot climb, proposing Stop.");
            return Command.CommandType.Stop;
        } 
        // Enemy at same level. Chase him if there's not any hole in the way.
        else if (Math.Abs(pursuedPosition.y - currentPosition.y) < YTolerance)
        {
            Debug.Log($"(EnemyPursuer - {gameObject.transform.parent.name}) Prince at the same level. Choosing best approach.");
            float horizontalDistance = pursuedPosition.x - currentPosition.x;
            float absHorizontalDistance = Math.Abs(horizontalDistance);
            if (absHorizontalDistance <= pursuingRange && absHorizontalDistance > _hittingRange)
            {
                // This method is only for guards and they walk only with swords unsheathed.
                if (horizontalDistance > 0 && absHorizontalDistance > _hittingRange && !HoleAtRight())
                {
                    Debug.Log($"(EnemyPursuer - {gameObject.transform.parent.name}) Proposing WalkRightWithSword with horizontalDistance: {horizontalDistance} and absHorizontalDistance: {absHorizontalDistance}");
                    return Command.CommandType.WalkRightWithSword;
                }
                if (horizontalDistance < 0 && absHorizontalDistance > _hittingRange && !HoleAtLeft()) return Command.CommandType.WalkLeftWithSword;
            }
            if (absHorizontalDistance > pursuingRange) Debug.Log($"(EnemyPursuer - {gameObject.transform.parent.name}) Prince beyond pursuing range ({pursuingRange}). Proposing Stop.");
            if (absHorizontalDistance <= _hittingRange) Debug.Log($"(EnemyPursuer - {gameObject.transform.parent.name}) Prince already at hitting range ({_hittingRange}). Proposing Stop.");
            if (HoleAtRight() || HoleAtLeft()) Debug.Log($"(EnemyPursuer - {gameObject.transform.parent.name}) Hole blocking route. Right hole: {HoleAtRight()}. Left hole: {HoleAtLeft()} Proposing Stop.");
            return Command.CommandType.Stop;
        } 
        // Enemy below. We saw him (if not we would not be aware of him) but he has disappeared probably
        // because he has fallen through a hole in front of us. So we got that hole and jump through it too.
        else if ((pursuedPosition.y < currentPosition.y && Math.Abs(pursuedPosition.y - currentPosition.y) > YTolerance) 
                 && DistanceToEnemy(pursuedEnemy) <= pursuingRange 
                 && characterStatus.CurrentState != CharacterStatus.States.Falling)
        {
            Debug.Log($"(EnemyPursuer - {gameObject.transform.parent.name}) Prince in a lower level. Choosing best option to chase him.");
            return GetForwardCommand();
        }
        // We are falling while we chase him. Let update our next command to look in the correct direction as soon
        // as we land on ground.
        else if ((pursuedPosition.y < currentPosition.y) && DistanceToEnemy(pursuedEnemy) <= pursuingRange && characterStatus.CurrentState == CharacterStatus.States.Falling)
        {
            Debug.Log($"(EnemyPursuer - {gameObject.transform.parent.name}) Prince detected while we fall. Choosing best option to chase him.");
            float horizontalDistance = pursuedPosition.x - currentPosition.x;
            if (horizontalDistance > 0) return Command.CommandType.WalkRightWithSword;
            if (horizontalDistance < 0) return Command.CommandType.WalkLeftWithSword;
        }
        return Command.CommandType.Stop;
    }
    
    private void FixedUpdate()
    {
        switch (characterStatus.CurrentState)
        {
            // In some states is useless wasting time calculating because we cannot move yet.
            // case CharacterStatus.States.Falling:
            // case CharacterStatus.States.FallStart:
            // case CharacterStatus.States.Crouch:
            // case CharacterStatus.States.CrouchFromStand:
            // case CharacterStatus.States.Landing:
            //     break;
            // In every other case get the job done.
            default:
                PursuedEnemy = GetEnemyToPursue();
                NextPursuingCommand = GetNextPursuingCommand(PursuedEnemy);
                break;
        }
    }

    private void Awake()
    {
        _hittingRange = fightingSensors.HittingRange;
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, pursuingRange);
    }
#endif
    
}
