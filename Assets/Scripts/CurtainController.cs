using System;
using System.Collections;
using UnityEngine;

namespace Prince
{
    [ExecuteAlways]
    public abstract class CurtainController<T>: MonoBehaviour
    {
        [Header("WIRING (CurtainController):")]
        [Tooltip("Needed to signal state machine state changes.")]
        [SerializeField] protected Animator stateMachine;
        [Tooltip("Needed to mode curtain.")]
        [SerializeField] protected Transform curtainTransform;
        [Tooltip("Needed to play gate sounds.")]
        [SerializeField] protected SoundController soundController;
        
        [Header("CONFIGURATION (CurtainController):")]
        [Tooltip("Height when curtain is considered open.")]
        [SerializeField] protected float openingHeight;
        [Tooltip("Height when curtain is considered closed.")]
        [SerializeField] protected float closingHeight;
        [Tooltip("Duration in seconds of opening sequence.")]
        [SerializeField] protected float openingTime;
        [Tooltip("Duration in seconds of closing sequence.")]
        [SerializeField] protected float closingTime;
        [Tooltip("Duration in seconds of closing fast sequence.")]
        [SerializeField] protected float closingFastTime;
        [Tooltip("Whether this door should close automatically after a time.")]
        [SerializeField] protected bool autoClose;
        [Tooltip("Time to wait before starting closing sequence if autoClose is true.")]
        [SerializeField] protected float autoCloseWaitingTime;
        [Tooltip("Initial opening of this gate (0 closed 1 full open).")]
        [Range(0.0f, 1.0f)] 
        [SerializeField] protected float initialOpening;

        /// <summary>
        /// Child class should map this property to its specific status component.
        /// </summary>
        protected abstract IStateMachineStatus<T> GateStatus { get; }
        
        /// <summary>
        /// Child class should map this property to its specific _currentState component.
        /// </summary>
        protected abstract T CurrentState { get; set; }
        
        protected float _gateOpening;
        protected float _openingNormalizedSpeed;
        protected float _closingNormalizedSpeed;
        protected float _closingNormalizedFastSpeed;
        protected SimpleTimer _timer = new SimpleTimer();
        
        /// <summary>
        /// Gate current opening. 0 closed and 1 open.
        /// </summary>
        public float GateOpening {
            get => _gateOpening;
            protected set => _gateOpening = Mathf.Clamp(value, 0, 1);
        }

        /// <summary>
        /// True if gate is fully open. False if not.
        /// </summary>
        public bool GateOpen => Mathf.Abs(GateOpening - 1) < 0.01;
        
        /// <summary>
        /// True if gate is fully closed. False if not.
        /// </summary>
        public bool GateClosed => Mathf.Abs(GateOpening - 0) < 0.01;
        
        /// <summary>
        /// Calculate normalized opening and closing speeds depending on given parameters.
        /// </summary>
        protected void calculateSpeeds()
        {
            _openingNormalizedSpeed = 1 / openingTime;
            _closingNormalizedSpeed = 1 / closingTime;
            _closingNormalizedFastSpeed = 1 / closingFastTime;
        }
        
        /// <summary>
        /// <p>Set current opening for curtain.</p>
        ///
        /// <p>Opening percentage used here goes from 0 (closed) to 1 (open). Values over 1 will be
        /// converted to 1 and values under 0 will converted to 0.</p>
        /// </summary>
        /// <param name="newOpening">Opening, from 0 (closed) to 1 (open)</param>
        protected virtual void setOpening(float newOpening)
        {
            GateOpening = newOpening;
            float newCurtaingHeight = Mathf.Lerp(closingHeight, openingHeight, GateOpening);
            curtainTransform.localPosition = new Vector3(curtainTransform.localPosition.x,
                newCurtaingHeight,
                curtainTransform.localPosition.z);
        }
        
        /// <summary>
        /// Start an opening sequence.
        /// </summary>
        public void OpenGate()
        {
            StartCoroutine(OpenGateAsync());
        }
                
        /// <summary>
        /// Coroutine to open the gate.
        /// </summary>
        protected virtual IEnumerator OpenGateAsync()
        {
            _timer.StartTimeMeasure();
            float previousTime = _timer.ElapsedTime;
            float elapsedTime = 0;
            while (IsOpening())
            {
                if (GateOpen)
                {
                    break;
                }
                else
                {
                    float newTime = _timer.ElapsedTime;
                    elapsedTime = newTime - previousTime;
                    setOpening(GateOpening + _openingNormalizedSpeed * elapsedTime);
                    previousTime = newTime;
                }
                yield return null;
            }
            if (IsOpening()) 
                stateMachine.SetTrigger("OpeningEnded");
            yield return null;
        }
        
        /// <summary>
        /// Start an slow closing sequence.
        /// </summary>
        public void CloseGateSlowly()
        {
            StartCoroutine(CloseGateSlowlyAsync());
        }
        
        /// <summary>
        /// Coroutine to slow close the portcullis.
        /// </summary>
        protected virtual IEnumerator CloseGateSlowlyAsync()
        {
            _timer.StartTimeMeasure();
            float previousTime = _timer.ElapsedTime;
            float elapsedTime = 0;
            while (IsClosingSlowly())
            {
                if (GateClosed)
                {
                    break;
                }
                else
                {
                    float newTime = _timer.ElapsedTime;
                    elapsedTime = newTime - previousTime;
                    setOpening(GateOpening - _closingNormalizedSpeed * elapsedTime);
                    previousTime = newTime;
                }
                yield return null;
            }
            if (IsClosingSlowly())
                stateMachine.SetTrigger("ClosingEnded");
            yield return null;
        }
        
        /// <summary>
        /// Start a fast closing sequence.
        /// </summary>
        public void closeGateFast()
        {
            StartCoroutine(CloseGateFastAsync());
        }
        
        /// <summary>
        /// Coroutine to fast close the portcullis.
        /// </summary>
        protected virtual IEnumerator CloseGateFastAsync()
        {
            _timer.StartTimeMeasure();
            float previousTime = _timer.ElapsedTime;
            float elapsedTime = 0;
            while (IsClosingFast())
            {
                if (GateClosed)
                {
                    break;
                }
                else
                {
                    float newTime = _timer.ElapsedTime;
                    elapsedTime = newTime - previousTime;
                    setOpening(GateOpening - _closingNormalizedFastSpeed * elapsedTime);
                    previousTime = newTime;
                }
                yield return null;
            }
            if (IsClosingFast()) 
                stateMachine.SetTrigger("ClosingEnded");
            yield return null;
        }
        
        /// <summary>
        /// Open portcullis at once.
        /// </summary>
        public void setGateOpen()
        {
            setOpening(1.0f);
            if (autoClose) StartCoroutine(AutoClose());
        }
        
        /// <summary>
        /// Coroutine to close the portcullis after autoclose waiting time.
        /// </summary>
        private IEnumerator AutoClose()
        {
            yield return new WaitForSeconds(autoCloseWaitingTime);
            if (IsOpen())
                stateMachine.SetTrigger("Close");
        }
        
        /// <summary>
        /// Close portcullis at once.
        /// </summary>
        public void SetGateClosed()
        {
            setOpening(0.0f);
        }
        
        /// <summary>
        /// If we are measuring time, then update counter.
        /// </summary>
        private void UpdateTimerCounter()
        {
            _timer.UpdateElapsedTime(Time.fixedDeltaTime);
        }
        
        /// <summary>
        /// At state changes trigger iron curtain movement methods.
        /// </summary>
        private void ReactToStateChanges()
        {
            if (StateHasChanged())
            {
                CurrentState = GateStatus.CurrentState;
                ReactToNewState(GateStatus.CurrentState);
            }
        }

        /// <summary>
        /// Whether state has changed compared with what we had in CurrentState.
        /// </summary>
        /// <returns>True is state has just changed. False otherwise.</returns>
        private bool StateHasChanged()
        {
            return !CurrentState.Equals(GateStatus.CurrentState);
        }

        /// <summary>
        /// Take and action depending on new state.
        /// </summary>
        /// <param name="newState">New state.</param>
        protected abstract void ReactToNewState(T newState);
        
        protected virtual void Awake()
        {
            calculateSpeeds();
        }

        protected virtual void FixedUpdate()
        {
            UpdateTimerCounter();
            ReactToStateChanges();
        }
        
        private void OnValidate()
        {
            setOpening(initialOpening);
        }


        /// <summary>
        /// Whether this gate is still in its opening sequence.
        /// </summary>
        /// <returns>True if this gate is still opening: false otherwise.</returns>
        public abstract bool IsOpening();

        /// <summary>
        /// Whether this gate is still in its closing slowly sequence.
        /// </summary>
        /// <returns>True if this gate is still closing: false otherwise.</returns>
        public abstract bool IsClosingSlowly();

        /// <summary>
        /// Whether this gate is still in its closing fast sequence.
        /// </summary>
        /// <returns>True if this gate is still closing: false otherwise.</returns>
        public abstract bool IsClosingFast();
        
        /// <summary>
        /// Whether this gate is already open.
        /// </summary>
        /// <returns>True if this gate is open: false otherwise.</returns>
        public abstract bool IsOpen();
    }
}