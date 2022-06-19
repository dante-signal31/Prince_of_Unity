using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Component to allow a brick detect when they are being climbed.
    ///
    /// When it detects brick is being climbed it sets a IsBeingClimbed bool to true at current
    /// state machine.
    /// </summary>
    public class ClimbingSensor : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to know when we are being climbed.")] 
        [SerializeField] private ClimbableStatus ClimbableStatus;
        [Tooltip("Brick state machine. Needed to signal we are being climbed")] 
        [SerializeField] private Animator stateMachine;

        /// <summary>
        /// True if this brick is being climbed.
        /// </summary>
        public bool IsBeingClimbed => ClimbableStatus.CurrentState != ClimbableStatus.States.Inactive;

        private bool _climbingAlreadySignaled;
        
        private void FixedUpdate()
        {
            if (IsBeingClimbed && !_climbingAlreadySignaled)
            {
                stateMachine.SetBool("IsBeingClimbed", true);
                _climbingAlreadySignaled = true;
            } 
            else if (!IsBeingClimbed && _climbingAlreadySignaled)
            {
                stateMachine.SetBool("IsBeingClimbed", false);
                _climbingAlreadySignaled = false;
            }
        }
    }
}