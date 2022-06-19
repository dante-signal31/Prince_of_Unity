using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component subscribes to vibrations events and makes falling ground tremble if vibration is near enough.
    /// </summary>
    public class VibrationsSensor : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to send trembling signal when needed.")] 
        [SerializeField] private Animator stateMachine;

        [Header("CONFIGURATION:")] 
        [Tooltip("Trembling activation distance (should be bigger than falling activation distance).")] 
        [SerializeField] private float tremblingActivationDistance;
        [Tooltip("Falling activation distance (should be smaller than trembling activation distance).")]
        [SerializeField] private float fallingActivationDistance;

        private EventBus _eventBus;
        private bool _eventRegisteringRetryNeeded;

        private void Awake()
        {
            _eventBus = GameObject.Find("EventBus").GetComponentInChildren<EventBus>();
        }

        private void Start()
        {
            if (_eventRegisteringRetryNeeded)
            {
                _eventBus.AddListener<VibrationsController.VibrationEvent>(OnVibration);
                _eventRegisteringRetryNeeded = false;
            }
        }

        private void OnEnable()
        {
            try
            {
                _eventBus.AddListener<VibrationsController.VibrationEvent>(OnVibration);
            }
            catch (EventBus.NotExistingEvent ev)
            {
                _eventRegisteringRetryNeeded = true;
            }
        }

        private void OnDisable()
        {
            _eventBus.RemoveListener<VibrationsController.VibrationEvent>(OnVibration);
        }

        /// <summary>
        /// If vibration event takes place into activation distance then trigger a tremble signal to state machine.
        /// </summary>
        /// <param name="sender">Game object who triggered vibration event.</param>
        /// <param name="eventArgs">Event data.</param>
        private void OnVibration(object sender, VibrationsController.VibrationEvent eventArgs)
        {
            if (IsJustBelow(eventArgs.SourcePosition))
            {
                stateMachine.SetBool("Fall", true);
            }
            else if (InTremblingActivationRange(eventArgs.SourcePosition))
            {
                stateMachine.SetTrigger("Tremble");
            }
        }

        /// <summary>
        /// Whether source position is in trembling activation range.
        /// </summary>
        /// <param name="sourcePosition">Vibration source position.</param>
        /// <returns>True if it is in range or false if not.</returns>
        private bool InTremblingActivationRange(Vector3 sourcePosition)
        {
            float distance = Vector3.Distance(transform.position, sourcePosition);
            return distance <= tremblingActivationDistance;
        }

        /// <summary>
        /// <p>Whether vibration source is just below vibrations sensors.</p>
        ///
        /// <p>Actually it checks two conditions:</p>
        /// <ul>
        /// <li> Vibration source position is in falling activation range. </li>
        /// <li> Vibration source Y position is under vibration sensor Y position. </li>
        /// </ul>
        /// </summary>
        /// <param name="sourcePosition">Vibration source position.</param>
        /// <returns>True if it is just below or false if not.</returns>
        private bool IsJustBelow(Vector3 sourcePosition)
        {
            Vector3 currentPosition = transform.position;
            float distanceX = Math.Abs(currentPosition.x - sourcePosition.x);
            float distanceY = Math.Abs(currentPosition.y - sourcePosition.y);
            bool isBelow = currentPosition.y > sourcePosition.y;
            return distanceX <= fallingActivationDistance && isBelow && distanceY <= 2.0f;
        }
        
#if UNITY_EDITOR
        private void DrawActivationDistance()
        {
            Vector3 currentPosition = transform.position;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(currentPosition, tremblingActivationDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(currentPosition, fallingActivationDistance);
        }
        

        private void OnDrawGizmosSelected()
        {
            DrawActivationDistance();
        }
#endif
    }
}