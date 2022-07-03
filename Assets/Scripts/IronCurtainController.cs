using System;
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
        [Tooltip("Needed to mode iron curtain.")]
        [SerializeField] private Transform curtainTransform;
        [Tooltip("Needed to block way when curtain is closed.")]
        [SerializeField] private BoxCollider2D curtainCollider;


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
        [SerializeField] private float closingFastSequence;
        [Tooltip("Whether this door should close automatically after a time.")]
        [SerializeField] private bool autoClose;
        [Tooltip("Time to wait before starting closing sequence if autoClose is true.")]
        [SerializeField] private float autoCloseWaitingTime;
        [Tooltip("Initial opening of this gate (0 closed 1 full open).")]
        [Range(0.0f, 1.0f)] 
        [SerializeField] private float initialOpening;

        private float _portcullisOpening;
        private PortcullisStatus.PortcullisStates _currentState = PortcullisStatus.PortcullisStates.Closed;
        
        /// <summary>
        /// Portcullis current opening. 0 closed and 1 open.
        /// </summary>
        public float PortcullisOpening {
            get => _portcullisOpening;
            private set => _portcullisOpening = Mathf.Clamp(value, 0, 1);
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
        public void openPortcullis()
        {
            
        }

        /// <summary>
        /// Start an slow closing sequence.
        /// </summary>
        public void closePortcullisSlowly()
        {
            
        }

        /// <summary>
        /// Start a fast closing sequence.
        /// </summary>
        public void closePortcullisFast()
        {
            
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
            if (_currentState != portcullisStatus.CurrentState)
            {
                switch (portcullisStatus.CurrentState)
                {
                    case PortcullisStatus.PortcullisStates.Opening:
                        openPortcullis();
                        break;
                    case PortcullisStatus.PortcullisStates.ClosingFast:
                        closePortcullisFast();
                        break;
                    case PortcullisStatus.PortcullisStates.ClosingSlow:
                        closePortcullisSlowly();
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

        private void OnValidate()
        {
            setOpening(initialOpening);
        }
        
    }
}