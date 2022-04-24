using System;
using System.Collections;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Component that handles climbing, hanging and descending interactions with Climbable game objects.
    /// </summary>
    public class ClimberInteractions : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to know when we enter climbing states.")] 
        [SerializeField] private CharacterStatus characterStatus;
        [Tooltip("Needed to send signals to character state machine")] 
        [SerializeField] private Animator stateMachine;
        [Tooltip("Needed to reposition character after climbing.")] 
        [SerializeField] private Transform characterTransform;
        [Tooltip("Needed to get a reference to climbable game object.")]
        [SerializeField] private CeilingSensors ceilingSensors;
        [Tooltip("Needed to get a reference to climbable game objects to descend.")]
        [SerializeField] private GroundSensors groundSensors;
        
        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;
        
        private enum ClimbingOptions
        {
            Climb,
            Descend,
        }
        
        /// <summary>
        /// Whether the character is currently climbing.
        /// </summary>
        public bool ClimbingInProgress => _climbable != null;

        /// <summary>
        /// Whether player could climb if he should want.
        /// </summary>
        public bool ClimbingPossible => ceilingSensors.LedgeReachable && !ceilingSensors.RoofOverHead;

        /// <summary>
        /// Whether current climbing can still be aborted.
        /// </summary>
        public bool ClimbingAbortable => ((_climbable != null) && (_climbable.ClimbingAbortable));
        
        private Climbable _climbable;
        private bool _climbAborted;
        private bool _actionPushed;
        private bool _jumpPushed;
        
        
        private void FixedUpdate()
        {
            switch (characterStatus.CurrentState)
            {
                case CharacterStatus.States.Climbing:
                    if (!ClimbingInProgress) StartCoroutine(Climb());
                    break;
                case CharacterStatus.States.Descending:
                    if (!ClimbingInProgress) StartCoroutine(Descend());
                    break;
            }
        }

        private IEnumerator Climb()
        {
            _climbable = ceilingSensors.Ledge.GetComponentInChildren<Climbable>();
            if (_climbable != null)
            {
                this.Log($"(ClimberInteractions - {transform.root.name}) Starting climbing.", showLogs);
                Climbable.HangableLedges hangingLedge = (characterStatus.LookingRightWards)
                    ? Climbable.HangableLedges.Left
                    : Climbable.HangableLedges.Right;
                yield return _climbable.Hang(hangingLedge);
                if (_climbAborted)
                {
                    this.Log($"(ClimberInteractions - {transform.root.name}) Climbing aborted.", showLogs);
                    _climbAborted = false;
                    _climbable = null;
                    stateMachine.SetTrigger("ClimbingAborted");
                }
                else
                {
                    UpdateCharacterPosition(hangingLedge, ClimbingOptions.Climb);
                    // yield return new WaitUntil(() => !_climbable.PlayingAnimations);
                    stateMachine.SetTrigger("ClimbingFinished");
                    _climbable = null;
                    this.Log($"(ClimberInteractions - {transform.root.name}) Climbing finished.", showLogs);
                }
               
            }
        }

        /// <summary>
        /// Used to signal that the jump button has been pushed.
        /// </summary>
        public void JumpPushed()
        {
            if (ClimbingInProgress) _climbable.JumpPushed(true);
            _jumpPushed = true;
        }
        
        /// <summary>
        /// Used to signal climbable that jump button has been released.
        /// </summary>
        public void JumpReleased()
        {
            if ((ClimbingInProgress) && (_climbable.ClimbingAbortable))
            {
                _climbable.JumpPushed(false);
                _jumpPushed = false;
                if (!_actionPushed) _climbAborted = true;
            }
        }

        /// <summary>
        /// Used to signal climbable that action has been pushed.
        /// </summary>
        public void ActionPushed()
        {
            if (ClimbingInProgress)
            {
                _climbable.ActionPushed(true);
                _actionPushed = true;
                _climbAborted = false;
            }
        }
        
        /// <summary>
        /// Used to signal climbable that action has been released.
        /// </summary>
        public void ActionReleased()
        {
            if (ClimbingInProgress)
            {
                _climbable.ActionPushed(false);
                _actionPushed = false;
                _climbAborted = true;
            }
        }
        
        private IEnumerator Descend()
        {
            _climbable = groundSensors.CenterGround.GetComponentInChildren<Climbable>();
            if (_climbable != null)
            {
                this.Log($"(ClimberInteractions - {transform.root.name}) Starting descend.", showLogs);
                Climbable.HangableLedges hangingLedge = (characterStatus.LookingRightWards)
                    ? Climbable.HangableLedges.Left
                    : Climbable.HangableLedges.Right;
                yield return _climbable.Descend(hangingLedge);
                UpdateCharacterPosition(hangingLedge, ClimbingOptions.Descend);
                stateMachine.SetTrigger("ClimbingFinished");
                _climbable = null;
                this.Log($"(ClimberInteractions - {transform.root.name}) Descend finished.", showLogs);
                
                // if (_climbAborted)
                // {
                //     this.Log($"(ClimberInteractions - {transform.root.name}) Climbing aborted.", showLogs);
                //     _climbAborted = false;
                //     _climbable = null;
                // }
                // else
                // {
                //     UpdateCharacterPosition(hangingLedge, ClimbingOptions.Climb);
                //     // yield return new WaitUntil(() => !_climbable.PlayingAnimations);
                //     stateMachine.SetTrigger("ClimbingFinished");
                //     _climbable = null;
                //     this.Log($"(ClimberInteractions - {transform.root.name}) Climbing finished.", showLogs);
                // }
               
            }
        }

        /// <summary>
        /// Translates the character to the correct position after climbing or descending.
        /// </summary>
        /// <param name="hangingLedge">The side we are climbing or descending.</param>
        /// <param name="climbingOption">Whether we are climbing or descending.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void UpdateCharacterPosition(Climbable.HangableLedges hangingLedge, ClimbingOptions climbingOption)
        {
            Vector3 newPosition = hangingLedge switch {
                Climbable.HangableLedges.Left => (climbingOption == ClimbingOptions.Climb)
                    ? _climbable.LeftLedgeTransform.position
                    : _climbable.LeftDescendingTransform.position,
                Climbable.HangableLedges.Right => (climbingOption == ClimbingOptions.Climb)
                    ? _climbable.RightLedgeTransform.position
                    : _climbable.RightDescendingTransform.position,
                _ => throw new ArgumentOutOfRangeException(nameof(hangingLedge), hangingLedge, null)
            };
            characterTransform.position = new Vector3(newPosition.x, newPosition.y, characterTransform.position.z);
        }

    }
}