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
        [SerializeField] private int life;
    
        [SerializeField] private int maximumLife;
    
        [SerializeField] private bool hasSword;
    
        [SerializeField] private Animator stateMachine;
    
        [SerializeField] private bool lookingRightWards;
    
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
        
    }
}
