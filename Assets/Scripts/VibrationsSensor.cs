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
        [Tooltip("Activation distance.")] 
        [SerializeField] private float activationDistance;

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
            float distance = Vector3.Distance(transform.position, eventArgs.SourcePosition);
            if (distance <= activationDistance)
            {
                stateMachine.SetTrigger("Tremble");
            }
        }
        
#if UNITY_EDITOR
        private void DrawActivationDistance(){
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, activationDistance);
        }
        

        private void OnDrawGizmosSelected()
        {
            DrawActivationDistance();
        }
#endif
    }
}