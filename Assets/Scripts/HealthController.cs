using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component centralizes every health change for character and signals every state
    /// change that depends on that life.
    /// </summary>
    public class HealthController : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to get and update current life.")]
        [SerializeField] private CharacterStatus characterStatus;
        [Tooltip("Needed to signal state machine state changes related to health conditions.")]
        [SerializeField] private Animator stateMachine;
        [Tooltip("Needed to show damage when hit by a falling ground.")]
        [SerializeField] private DamageEffect damageEffects;
        [Tooltip("Needed to know if we are a guard or not.")]
        [SerializeField] private FightingInteractions fightingInteractions;

        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;

        private CharacterStatus.States _oldState;
        
        /// <summary>
        /// Current character life.
        ///
        /// Actually, just a forwarder to CharacterStatus life property.
        /// </summary>
        private int Life
        {
            get => characterStatus.Life;
            //set => characterStatus.Life = value;
            set => characterStatus.Life = Math.Clamp(value, 0, MaximumLife);
        }

        /// <summary>
        /// Current character maximum life.
        ///
        /// Actually, just a forwarder to CharacterStatus maximum life property.
        /// </summary>
        private int MaximumLife
        {
            get => characterStatus.MaximumLife;
            set => characterStatus.MaximumLife = (value > 0) ? value : 0;
        }

        /// <summary>
        /// Add a new health point.
        /// </summary>
        public void HealLifePoint()
        {
            Life++;
        }

        /// <summary>
        /// Set life point to its maximum.
        /// </summary>
        public void HealToFullLife()
        {
            Life = MaximumLife;
        }

        /// <summary>
        /// Add a new maximum life point.
        /// </summary>
        public void AddMaximumLifePoint()
        {
            MaximumLife++;
        }
        
        /// <summary>
        /// Character has been hit by an enemy's sword.
        /// </summary>
        public void SwordHit()
        {
            switch (characterStatus.CurrentState)
            {
                // If character has sword unsheathed then only loses a life.
                case CharacterStatus.States.IdleSword:
                case CharacterStatus.States.AdvanceSword:
                case CharacterStatus.States.AttackWithSword:
                case CharacterStatus.States.BlockedSword:
                case CharacterStatus.States.CounterBlockSword:
                case CharacterStatus.States.BlockSword:
                case CharacterStatus.States.Retreat:
                    Life--;
                    break;
                // In every other case character is directly killed.
                default:
                    Life = 0;
                    break;
            }
            
            if (characterStatus.IsDead)
            {
                this.Log($"(HealthController - {transform.root.name}) Dead by sword.", showLogs);
                stateMachine.SetBool("isDead", true);
                stateMachine.SetTrigger("Hit");
            } 
            else
            {
                this.Log($"(HealthController - {transform.root.name}) Hit by sword. New current life: {Life}", showLogs);
                stateMachine.SetTrigger("Hit");
            }
        }

        /// <summary>
        /// Character has been hit by a falling ground.
        /// </summary>
        /// <param name="damage">Damage amount to inflict to character.</param>
        public void GroundHit(int damage)
        {
            //Life = Math.Clamp(Life - damage, 0, MaximumLife);
            // Don't change for Life =- damage. Maybe because it is a property but if you do that way wrong things happen.
            Life = Life - damage;
            if (characterStatus.IsDead)
            {
                this.Log($"(HealthController - {transform.root.name}) Dead by falling ground.", showLogs);
                stateMachine.SetBool("isDead", true);
                Invoke(nameof(ShowFallingGroundHitEffect), 0.05f);
            } 
            else
            {
                this.Log($"(HealthController - {transform.root.name}) Hit by falling ground. New current life: {Life}", showLogs);
                Invoke(nameof(ShowFallingGroundHitEffect), 0.15f);
            }
        }

        /// <summary>
        /// Make Prince crouch and show damage signal.
        /// </summary>
        private void ShowFallingGroundHitEffect()
        {
            stateMachine.SetTrigger("HitByFallingGround");
            damageEffects.ShowFallingGroundHit(fightingInteractions.ImGuard);
        }

        private void FixedUpdate()
        {
            if (characterStatus.CurrentState != _oldState)
            {
                switch (characterStatus.CurrentState)
                {
                    case CharacterStatus.States.HardLanding:
                        Life--;
                        this.Log($"(HealthController - {transform.root.name}) Hard landing. Now life is {Life}.", showLogs);
                        break;
                    case CharacterStatus.States.DeadByFall:
                        Life = 0;
                        this.Log($"(HealthController - {transform.root.name}) Deadly landing.", showLogs);
                        break;
                    case CharacterStatus.States.KilledByTrap:
                        Life = 0;
                        this.Log($"(HealthController - {transform.root.name}) Killed by a trap.", showLogs);
                        break;
                }
                _oldState = characterStatus.CurrentState;
            }
            
        }
    }
}