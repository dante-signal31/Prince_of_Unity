using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component emits events that affect falling ground and make it vibrate or fall.
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
        
        private EventBus _eventBus;

        /// <summary>
        /// Trigger vibration event through event bus.
        ///
        /// This method is called from animation events.
        /// </summary>
        public void TriggerVibrationEvent()
        {
            _eventBus.TriggerEvent(new VibrationEvent(transform.root.position), this);
        }
        
        private void Awake()
        {
            _eventBus = GameObject.Find("EventBus").GetComponentInChildren<EventBus>();
            _eventBus.RegisterEvent<VibrationEvent>();
        }
        
    }
}