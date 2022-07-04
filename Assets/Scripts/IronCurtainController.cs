using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using Unity.VisualScripting;

namespace Prince
{
    [ExecuteAlways]
    public class IronCurtainController : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to know current portcullis status.")]
        [SerializeField] private PortcullisStatus portcullisStatus;
        [Tooltip("Needed to signal state machine state changes.")]
        [SerializeField] private Animator stateMachine;
        [Tooltip("Needed to mode iron curtain.")]
        [SerializeField] private Transform curtainTransform;
        [Tooltip("Needed to block way when curtain is closed.")]
        [SerializeField] private BoxCollider2D curtainCollider;
        [Tooltip("Needed to play portcullis sounds.")]
        [SerializeField] private SoundController soundController;


        [Header("CONFIGURATION:")] 
        [Tooltip("Needed to calculate Y offsets.")]
        [SerializeField] private float colliderInitialYPosition;
        [Tooltip("Minimum Y size for curtain collider.")]
        [SerializeField] private float colliderMinimumHeight;
        [Tooltip("Maximum Y size for curtain collider.")]
        [SerializeField] private float colliderMaximumHeight;
        [Tooltip("Height when curtain is considered open.")]
        [SerializeField] private float openingHeight;
        [Tooltip("Height when curtain is considered closed.")]
        [SerializeField] private float closingHeight;
        [Tooltip("Duration in seconds of opening sequence.")]
        [SerializeField] private float openingTime;
        [Tooltip("Duration in seconds of closing sequence.")]
        [SerializeField] private float closingTime;
        [Tooltip("Duration in seconds of closing fast sequence.")]
        [SerializeField] private float closingFastTime;
        [Tooltip("Whether this door should close automatically after a time.")]
        [SerializeField] private bool autoClose;
        [Tooltip("Time to wait before starting closing sequence if autoClose is true.")]
        [SerializeField] private float autoCloseWaitingTime;
        [Tooltip("Initial opening of this gate (0 closed 1 full open).")]
        [Range(0.0f, 1.0f)] 
        [SerializeField] private float initialOpening;

        private float _portcullisOpening;
        private PortcullisStatus.PortcullisStates _currentState = PortcullisStatus.PortcullisStates.Closed;
        private float _openingNormalizedSpeed;
        private float _closingNormalizedSpeed;
        private float _closingNormalizedFastSpeed;
        private SimpleTimer _timer = new SimpleTimer();
        
        /// <summary>
        /// Portcullis current opening. 0 closed and 1 open.
        /// </summary>
        public float PortcullisOpening {
            get => _portcullisOpening;
            private set => _portcullisOpening = Mathf.Clamp(value, 0, 1);
        }

        /// <summary>
        /// True if portcullis is fully open. False if not.
        /// </summary>
        public bool PortcullisOpen => Mathf.Abs(PortcullisOpening - 1) < 0.01;
        
        /// <summary>
        /// True if portcullis is fully closed. False if not.
        /// </summary>
        public bool PortcullisClosed => Mathf.Abs(PortcullisOpening - 0) < 0.01;

        private void Awake()
        {
            calculateSpeeds();
        }

        /// <summary>
        /// Calculate normalized opening and closing speeds depending on given parameters.
        /// </summary>
        private void calculateSpeeds()
        {
            _openingNormalizedSpeed = 1 / openingTime;
            _closingNormalizedSpeed = 1 / closingTime;
            _closingNormalizedFastSpeed = 1 / closingFastTime;
        }

        // [Header("DEBUG:")]
        // [Tooltip("Show this component logs on console window.")]
        // [SerializeField] private bool showLogs;

        /// <summary>
        /// <p>Set current opening for iron curtain.</p>
        ///
        /// <p>Opening percentage used here goes from 0 (closed) to 1 (open). Values over 1 will be
        /// converted to 1 and values under 0 will converted to 0.</p>
        /// </summary>
        /// <param name="newOpening">Opening, from 0 (closed) to 1 (open)</param>
        private void setOpening(float newOpening)
        {
            PortcullisOpening = newOpening;
            float newCurtaingHeight = Mathf.Lerp(closingHeight, openingHeight, PortcullisOpening);
            curtainTransform.localPosition = new Vector3(curtainTransform.localPosition.x,
                newCurtaingHeight,
                curtainTransform.localPosition.z);
            updateColliderSize();
        }

        /// <summary>
        /// Make collider longer while curtain closes to block the way.
        /// </summary>
        /// <param name="portcullisOpening">Opening, from 0 (closed) to 1 (open)</param>
        private void updateColliderSize()
        {
            // Collider size rate is the inverse of opening rate.
            float colliderSize = (1 - PortcullisOpening);
            float newColliderHeight = Mathf.Lerp(colliderMinimumHeight, colliderMaximumHeight, colliderSize);
            curtainCollider.size = new Vector2(curtainCollider.size.x, newColliderHeight);
            curtainCollider.transform.localPosition = new Vector3(curtainCollider.transform.localPosition.x,
                colliderInitialYPosition - ((newColliderHeight - colliderMinimumHeight) / 2),
                curtainCollider.transform.localPosition.z);
        }

        /// <summary>
        /// Start an opening sequence.
        /// </summary>
        public void OpenPortcullis()
        {
            StartCoroutine(OpenPortCullisAsync());
        }

        private IEnumerator OpenPortCullisAsync()
        {
            soundController.PlaySound("portcullis_opening");
            _timer.StartTimeMeasure();
            float previousTime = _timer.ElapsedTime;
            float elapsedTime = 0;
            while (portcullisStatus.CurrentState == PortcullisStatus.PortcullisStates.Opening)
            {
                if (PortcullisOpen)
                {
                    break;
                }
                else
                {
                    float newTime = _timer.ElapsedTime;
                    elapsedTime = newTime - previousTime;
                    setOpening(PortcullisOpening + _openingNormalizedSpeed * elapsedTime);
                    previousTime = newTime;
                }
                yield return null;
            }
            soundController.PlaySound("portcullis_end");
            if (portcullisStatus.CurrentState == PortcullisStatus.PortcullisStates.Opening) 
                stateMachine.SetTrigger("OpeningEnded");
            yield return null;
        }

        /// <summary>
        /// Start an slow closing sequence.
        /// </summary>
        public void ClosePortcullisSlowly()
        {
            StartCoroutine(ClosePortcullisSlowlyAsync());
        }

        private IEnumerator ClosePortcullisSlowlyAsync()
        {
            soundController.PlaySound("portcullis_closing_slowly");
            _timer.StartTimeMeasure();
            float previousTime = _timer.ElapsedTime;
            float elapsedTime = 0;
            while (portcullisStatus.CurrentState == PortcullisStatus.PortcullisStates.ClosingSlow)
            {
                if (PortcullisClosed)
                {
                    break;
                }
                else
                {
                    float newTime = _timer.ElapsedTime;
                    elapsedTime = newTime - previousTime;
                    setOpening(PortcullisOpening - _closingNormalizedSpeed * elapsedTime);
                    previousTime = newTime;
                }
                yield return null;
            }
            soundController.PlaySound("portcullis_end");
            if (portcullisStatus.CurrentState == PortcullisStatus.PortcullisStates.ClosingSlow)
                stateMachine.SetTrigger("ClosingEnded");
            yield return null;
        }

        /// <summary>
        /// Start a fast closing sequence.
        /// </summary>
        public void closePortcullisFast()
        {
            StartCoroutine(ClosePortcullisFastAsync());
        }
        
        private IEnumerator ClosePortcullisFastAsync()
        {
            _timer.StartTimeMeasure();
            float previousTime = _timer.ElapsedTime;
            float elapsedTime = 0;
            while (portcullisStatus.CurrentState == PortcullisStatus.PortcullisStates.ClosingFast)
            {
                if (PortcullisClosed)
                {
                    break;
                }
                else
                {
                    float newTime = _timer.ElapsedTime;
                    elapsedTime = newTime - previousTime;
                    setOpening(PortcullisOpening - _closingNormalizedFastSpeed * elapsedTime);
                    previousTime = newTime;
                }
                yield return null;
            }
            soundController.PlaySound("portcullis_closing_fast");
            if (portcullisStatus.CurrentState == PortcullisStatus.PortcullisStates.ClosingFast) 
                stateMachine.SetTrigger("ClosingEnded");
            yield return null;
        }

        /// <summary>
        /// Open portcullis at once.
        /// </summary>
        public void setPortcullisOpen()
        {
            setOpening(1.0f);
        }

        /// <summary>
        /// Close portcullis at once.
        /// </summary>
        public void setPortcullisClosed()
        {
            setOpening(0.0f);
        }



        private void FixedUpdate()
        {
            UpdateTimerCounter();
            ReactToStateChanges();
        }

        /// <summary>
        /// At state changes trigger iron curtain movement methods.
        /// </summary>
        private void ReactToStateChanges()
        {
            if (_currentState != portcullisStatus.CurrentState)
            {
                switch (portcullisStatus.CurrentState)
                {
                    case PortcullisStatus.PortcullisStates.Opening:
                        OpenPortcullis();
                        break;
                    case PortcullisStatus.PortcullisStates.ClosingFast:
                        closePortcullisFast();
                        break;
                    case PortcullisStatus.PortcullisStates.ClosingSlow:
                        ClosePortcullisSlowly();
                        break;
                    case PortcullisStatus.PortcullisStates.Open:
                        setPortcullisOpen();
                        break;
                    case PortcullisStatus.PortcullisStates.Closed:
                        setPortcullisClosed();
                        break;
                }

                _currentState = portcullisStatus.CurrentState;
            }
        }

        /// <summary>
        /// If we are measuring time, then update counter.
        /// </summary>
        private void UpdateTimerCounter()
        {
            _timer.UpdateElapsedTime(Time.fixedDeltaTime);
        }

        private void OnValidate()
        {
            setOpening(initialOpening);
        }
        
    }
}