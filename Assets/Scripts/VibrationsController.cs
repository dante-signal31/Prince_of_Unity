using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component emit vibrations events depending on Prince states.
    /// </summary>
    public class VibrationsController : MonoBehaviour
    {

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

        public event EventHandler<VibrationEvent> Vibration;

        private EventBus _eventBus;

        /// <summary>
        /// Trigger vibration event both locally and through event bus.
        /// </summary>
        private void TriggerVibrationEvent()
        {
            VibrationEvent eventArgs = new VibrationEvent(transform.root.position);
            _eventBus.TriggerEvent(Vibration, eventArgs, this);
        }
        
        private void Awake()
        {
            _eventBus = GameObject.Find("EventBus").GetComponentInChildren<EventBus>();
            _eventBus.RegisterEvent(Vibration);
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
            }
        }
    }
}