using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Prince;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Prince
{
    /// <summary>
    /// This is the main guard AI. Its main goal is get near Prince and fight with him to death.
    /// </summary>
    public class GuardController : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to approach to Prince.")]
        [SerializeField] private EnemyPursuer enemyPursuer;
        [Tooltip("Needed to perform actions commands.")]
        [SerializeField] private InputController inputController;
        [Tooltip("Needed to perform fighting calculations.")]
        [SerializeField] private GuardFightingProfile guardFightingProfile;
        [Tooltip("Needed to check if Prince is at hitting range.")]
        [SerializeField] private FightingSensors fightingSensor;
        [Tooltip("Needed to know if we are being attacked and we have a chance to block that attack.")]
        [SerializeField] private FightingInteractions fightingInteractions;
        [Tooltip("Needed to know current character state.")]
        [SerializeField] private CharacterStatus characterStatus;

        [Header("CONFIGURATION:")] 
        [Tooltip("Time (in seconds) the guard does not move if he fails a boldness test.")] 
        [SerializeField] private float stopTime;
        [Tooltip("Time (in seconds) the guard does not attack if he fails a boldness test.")] 
        [SerializeField] private float stopAttackTime;
        [Tooltip("Time (in seconds) the guard does not try to block again if he fails a defense test.")]
        [SerializeField] private float stopBlockTime;
        

        private FightingProfile _fightingProfile;
        private bool _engagingEnemy = false;
        private bool _movementAllowed = true;
        private bool _attackAllowed = true;
        private bool _blockAllowed = true;

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
                
                // Must we block and incoming attack?
                if (_blockAllowed && fightingInteractions.BlockingStrikePossible) BlockAttack();
                
                // Chasing phase.
                if (_movementAllowed && ChaseEnemy()) return;
                
                // We can't approach nearer. May be Prince is unreachable or he is already at hitting range.
                if (fightingSensor.EnemyAtHittingRange)
                {
                    Debug.Log($"(GuardController - {transform.root.name}) We got to hitting range.");
                    // Fighting phase.
                    if (_attackAllowed) AttackEnemy();
                }
                else
                {
                    Debug.Log($"(GuardController - {transform.root.name}) Cannot fight because not in hitting range.");
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
            Debug.Log($"(GuardController - {transform.root.name}) EnemyPursuer proposed: {bestCommandToExecute}");
            if (bestCommandToExecute != Command.CommandType.Stop)
            {
                // We still need to move to be at hitting range, but will us be bold enough?
                if (Random.value < _fightingProfile.boldness)
                {
                    Debug.Log($"(GuardController - {transform.root.name}) Boldness check succeeded (threshold: {_fightingProfile.boldness}), performing proposed command.");
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
                Debug.Log($"(GuardController - {transform.root.name}) Boldness check failed (threshold: {_fightingProfile.boldness}), so guard will stay where it is.");
                // If we are not bold enough to advance just stay where we are.
                StartCoroutine(StayStoppedForAWhile());
                inputController.Stop();
                return true;
            }
            Debug.Log($"(GuardController - {transform.root.name}) Stopping.");
            inputController.Stop();
            return false;
        }

        /// <summary>
        /// Try to launch an strike against enemy if attack skill is high enough.
        ///
        /// A random value is extracted, if that value is under our attack profile then attack is performed else
        /// character does not tries to attack again for a time.
        /// </summary>
        private void AttackEnemy()
        {
            // If we are yet attacking just skip.
            if (characterStatus.CurrentState == CharacterStatus.States.AttackWithSword) return;
            
            // We want to attack, but will we have attack skill enough?
            if (Random.value < _fightingProfile.attack)
            {
                Debug.Log(
                    $"(GuardController - {transform.root.name}) Attack check succeeded (threshold: {_fightingProfile.attack}), performing attack against enemy.");
                inputController.Strike();
            }
            else
            {
                Debug.Log(
                    $"(GuardController - {transform.root.name}) Attack check failed (threshold: {_fightingProfile.attack}), just standing where we are.");
                // If we are not skilled enough to attack just stay where we are.
                StartCoroutine(DontAttackForAWhile());
                inputController.Stop();
            }
        }
        
        /// <summary>
        /// Try to block an strike from enemy if defense skill is high enough.
        ///
        /// A random value is extracted, if that value is under our defense profile then block is performed else
        /// character does not tries to block again for a time.
        /// </summary>
        private void BlockAttack()
        {
            // If we are yet blocking just skip.
            if (characterStatus.CurrentState == CharacterStatus.States.BlockSword) return;
            
            // We need to block, but will we have defense skill enough?
            if (Random.value < _fightingProfile.defense)
            {
                Debug.Log(
                    $"(GuardController - {transform.root.name}) Defense check succeeded (threshold: {_fightingProfile.attack}), blocking attack from enemy.");
                inputController.Block();
            }
            else
            {
                Debug.Log(
                    $"(GuardController - {transform.root.name}) Defense check failed (threshold: {_fightingProfile.attack}), we are going to receive a hit.");
                // If we are not skilled enough to block just stay where we are.
                StartCoroutine(DontBlockForAWhile());
                inputController.Stop();
            }
        }

        /// <summary>
        /// It is not enough to do nothing in a cycle if boldness test fails because they are performed so many times a second
        /// that only winning a fraction of times the character keeps moving in a apparently constant manner. To really
        /// stop character movement a waiting time is needed so ChaseEnemy() is not called for that waiting time. 
        /// </summary>
        private IEnumerator StayStoppedForAWhile()
        {
            _movementAllowed = false;
            yield return new WaitForSeconds(stopTime);
            _movementAllowed = true;
        }
        
        /// <summary>
        /// It is not enough to do nothing in a cycle if attack test fails because they are performed so many times a second
        /// that only winning a fraction of times the character keeps attacking in a apparently constant manner. To really
        /// stop character attacks a waiting time is needed so AttackEnemy() does nothing for that waiting time. 
        /// </summary>
        private IEnumerator DontAttackForAWhile()
        {
            _attackAllowed = false;
            yield return new WaitForSeconds(stopAttackTime);
            _attackAllowed = true;
        }
        
        /// <summary>
        /// It is not enough to do nothing in a cycle if attack test fails because they are performed so many times a second
        /// that only winning a fraction of times the character keeps attacking in a apparently constant manner. To really
        /// stop character attacks a waiting time is needed so AttackEnemy() does nothing for that waiting time. 
        /// </summary>
        private IEnumerator DontBlockForAWhile()
        {
            _blockAllowed = false;
            yield return new WaitForSeconds(stopBlockTime);
            _blockAllowed = true;
        }

        private void Awake()
        {
            _fightingProfile = guardFightingProfile.fightingProfile;
        }
        
    }
}
