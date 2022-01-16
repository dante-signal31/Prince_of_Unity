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
            Landing,
            BlockedSword, // My attack was blocked by my enemy.
            CounterBlockSword, // My attack was blocked but I immediately block the attack from my enemy.
            CounterAttackWithSword, // I block the attack from my enemy and I immediately attack him.
        }
        
        [SerializeField] private int life;
    
        [SerializeField] private int maximumLife;
    
        [SerializeField] private bool hasSword;
    
        [SerializeField] private Animator stateMachine;
    
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
                    stateMachine.SetBool("isFalling", value);
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

        /// <summary>
        /// Create references to CharacterStatus in StateMachineBehaviours.
        ///
        /// Unlike MonoBehaviours I've not been able to reference CharacterStatus in StateMachineBehaviours
        /// using inspector nor calling gameObject from StateMachineBehaviour itself. So I must link it
        /// from outside.
        /// </summary>
        private void LinkWithStateMachine()
        {
            // TurnBackState's set this.lookingRightWards when turn back animation has ended.
            TurnBackState[] turnBackStates = stateMachine.GetBehaviours<TurnBackState>();
            foreach (TurnBackState state in turnBackStates)
            {
                state.characterStatus = this;
            }
            // StateUpdaters set CurrentState when every state at Animator is started.
            // StateUpdater[] stateUpdaters = stateMachine.GetBehaviours<StateUpdater>();
            // foreach (StateUpdater state in stateUpdaters)
            // {
            //     state.characterStatus = this;
            // }
        }
        
        private void Awake()
        {
            LinkWithStateMachine();
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
