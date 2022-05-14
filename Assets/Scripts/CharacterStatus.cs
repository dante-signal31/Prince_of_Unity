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
            CrouchWalking,
            Landing,
            HardLanding,
            DeadByFall,
            RunningJumpImpulse,
            RunningJump,
            WalkingJump,
            WalkingJumpStart,
            WalkingJumpEnd,
            VerticalJumpStart,
            VerticalJump,
            VerticalJumpEnd,
            Climbing,
            Descending,
        }

        public enum JumpingTypes
        {
            RunningJumping,
            WalkingJumping,
            None
        }
        
        [Header("WIRING:")]
        [Tooltip("Needed to set state machine parameters depending on status.")]
        [SerializeField] private Animator stateMachine;
        [Tooltip("Needed to know is enabled and we can fall.")]
        [SerializeField] private GravityController gravityController;
        [Tooltip("Needed to know if we hve ground under our feet.")]
        [SerializeField] private GroundSensors groundSensors;
        
        [Header("CONFIGURATION:")]
        [Tooltip("Current character life.")]
        [SerializeField] private int life;
        [Tooltip("Current character maximum life.")]
        [SerializeField] private int maximumLife;
        [Tooltip("This character has already a sword o should look for it first?")]
        [SerializeField] private bool hasSword;
        [Tooltip("Is this character looking rightwards?")]
        [SerializeField] private bool lookingRightWards;
        
        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;

        private bool _isFalling;
        
        /// <summary>
        /// Whether this character is falling or not.
        /// </summary>
        public bool IsFalling
        {
            get=> _isFalling;
            
            // This value is set from GroundSensors.
            //
            // I don't like IsFalling being set from another component. This component should
            // read GroundSensor and update itself accordingly. Problem is that I've tried
            // to refactor in that way and I've meet odd problems, likely because of some 
            // time race between GroundSensor and CharacterStatus. In my test I've only got an
            // stable behaviour if IsFalling is set from GroundSensor.
            set
            {
                if (_isFalling != value)
                {
                    if (value && gravityController.GravityEnabled)
                    {
                        _isFalling = true;
                        stateMachine.SetTrigger("Fall");
                        this.Log($"(CharacterStatus - {transform.root.name}) We are falling.", showLogs);
                    } 
                    else if (gravityController.GravityEnabled)
                    {
                        _isFalling = false;
                        // stateMachine.SetTrigger("Land");
                        this.Log($"(CharacterStatus - {transform.root.name}) We are landing.", showLogs);
                    }
                    else
                    {
                        _isFalling = false;
                        this.Log($"(CharacterStatus - {transform.root.name}) Gravity is not enabled so we are not falling.", showLogs);
                    }
                }
            }
        }

        private States _currentState;
        
        /// <summary>
        /// <p>Jumping sequence this character is performing</p>
        ///
        /// <p>You can jump over an empty space and chain that jump with a fall.
        /// Fall speeds are different depending on the jumping type. So we must
        /// take note of current fall type if we are performing one. If we are
        /// not doing any jump sequence then CurrentJumpingType is None.</p>
        /// <p>CurrentJumpingType is set to None in any of the landing states.</p>
        /// </summary>
        public JumpingTypes CurrentJumpingSequence { get; private set; }
        
        /// <summary>
        /// State of this character.
        /// </summary>
        public States CurrentState
        {
            get => _currentState;
            
            set
            {
                _currentState = value;
                CurrentJumpingSequence = value switch
                {
                    States.RunningJumpImpulse => JumpingTypes.RunningJumping,
                    States.WalkingJump => JumpingTypes.WalkingJumping,
                    States.Landing or States.HardLanding => JumpingTypes.None,
                    // When you chain a running jump with a fall you pass through 
                    // running state. I only
                    // want reset CurrentJumpingSequence to None only when we
                    // chain a running jump with running (i.e. we jump through
                    // hole and we continue running on the other side). Than last
                    // thing happens when you have ground under your feet.
                    // States.Running when groundSensors.GroundBelow => JumpingTypes.None,
                    _ when groundSensors.GroundBelow => JumpingTypes.None,
                    _ => CurrentJumpingSequence
                };
                
            } 
        }

        public int Life
        {
            get => life;
            set
            {
                life = Math.Clamp(value, 0, maximumLife);
                // Added check to get rid of "Animator is not playing an AnimatorController" warning.
                if (stateMachine.isActiveAndEnabled) stateMachine.SetBool("isDead", IsDead);
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
                // Added check to get rid of "Animator is not playing an AnimatorController" warning.
                if (stateMachine.isActiveAndEnabled) stateMachine.SetBool("hasSword", value);
            }
        }
    
        public bool IsDead => (Life == 0);
    
        public bool LookingRightWards
        {
            get => lookingRightWards;
            set
            {
                lookingRightWards = value;
                // Added check to get rid of "Animator is not playing an AnimatorController" warning.
                if (stateMachine.isActiveAndEnabled) stateMachine.SetBool("lookingRightWards", lookingRightWards);
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
            //stateMachine.SetBool("OnGround", !IsFalling);
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
