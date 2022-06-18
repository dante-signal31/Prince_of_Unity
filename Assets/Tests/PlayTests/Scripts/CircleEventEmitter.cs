using System;
using Prince;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Tests.PlayTests.Scripts
{
    public class CircleEventEmitter : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;

        private EventBus _eventBus;

        public Color SpriteColor => sprite.color;

        public class CircleEvent : EventArgs
        {
            public Color ActivationValue { get; set; }

            public CircleEvent(Color color)
            {
                ActivationValue = color;
            }
        }

        public event EventHandler<CircleEvent> CircleActivated;
        public UnityEvent<object, CircleEvent> uCircleActivated = new UnityEvent<object, CircleEvent>();

        /// <summary>
        /// Set circle sprite color.
        /// </summary>
        /// <param name="color">Color to paint circle of.</param>
        private void SetColor(Color color)
        {
            sprite.color = color;
        }

        /// <summary>
        /// Choose a random color, paint with it circle and emit a CircleActivated event.
        /// </summary>
        public void ActivateCircle()
        {
            SetColor(new Color(Random.value, Random.value, Random.value, Random.value));
            CircleEvent circleEventData = new CircleEvent(SpriteColor);
            // Call callbacks directly attached to local event.
            CircleActivated?.Invoke(this, new CircleEvent(SpriteColor));
            // Call callbacks registered to event bus.
            _eventBus.TriggerEvent(circleEventData, this);
            // Call callbacks registered to local UnityEvent.
            uCircleActivated.Invoke(this, circleEventData);
        }

        private void Awake()
        {
            _eventBus = GameObject.Find("EventBus").GetComponentInChildren<EventBus>();
            _eventBus.RegisterEvent<CircleEvent>();
        }
    }
}