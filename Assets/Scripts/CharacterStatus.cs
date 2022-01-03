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
            AttackWithSword,
            BlockSword,
            Retreat,
            HitBySword,
            KilledBySword
        }
        
        [SerializeField] private int life;
    
        [SerializeField] private int maximumLife;
    
        [SerializeField] private bool hasSword;
    
        [SerializeField] private Animator stateMachine;
    
        [SerializeField] private bool lookingRightWards = true;
        
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
            // TurnBackState set this.lookingRightWards when turn back animation has ended.
            TurnBackState turnBackState = stateMachine.GetBehaviour<TurnBackState>();
            turnBackState.characterStatus = this;
            // StateUpdaters set CurrentState when every state at Animator is started.
            StateUpdater[] stateUpdaters = stateMachine.GetBehaviours<StateUpdater>();
            foreach (StateUpdater state in stateUpdaters)
            {
                state.characterStatus = this;
            }
        }
        
        private void Awake()
        {
            LinkWithStateMachine();
            UpdateStateMachineFlags();
        }
        
    }
}
