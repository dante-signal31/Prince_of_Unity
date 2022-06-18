using System;
using Prince;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Tests.PlayTests.Scripts
{
    public class BoxEventEmitter : MonoBehaviour
    {
        private EventBus _eventBus;

        public float RandomValue { get; private set; }
        
        public class BoxEvent : EventArgs
        {
            public float Value { get; set; }

            public BoxEvent(float value)
            {
                Value = value;
            }
        }
        
        public event EventHandler<BoxEvent> BoxActivated;
        public UnityEvent<object, BoxEvent> uBoxActivated = new UnityEvent<object, BoxEvent>();

        /// <summary>
        /// Set current value.
        /// </summary>
        /// <param name="value">New current value.</param>
        private void SetValue(float value)
        {
            RandomValue = value;
        }

        /// <summary>
        /// Send current random value to every listener.
        /// </summary>
        public void ActivateBox()
        {
            BoxEvent boxEventData = new BoxEvent(RandomValue);
            // Call callbacks directly attached to local event.
            BoxActivated?.Invoke(this, boxEventData);
            // Call callbacks registered to event bus.
            _eventBus.TriggerEvent(boxEventData, this);
            // Call callbacks registered to local UnityEvent.
            uBoxActivated.Invoke(this, boxEventData);
        }

        private void Awake()
        {
            _eventBus = GameObject.Find("EventBus").GetComponentInChildren<EventBus>();
            _eventBus.RegisterEvent<BoxEvent>();
            SetValue(Random.value);
        }
    }
}