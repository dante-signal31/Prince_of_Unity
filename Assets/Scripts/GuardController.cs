using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Prince;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// This is the main guard AI. Its main goal is get near Prince and fight with him to death.
/// </summary>
public class GuardController : MonoBehaviour
{
    [Header("WIRING:")]
    [Tooltip("Needed to get Prince to hitting range.")]
    [SerializeField] private EnemyPursuer enemyPursuer;
    [Tooltip("Needed to perform actions commands.")]
    [SerializeField] private InputController inputController;
    [Tooltip("Needed to perform fighting calculations.")]
    [SerializeField] private GuardFightingProfile guardFightingProfile;

    private FightingProfile _fightingProfile;
    private bool _engagingEnemy = false;

    private void FixedUpdate()
    {
        if (enemyPursuer.PursuedEnemy != null)
        {
            if (!_engagingEnemy)
            {
                // Unsheathe.
                inputController.Action();
                _engagingEnemy = true;
            }
            
            // Chasing phase.
            if (ChaseEnemy()) return;
            
            // We can't approach nearer. May be Prince is unreachable or he is already at hitting range.
            if (enemyPursuer.PursuedEnemyHittable)
            {
                // Fighting phase.
                Debug.Log($"(GuardController - {gameObject.transform.parent.name}) We got to hitting range.");
            } 
        }
        else
        {
            if (_engagingEnemy)
            {
                inputController.Sheathe();
                _engagingEnemy = true;
            }
        }
    }

    /// <summary>
    /// Move towards enemy if possible and if we are bold enough.
    ///
    /// This function returns true if chase was possible, but false otherwise. A false value means
    /// didn't find a route to get nearer to enemy. That can happens if enemy is out of range and
    /// cannot be approached or because we are already at hitting range. So a false value should
    /// be further tested against hitting range to decide if we can fight against enemy or not.
    /// </summary>
    /// <returns>True if chase was possible, false otherwise.</returns>
    private bool ChaseEnemy()
    {
        Command.CommandType bestCommandToExecute = enemyPursuer.NextPursuingCommand;
        Debug.Log($"(GuardController - {gameObject.transform.parent.name}) EnemyPursuer proposed: {bestCommandToExecute}");
        if (bestCommandToExecute != Command.CommandType.Stop)
        {
            // We still need to move to be at hitting range, but will we bold enough?
            if (Random.value < _fightingProfile.boldness)
            {
                Debug.Log($"(GuardController - {gameObject.transform.parent.name}) Boldness check succeeded (threshold: {_fightingProfile.boldness}), performing proposed command.");
                switch (bestCommandToExecute)
                {
                    case Command.CommandType.WalkRightWithSword:
                        inputController.WalkRightWithSword();
                        break;
                    case Command.CommandType.WalkLeftWithSword:
                        inputController.WalkLeftWithSword();
                        break;
                }
                return true;
            }
            Debug.Log($"(GuardController - {gameObject.transform.parent.name}) Boldness check failed (threshold: {_fightingProfile.boldness}), so guard will stay where it is.");
            // If we are not bold enough to advance just stay where we are.
            inputController.Stop();
            return true;
        }
        return false;
    }

    private void Awake()
    {
        _fightingProfile = guardFightingProfile.fightingProfile;
    }
    
}
