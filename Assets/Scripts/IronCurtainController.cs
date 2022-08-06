using System.Collections;
using UnityEngine;

namespace Prince
{
    [ExecuteAlways]
    public class IronCurtainController : CurtainController<PortcullisStatus.PortcullisStates>
    {
        [Header("WIRING (IronCurtainController):")] 
        [Tooltip("Needed to know current portcullis status.")]
        [SerializeField] private new PortcullisStatus gateStatus;
        // [Tooltip("Needed to signal state machine state changes.")]
        // [SerializeField] private Animator stateMachine;
        // [Tooltip("Needed to mode iron curtain.")]
        // [SerializeField] private Transform curtainTransform;
        [Tooltip("Needed to block way when curtain is closed.")]
        [SerializeField] private BoxCollider2D curtainCollider;
        // [Tooltip("Needed to play portcullis sounds.")]
        // [SerializeField] private SoundController soundController;


        [Header("CONFIGURATION (IronCurtainController):")] 
        [Tooltip("Needed to calculate Y offsets.")]
        [SerializeField] private float colliderInitialYPosition;
        [Tooltip("Minimum Y size for curtain collider.")]
        [SerializeField] private float colliderMinimumHeight;
        [Tooltip("Maximum Y size for curtain collider.")]
        [SerializeField] private float colliderMaximumHeight;
        
        // protected new PortcullisStatus.PortcullisStates _currentState = PortcullisStatus.PortcullisStates.Closed;
        
        // [Tooltip("Height when curtain is considered open.")]
        // [SerializeField] private float openingHeight;
        // [Tooltip("Height when curtain is considered closed.")]
        // [SerializeField] private float closingHeight;
        // [Tooltip("Duration in seconds of opening sequence.")]
        // [SerializeField] private float openingTime;
        // [Tooltip("Duration in seconds of closing sequence.")]
        // [SerializeField] private float closingTime;
        // [Tooltip("Duration in seconds of closing fast sequence.")]
        // [SerializeField] private float closingFastTime;
        // [Tooltip("Whether this door should close automatically after a time.")]
        // [SerializeField] private bool autoClose;
        // [Tooltip("Time to wait before starting closing sequence if autoClose is true.")]
        // [SerializeField] private float autoCloseWaitingTime;
        // [Tooltip("Initial opening of this gate (0 closed 1 full open).")]
        // [Range(0.0f, 1.0f)] 
        // [SerializeField] private float initialOpening;

        // private float _gateOpening;
        private PortcullisStatus.PortcullisStates _currentState = PortcullisStatus.PortcullisStates.Closed;
        // private float _openingNormalizedSpeed;
        // private float _closingNormalizedSpeed;
        // private float _closingNormalizedFastSpeed;
        // private SimpleTimer _timer = new SimpleTimer();
        
        // /// <summary>
        // /// Gate current opening. 0 closed and 1 open.
        // /// </summary>
        // public float GateOpening {
        //     get => _gateOpening;
        //     private set => _gateOpening = Mathf.Clamp(value, 0, 1);
        // }
        //
        // /// <summary>
        // /// True if gate is fully open. False if not.
        // /// </summary>
        // public bool GateOpen => Mathf.Abs(GateOpening - 1) < 0.01;
        //
        // /// <summary>
        // /// True if gate is fully closed. False if not.
        // /// </summary>
        // public bool GateClosed => Mathf.Abs(GateOpening - 0) < 0.01;

        // private void Awake()
        // {
        //     calculateSpeeds();
        // }

        // /// <summary>
        // /// Calculate normalized opening and closing speeds depending on given parameters.
        // /// </summary>
        // private void calculateSpeeds()
        // {
        //     _openingNormalizedSpeed = 1 / openingTime;
        //     _closingNormalizedSpeed = 1 / closingTime;
        //     _closingNormalizedFastSpeed = 1 / closingFastTime;
        // }

        // [Header("DEBUG:")]
        // [Tooltip("Show this component logs on console window.")]
        // [SerializeField] private bool showLogs;

        protected override IStateMachineStatus<PortcullisStatus.PortcullisStates> GateStatus => gateStatus;

        protected override PortcullisStatus.PortcullisStates CurrentState
        {
            get=> _currentState;
            set
            {
                _currentState = value;
            }
        }

        /// <summary>
        /// <p>Set current opening for iron curtain.</p>
        ///
        /// <p>Opening percentage used here goes from 0 (closed) to 1 (open). Values over 1 will be
        /// converted to 1 and values under 0 will converted to 0.</p>
        /// </summary>
        /// <param name="newOpening">Opening, from 0 (closed) to 1 (open)</param>
        protected override void setOpening(float newOpening)
        {
            // GateOpening = newOpening;
            // float newCurtaingHeight = Mathf.Lerp(closingHeight, openingHeight, GateOpening);
            // curtainTransform.localPosition = new Vector3(curtainTransform.localPosition.x,
            //     newCurtaingHeight,
            //     curtainTransform.localPosition.z);
            base.setOpening(newOpening);
            updateColliderSize();
        }

        /// <summary>
        /// Make collider longer while curtain closes to block the way.
        /// </summary>
        /// <param name="portcullisOpening">Opening, from 0 (closed) to 1 (open)</param>
        private void updateColliderSize()
        {
            // Collider size rate is the inverse of opening rate.
            float colliderSize = (1 - GateOpening);
            float newColliderHeight = Mathf.Lerp(colliderMinimumHeight, colliderMaximumHeight, colliderSize);
            curtainCollider.size = new Vector2(curtainCollider.size.x, newColliderHeight);
            curtainCollider.transform.localPosition = new Vector3(curtainCollider.transform.localPosition.x,
                colliderInitialYPosition - ((newColliderHeight - colliderMinimumHeight) / 2),
                curtainCollider.transform.localPosition.z);
        }

        // /// <summary>
        // /// Start an opening sequence.
        // /// </summary>
        // public void OpenGate()
        // {
        //     StartCoroutine(OpenGateAsync());
        // }

        /// <summary>
        /// Coroutine to open the gate.
        /// </summary>
        protected override IEnumerator OpenGateAsync()
        {
            // soundController.PlaySound("portcullis_opening");
            // _timer.StartTimeMeasure();
            // float previousTime = _timer.ElapsedTime;
            // float elapsedTime = 0;
            // while (gateStatus.CurrentState == PortcullisStatus.PortcullisStates.Opening)
            // {
            //     if (GateOpen)
            //     {
            //         break;
            //     }
            //     else
            //     {
            //         float newTime = _timer.ElapsedTime;
            //         elapsedTime = newTime - previousTime;
            //         setOpening(GateOpening + _openingNormalizedSpeed * elapsedTime);
            //         previousTime = newTime;
            //     }
            //     yield return null;
            // }
            // soundController.PlaySound("portcullis_end");
            // if (gateStatus.CurrentState == PortcullisStatus.PortcullisStates.Opening) 
            //     stateMachine.SetTrigger("OpeningEnded");
            // yield return null;
            soundController.PlaySound("portcullis_opening");
            yield return base.OpenGateAsync();
            soundController.PlaySound("portcullis_end");
        }

        // /// <summary>
        // /// Start an slow closing sequence.
        // /// </summary>
        // public void CloseGateSlowly()
        // {
        //     StartCoroutine(CloseGateSlowlyAsync());
        // }

        /// <summary>
        /// Coroutine to slow close the portcullis.
        /// </summary>
        protected override IEnumerator CloseGateSlowlyAsync()
        {
            // soundController.PlaySound("portcullis_closing_slowly");
            // _timer.StartTimeMeasure();
            // float previousTime = _timer.ElapsedTime;
            // float elapsedTime = 0;
            // while (gateStatus.CurrentState == PortcullisStatus.PortcullisStates.ClosingSlow)
            // {
            //     if (GateClosed)
            //     {
            //         break;
            //     }
            //     else
            //     {
            //         float newTime = _timer.ElapsedTime;
            //         elapsedTime = newTime - previousTime;
            //         setOpening(GateOpening - _closingNormalizedSpeed * elapsedTime);
            //         previousTime = newTime;
            //     }
            //     yield return null;
            // }
            // soundController.PlaySound("portcullis_end");
            // if (gateStatus.CurrentState == PortcullisStatus.PortcullisStates.ClosingSlow)
            //     stateMachine.SetTrigger("ClosingEnded");
            // yield return null;
            soundController.PlaySound("portcullis_closing_slowly");
            yield return base.CloseGateSlowlyAsync();
            soundController.PlaySound("portcullis_end");
        }

        // /// <summary>
        // /// Start a fast closing sequence.
        // /// </summary>
        // public void closeGateFast()
        // {
        //     StartCoroutine(CloseGateFastAsync());
        // }
        
        /// <summary>
        /// Coroutine to fast close the portcullis.
        /// </summary>
        protected override IEnumerator CloseGateFastAsync()
        {
            // _timer.StartTimeMeasure();
            // float previousTime = _timer.ElapsedTime;
            // float elapsedTime = 0;
            // while (gateStatus.CurrentState == PortcullisStatus.PortcullisStates.ClosingFast)
            // {
            //     if (GateClosed)
            //     {
            //         break;
            //     }
            //     else
            //     {
            //         float newTime = _timer.ElapsedTime;
            //         elapsedTime = newTime - previousTime;
            //         setOpening(GateOpening - _closingNormalizedFastSpeed * elapsedTime);
            //         previousTime = newTime;
            //     }
            //     yield return null;
            // }
            // soundController.PlaySound("portcullis_closing_fast");
            // if (gateStatus.CurrentState == PortcullisStatus.PortcullisStates.ClosingFast) 
            //     stateMachine.SetTrigger("ClosingEnded");
            // yield return null;
            yield return base.CloseGateFastAsync();
            soundController.PlaySound("portcullis_closing_fast");
        }

        // /// <summary>
        // /// Open portcullis at once.
        // /// </summary>
        // public void setGateOpen()
        // {
        //     setOpening(1.0f);
        //     if (autoClose) StartCoroutine(AutoClose());
        // }

        // /// <summary>
        // /// Coroutine to close the portcullis after autoclose waiting time.
        // /// </summary>
        // private IEnumerator AutoClose()
        // {
        //     yield return new WaitForSeconds(autoCloseWaitingTime);
        //     if (gateStatus.CurrentState == PortcullisStatus.PortcullisStates.Open)
        //         stateMachine.SetTrigger("Close");
        // }

        // /// <summary>
        // /// Close portcullis at once.
        // /// </summary>
        // public void SetGateClosed()
        // {
        //     setOpening(0.0f);
        // }

        protected override void ReactToNewState(PortcullisStatus.PortcullisStates newState)
        {
            switch (newState)
            {
                case PortcullisStatus.PortcullisStates.Opening:
                    OpenGate();
                    break;
                case PortcullisStatus.PortcullisStates.ClosingFast:
                    closeGateFast();
                    break;
                case PortcullisStatus.PortcullisStates.ClosingSlow:
                    CloseGateSlowly();
                    break;
                case PortcullisStatus.PortcullisStates.Open:
                    setGateOpen();
                    break;
                case PortcullisStatus.PortcullisStates.Closed:
                    SetGateClosed();
                    break;
            }
        }

        protected override void Awake()
        {
            _currentState = PortcullisStatus.PortcullisStates.Closed;
            base.Awake();
        }

        // /// <summary>
        // /// At state changes trigger iron curtain movement methods.
        // /// </summary>
        // private void ReactToStateChanges()
        // {
        //     if (_currentState != gateStatus.CurrentState)
        //     {
        //         switch (gateStatus.CurrentState)
        //         {
        //             case PortcullisStatus.PortcullisStates.Opening:
        //                 OpenGate();
        //                 break;
        //             case PortcullisStatus.PortcullisStates.ClosingFast:
        //                 closeGateFast();
        //                 break;
        //             case PortcullisStatus.PortcullisStates.ClosingSlow:
        //                 CloseGateSlowly();
        //                 break;
        //             case PortcullisStatus.PortcullisStates.Open:
        //                 setGateOpen();
        //                 break;
        //             case PortcullisStatus.PortcullisStates.Closed:
        //                 SetGateClosed();
        //                 break;
        //         }
        //
        //         _currentState = gateStatus.CurrentState;
        //     }
        // }
        

        // /// <summary>
        // /// If we are measuring time, then update counter.
        // /// </summary>
        // private void UpdateTimerCounter()
        // {
        //     _timer.UpdateElapsedTime(Time.fixedDeltaTime);
        // }

        // private void OnValidate()
        // {
        //     setOpening(initialOpening);
        // }

        public override bool IsOpening()
        {
            return gateStatus.CurrentState == PortcullisStatus.PortcullisStates.Opening;
        }

        public override bool IsClosingSlowly()
        {
            return gateStatus.CurrentState == PortcullisStatus.PortcullisStates.ClosingSlow;
        }

        public override bool IsClosingFast()
        {
            return gateStatus.CurrentState == PortcullisStatus.PortcullisStates.ClosingFast;
        }

        public override bool IsOpen()
        {
            return gateStatus.CurrentState == PortcullisStatus.PortcullisStates.Open;
        }
    }
}