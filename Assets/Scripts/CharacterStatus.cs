using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component keeps every variable about Character general state.
    /// </summary>
    [ExecuteAlways]
    public class CharacterStatus : MonoBehaviour
    {
        public enum States
        {
            Idle,
            TurnBack,
            Unsheathe,
            IdleSword,
            Sheathe,
            AdvanceSword,
            AttackWithSword, // I attack to my enemy.
            BlockSword, // I block an attack from my enemy.
            Retreat,
            HitBySword,
            KilledBySword,
            Falling,
            FallStart,
            Dead,
            CrouchFromStand,
            Crouch,
            StandFromCrouch,
            TurnBackWithSword,
            BlockedSword, // My attack was blocked by my enemy.
            CounterBlockSword, // My attack was blocked but I immediately block the attack from my enemy.
            CounterAttackWithSword, // I block the attack from my enemy and I immediately attack him.
            RunningStart,
            Running,
            RunningEnd,
            TurnBackRunning,
            Walk,
            CrouchWalking
        }
        
        [Header("WIRING:")]
        [Tooltip("Needed to set state machine parameters depending on status.")]
        [SerializeField] private Animator stateMachine;
        
        [Header("CONFIGURATION:")]
        [Tooltip("Current character life.")]
        [SerializeField] private int life;
        [Tooltip("Current character maximum life.")]
        [SerializeField] private int maximumLife;
        [Tooltip("This character has already a sword o should look for it first?")]
        [SerializeField] private bool hasSword;
        [Tooltip("Is this character looking rightwards?")]
        [SerializeField] private bool lookingRightWards;

        private bool _isFalling;
        public bool IsFalling
        {
            get=> _isFalling;
            set
            {
                if (_isFalling != value)
                {
                    _isFalling = value;
                    if (_isFalling)
                    {
                        stateMachine.SetTrigger("Fall");
                    } 
                    else
                    {
                        stateMachine.SetTrigger("Land");
                    }
                }
            }
        }
        
        // CurrentState is updated from StateUpdater components present in every machine state. 
        public States CurrentState { get; set;} 
    
        public int Life
        {
            get => life;
            set
            {
                life = Math.Clamp(value, 0, maximumLife);
                stateMachine.SetBool("isDead", IsDead);
            }
        }
    
        public int MaximumLife
        {
            get => maximumLife;
            set
            {
                if (value > 0)
                {
                    maximumLife = value;
                    life = Math.Clamp(life, 0, maximumLife);
                }
                
            }
        }
    
        public bool HasSword
        {
            get => hasSword;
            set
            {
                hasSword = value;
                stateMachine.SetBool("hasSword", value);
            }
        }
    
        public bool IsDead => (Life == 0);
    
        public bool LookingRightWards
        {
            get => lookingRightWards;
            set
            {
                lookingRightWards = value;
                stateMachine.SetBool("lookingRightWards", lookingRightWards);
            }
        }

        public Vector2 ForwardVector
        {
            get
            {
                return (LookingRightWards) ? Vector2.right : Vector2.left;
            }
        }
        
        /// <summary>
        /// UpdateAnimator flags that depend on character status.
        /// </summary>
        private void UpdateStateMachineFlags()
        {
            stateMachine.SetBool("hasSword", HasSword);
            stateMachine.SetBool("isDead", IsDead);
            stateMachine.SetBool("lookingRightWards", LookingRightWards);
        }

        private void Awake()
        {
            UpdateStateMachineFlags();
        }

        private void FixedUpdate()
        {
            UpdateStateMachineFlags();
        }

        private void OnValidate()
        {
            // Inspector can change only fields. So we update properties in case of field change.
            Life = life;
            MaximumLife = maximumLife;
            HasSword = hasSword;
            LookingRightWards = lookingRightWards;
        }
    }
}
