using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component emits vibrations events depending on Prince states.
    /// </summary>
    public class VibrationsController : MonoBehaviour
    {

        /// <summary>
        /// Data model for a vibration event.
        ///
        /// Those vibrations are originated by Prince character falls over the ground making falling grounds tremble.
        /// </summary>
        public class VibrationEvent : EventArgs
        {
            public Vector3 SourcePosition { get; private set; }
            
            /// <param name="position">Where vibration event was originated</param>
            public VibrationEvent(Vector3 position)
            {
                SourcePosition = position;
            }
        }

        [Header("WIRING:")] 
        [Tooltip("Needed to know current character state.")] 
        [SerializeField] private CharacterStatus characterStatus;

        private EventBus _eventBus;
        private bool _eventTriggered;

        /// <summary>
        /// Trigger vibration event both locally and through event bus.
        /// </summary>
        private void TriggerVibrationEvent()
        {
            if (!_eventTriggered)
            {
                _eventBus.TriggerEvent(new VibrationEvent(transform.root.position), this);
                _eventTriggered = true;
            }
        }
        
        private void Awake()
        {
            _eventBus = GameObject.Find("EventBus").GetComponentInChildren<EventBus>();
            _eventBus.RegisterEvent<VibrationEvent>();
        }

        private void FixedUpdate()
        {
            switch (characterStatus.CurrentState)
            {
                case CharacterStatus.States.Landing:
                case CharacterStatus.States.HardLanding:
                case CharacterStatus.States.DeadByFall:
                case CharacterStatus.States.VerticalJumpEnd:
                    TriggerVibrationEvent();
                    break;
                default:
                    _eventTriggered = false;
                    break;
            }
        }
    }
}