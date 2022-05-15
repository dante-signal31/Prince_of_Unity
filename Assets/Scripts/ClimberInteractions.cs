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
        [Tooltip("Needed play hanging sounds.")]
        [SerializeField] private SoundController soundController;
        
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
        private bool _fallingHangingAllowed;

        private bool FallingHangingAllowed
        {
            get
            {
                return _fallingHangingAllowed;
            }
            set
            {
                if (_fallingHangingAllowed != value)
                {
                    _fallingHangingAllowed = value;
                    stateMachine.SetBool("FallingHangingAllowed", _fallingHangingAllowed);
                    this.Log($"(ClimberInteractions - {transform.root.name}) FallingHangingAllowed set to: {_fallingHangingAllowed}.", showLogs);
                }
            }
            
        }
        
        private void Start()
        {
            // FallingHangingAllowed = true;
            _fallingHangingAllowed = stateMachine.GetBool("FallingHangingAllowed");
        }

        private void FixedUpdate()
        {
            // FixFallingHangingAllowedFlag();
            switch (characterStatus.CurrentState)
            {
                case CharacterStatus.States.Climbing when characterStatus.IsFalling:
                    if (!ClimbingInProgress)
                    {
                        // // TODO: Remove this _actionPushed = true. It is only useful for tests. Usual game play set it by gameplay events.
                        // _actionPushed = true;
                        StartCoroutine(Climb(fallingHanging: true));
                    }
                    break;
                case CharacterStatus.States.Climbing:
                    if (!ClimbingInProgress) StartCoroutine(Climb());
                    break;
                case CharacterStatus.States.Descending:
                    if (!ClimbingInProgress) StartCoroutine(Descend());
                    break;
            }
        }

        // /// <summary>
        // /// This embarrasing, but I don't know why FallingHangingAllowed animator flag backs to false after being set at start
        // /// and with no call to SuspendFallingHanging(). Oddly it does not happen in normal game execution but
        // /// in play tests. I've check calls to stateMachine.SetBool("FallingHangingAllowed") and the only one
        // /// is in FallingHangingAllowed property and that one is only called at Start(), SuspendFallingHanging()
        // /// and AllowFallingHanging(). Even when the later two methods are not called FallingHangingAllowed animator
        // /// flag is set to true after Start(), stays in that value for a FixedUpdate() round and in the next one is back
        // /// to false. It only happens in play mode tests.
        // ///
        // /// So, this method is a workaround to fix that. Hopefully it will only be needed in play mode tests.
        // /// </summary>
        // private void FixFallingHangingAllowedFlag()
        // {
        //     if (FallingHangingAllowed != stateMachine.GetBool("FallingHangingAllowed"))
        //     {
        //         this.Log(
        //             $"(ClimberInteractions - {transform.root.name}) State mismatch. FallingHangingAllowed: {FallingHangingAllowed} while in state machine is {stateMachine.GetBool("FallingHangingAllowed")}",
        //             showLogs);
        //         stateMachine.SetBool("FallingHangingAllowed", FallingHangingAllowed);
        //         this.Log(
        //             $"(ClimberInteractions - {transform.root.name}) Fix applied to state mismatch.",
        //             showLogs);
        //     }
        // }

        private void AllowFallingHanging()
        {
            FallingHangingAllowed = true;
            this.Log($"(ClimberInteractions - {transform.root.name}) Falling allowed again.", showLogs);
        }
        
        /// <summary>
        /// Suspend temporarily falling hanging.
        /// </summary>
        /// <param name="duration">Time in seconds to suspend falling hanging.</param>
        private void SuspendFallingHanging(float duration)
        {
            FallingHangingAllowed = false;
            Invoke(nameof(AllowFallingHanging), duration);
            this.Log($"(ClimberInteractions - {transform.root.name}) Falling hanging suspended for {duration} seconds.", showLogs);
        }

        /// <summary>
        /// Climb a climbable brick
        ///
        /// This method is called both when prince wants to climb a brick and when he is falling
        /// and trying to grab a ledge to hang from it.
        /// </summary>
        /// <param name="fallingHanging">True if character is falling when he tries to grab the ledge.</param>
        private IEnumerator Climb(bool fallingHanging = false)
        {
            _climbable = fallingHanging
                ? ceilingSensors.FallingLedge.GetComponentInChildren<Climbable>()
                : ceilingSensors.Ledge.GetComponentInChildren<Climbable>();
            if (_climbable != null)
            {
                this.Log($"(ClimberInteractions - {transform.root.name}) Starting climbing. " +
                         $"Falling hanging: {fallingHanging}" +
                         $" Action pushed:{_actionPushed} Jump pushed: {_jumpPushed}", showLogs);
                if (fallingHanging) soundController.PlaySound("hanged_after_jump");
                Climbable.HangableLedges hangingLedge = (characterStatus.LookingRightWards)
                    ? Climbable.HangableLedges.Left
                    : Climbable.HangableLedges.Right;
                yield return _climbable.Hang(hangingLedge, _actionPushed, _jumpPushed);
                // If we tried to climb and ended descending then we aborted climbing.
                if (_climbable.ClimbingResult == ClimbableStatus.ClimbingResult.Descended)
                {
                    this.Log($"(ClimberInteractions - {transform.root.name}) Climbing aborted.", showLogs);
                    stateMachine.SetTrigger("ClimbingAborted");
                    if (fallingHanging)
                    {
                        SuspendFallingHanging(0.3f);
                        UpdateCharacterPosition(hangingLedge, ClimbingOptions.Descend);
                    }
                }
                // If we tried to climb and ended up the brick (climbed) then everything was ok.
                else if (_climbable.ClimbingResult == ClimbableStatus.ClimbingResult.Climbed)
                {
                    UpdateCharacterPosition(hangingLedge, ClimbingOptions.Climb);
                    stateMachine.SetTrigger("ClimbingFinished");
                    this.Log($"(ClimberInteractions - {transform.root.name}) Climbing finished.", showLogs);
                }
                else
                {
                    this.Log($"(ClimberInteractions - {transform.root.name}) Climbing ended in an undefined state.", showLogs);
                }
                // Let character status time to get to new state.
                yield return new WaitUntil(() => characterStatus.CurrentState != CharacterStatus.States.Climbing);
                _climbable = null;
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
                if (!_actionPushed) _climbAborted = true;
            }
            _jumpPushed = false;
        }

        /// <summary>
        /// Used to signal climbable that action has been pushed.
        /// </summary>
        public void ActionPushed()
        {
            if (ClimbingInProgress)
            {
                _climbable.ActionPushed(true);
                _climbAborted = false;
            } 
            _actionPushed = true;
        }
        
        /// <summary>
        /// Used to signal climbable that action has been released.
        /// </summary>
        public void ActionReleased()
        {
            if (ClimbingInProgress)
            {
                _climbable.ActionPushed(false);
                _climbAborted = true;
            }
            _actionPushed = false;
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
                yield return _climbable.Descend(hangingLedge, _actionPushed);
                if (_climbable.ClimbingResult == ClimbableStatus.ClimbingResult.Descended)
                {
                    UpdateCharacterPosition(hangingLedge, ClimbingOptions.Descend);
                    stateMachine.SetTrigger("ClimbingFinished");
                    this.Log($"(ClimberInteractions - {transform.root.name}) Descend finished.", showLogs);
                } 
                else if (_climbable.ClimbingResult == ClimbableStatus.ClimbingResult.Climbed)
                {
                    this.Log($"(ClimberInteractions - {transform.root.name}) Descend aborted.", showLogs);
                    stateMachine.SetTrigger("ClimbingAborted");
                }
                else
                {
                    this.Log($"(ClimberInteractions - {transform.root.name}) Descend ended in an undefined state.", showLogs);
                }
                _climbable = null;
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