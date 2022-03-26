using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component controls falling duration.
    ///
    /// This component detects when falling is so long that hurts or even kill the character.
    /// </summary>
    public class FallingController : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to detect falling.")]
        [SerializeField] private CharacterStatus characterStatus;
        [Tooltip("Needed to signal to state machine that hurt or death fall distance has been reached.")]
        [SerializeField] private Animator stateMachine;
        [Tooltip("Needed to play scream sound.")]
        [SerializeField] private SoundController soundController;

        [Header("CONFIGURATION:")]
        [Tooltip("Maximum fall height to not be hurt.")]
        [SerializeField] private float safeHeight;
        [Tooltip("Maximum height to be just hurt.")] 
        [SerializeField] private float hurtHeight;
        [Tooltip("Fall height before screaming.")]
        [SerializeField] private float screamFallHeight;
        
        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;

        /// <summary>
        /// Whether this character is falling or not.
        /// </summary>
        private bool IsFalling => characterStatus.IsFalling;
        /// <summary>
        /// Current character height.
        /// </summary>
        private float CurrentHeight => transform.position.y;

        private enum LandingStates
        {
            Normal, // No hurt expected.
            Hard, // Hurt expected.
            Deadly // This character is going to die as soon it touches ground.
        }

        private bool _alreadyScreamed;
        private bool _previouslyWasFalling;
        private float _previousHeight;
        private float _totalFallingHeight;
        private LandingStates _expectedLanding;

        /// <summary>
        /// On get return expected landing state so far. On set it signal expected landing
        /// state to state machine if change in current value has happened.
        /// </summary>
        private LandingStates ExpectedLanding
        {
            get => _expectedLanding;
            set
            {
                if (_expectedLanding != value)
                {
                    switch (value)
                    {
                        case LandingStates.Normal:
                            SignalNormalLandingState();
                            break;
                        case LandingStates.Hard:
                            SignalHardLandingState();
                            break;
                        case LandingStates.Deadly:
                            SignalDeadlyLandingState();
                            break;
                    }
                }
                _expectedLanding = value;
            }
        }

        /// <summary>
        /// Signal to state machine that a normal landing is expected at the time being.
        /// </summary>
        private void SignalNormalLandingState()
        {
            stateMachine.SetBool("hardLandingExpected", false);
            stateMachine.SetBool("deadlyLandingExpected", false);
            this.Log($"(FallingController - {transform.root.name}) So far, a normal landing is expected.", showLogs);
        }
        
        /// <summary>
        /// Signal to state machine that a hard landing is expected at the time being.
        /// </summary>
        private void SignalHardLandingState()
        {
            stateMachine.SetBool("hardLandingExpected", true);
            stateMachine.SetBool("deadlyLandingExpected", false);
            this.Log($"(FallingController - {transform.root.name}) Danger, a hard landing is expected.", showLogs);
        }
        
        /// <summary>
        /// Signal to state machine that a deadly landing is expected at the time being.
        /// </summary>
        private void SignalDeadlyLandingState()
        {
            stateMachine.SetBool("hardLandingExpected", false);
            stateMachine.SetBool("deadlyLandingExpected", true);
            this.Log($"(FallingController - {transform.root.name}) No hope, a deadly landing is expected.", showLogs);
        }
        
        private void Awake()
        {
            _previouslyWasFalling = false;
            ExpectedLanding = LandingStates.Normal;
            _alreadyScreamed = false;
        }
        

        private void FixedUpdate()
        {
            UpdateFallingCounter();
            UpdateExpectedLandingState();
            ScreamIfNeeded();
        }

        /// <summary>
        /// Scream if we are reached scream height and we are not screaming yet.
        /// </summary>
        private void ScreamIfNeeded()
        {
            if (IsFalling && ExpectedLanding == LandingStates.Deadly)
            {
                if (_totalFallingHeight >= screamFallHeight && !_alreadyScreamed)
                {
                    _alreadyScreamed = true;
                    soundController.PlaySound("falling_scream");
                    this.Log($"(FallingController - {transform.root.name}) Screaming.", showLogs);
                }
            }
        }

        /// <summary>
        /// Update expected landing state depending on _totalFallingHeight if we are falling.
        /// </summary>
        private void UpdateExpectedLandingState()
        {
            if (IsFalling)
            {
                if (_totalFallingHeight <= safeHeight)
                {
                    ExpectedLanding = LandingStates.Normal;
                }
                else if (_totalFallingHeight <= hurtHeight)
                {
                    ExpectedLanding = LandingStates.Hard;
                }
                else if (_totalFallingHeight > hurtHeight)
                {
                    ExpectedLanding = LandingStates.Deadly;
                }
            }
        }

        /// <summary>
        /// If character is falling update _totalFallingHeight.
        /// </summary>
        private void UpdateFallingCounter()
        {
            if (IsFalling)
            {
                if (!_previouslyWasFalling)
                {
                    _previouslyWasFalling = true;
                    _totalFallingHeight = 0;
                }

                _totalFallingHeight += _previousHeight - CurrentHeight;
            }
            else
            {
                if (_previouslyWasFalling)
                {
                    _previouslyWasFalling = false;
                }
            }
            _previousHeight = CurrentHeight;
        }
    }
}