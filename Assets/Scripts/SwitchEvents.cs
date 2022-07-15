using System;
using UnityEngine;
using UnityEngine.Events;

namespace Prince
{
    /// <summary>
    /// Component to activate things from this switch.
    /// </summary>
    public class SwitchEvents : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to know when this switch is activated.")]
        [SerializeField] private SwitchStatus switchStatus;
        
        [Header("CONFIGURATION:")]
        [Tooltip("Event launched by this brick when it is activated.")]
        [SerializeField] private UnityEvent activated;

        private bool _eventAlreadyTriggered;
        private void Update()
        {
            if (switchStatus.CurrentState == SwitchStatus.States.Activated && !_eventAlreadyTriggered)
            {
                if (activated != null) activated.Invoke();
                _eventAlreadyTriggered = true;
            } 
            else if (switchStatus.CurrentState == SwitchStatus.States.Idle && _eventAlreadyTriggered)
            {
                _eventAlreadyTriggered = false;
            }
        }
    }
}