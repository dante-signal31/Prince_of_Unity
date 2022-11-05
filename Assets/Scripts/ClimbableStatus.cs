using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component keeps current state from climbable state machine.
    /// </summary>
    public class ClimbableStatus : MonoBehaviour, IStateMachineStatus<ClimbableStatus.States>
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to signal state machine when climbing is abortable.")]
        [SerializeField] private Animator stateMachine;
        [Tooltip("Needed to show and hide animations.")] 
        [SerializeField] private ClimbableAnimationController animationController;
    
        public enum States
        {
            Hanging,
            Climbing,
            HangingLong,
            HangingBlocked,
            Inactive,
            Descending,
            // Hanged
        }
        
        // State before current one.
        // Let it have a default value of Inactive to avoid triggering a PrinceClimbingEnded event just at the first 
        // iteration of Update().
        public States PreviousState { get; private set; } = States.Inactive;
        
        // Let it have a default value of Inactive or falling ground may fall at initialization.
        // I guess an enum is set by default to its 0 state that in this case is Hanging.
        private States _currentState = States.Inactive;
        
        /// <summary>
        /// Current animation state.
        /// </summary>
        public States CurrentState{ 
            get => _currentState;
            set
            {
                PreviousState = _currentState;
                _currentState = value;
            } 
        }

        private bool _climbingClear;
        
        /// <summary>
        /// True if there is no obstacle blocking a potential climbing.
        /// </summary>
        public bool ClimbingClear
        {
            get => _climbingClear;
            set
            {
                _climbingClear = value;
                stateMachine.SetBool("ClimbingClear", value);
            }
        }
        
        private bool _climbingAbortable;
    
        /// <summary>
        /// Whether we can abort a climbing.
        /// </summary>
        public bool ClimbingAbortable { 
            get => _climbingAbortable;
            set
            {
                _climbingAbortable = value;
                stateMachine.SetBool("ClimbingAbortable", value);
            } 
        }
        
        /// <summary>
        /// <p>Possible results of a climbing operation:</p>
        /// <ul>
        /// <li> <b>Climbed</b>: Character climbed to a ledge grabbing point.</li>
        /// <li> <b>Descended</b>: Character descended to a descending point.</li>
        /// <li> <b>Uncertain</b>: Climbing interaction has not ended yet.</li>
        /// </ul>
        /// </summary>
        public enum ClimbingResult
        {
            Climbed,
            Descended,
            Uncertain,
        }

        /// <summary>
        /// Result of the last climbing interaction from the point of view of the climbable.
        /// </summary>
        public ClimbingResult LastClimbingResult
        {
            get
            {
                return CurrentState switch
                {
                    States.Inactive when (PreviousState == States.Climbing && !ClimbingAbortable)=> ClimbingResult.Climbed,
                    States.Inactive when (PreviousState == States.Climbing && ClimbingAbortable)=> ClimbingResult.Descended,
                    States.Inactive when (PreviousState != States.Climbing)=> ClimbingResult.Descended,
                    _ => ClimbingResult.Uncertain
                };
            }
        }
        
        /// <summary>
        /// True if no animation is being played.
        /// </summary>
        public bool Inactive => CurrentState == States.Inactive;

        private bool _lookingRightWards;
        
        /// <summary>
        /// Whether player animations this component shows are looking rightwards.
        /// </summary>
        public bool LookingRightWards
        {
            get => _lookingRightWards;
            set
            {
                if (value != _lookingRightWards)
                {
                    animationController.PlaceAnimation(value);
                    _lookingRightWards = value;
                }
            }
        }
        
        private void Start()
        {
            animationController.PlaceAnimation(LookingRightWards);
        }

        private void Update()
        {
            switch (CurrentState)
            {
                case States.Inactive:
                    if (animationController.AnimationEnabled) animationController.HideAnimation();
                    break;
                default:
                    if (!animationController.AnimationEnabled) animationController.ShowAnimation();
                    break;
            }
        }
    }
}