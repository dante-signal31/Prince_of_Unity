using System;
using Prince;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tests.PlayTests.Scripts
{
    public class BoxEventEmitter : MonoBehaviour
    {
        private EventBus _eventBus;

        public float RandomValue { get; private set; }
        
        public class BoxEventArgs : EventArgs
        {
            public float Value { get; set; }

            public BoxEventArgs(float value)
            {
                Value = value;
            }
        }
        
        public event EventHandler<BoxEventArgs> BoxActivated;

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
            BoxEventArgs args = new BoxEventArgs(RandomValue);
            BoxActivated?.Invoke(this, args);
            _eventBus.TriggerEvent(BoxActivated, args, this);
        }

        private void Awake()
        {
            _eventBus = GameObject.Find("EventBus").GetComponentInChildren<EventBus>();
            _eventBus.RegisterEvent(BoxActivated);
            SetValue(Random.value);
        }
    }
}