using System;
using Prince;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tests.PlayTests.Scripts
{
    public class CircleEventEmitter : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;

        private EventBus _eventBus;

        public Color SpriteColor => sprite.color;

        public class CircleEventArgs : EventArgs
        {
            public Color ActivationValue { get; set; }

            public CircleEventArgs(Color color)
            {
                ActivationValue = color;
            }
        }

        public event EventHandler<CircleEventArgs> CircleActivated;

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
            CircleEventArgs args = new CircleEventArgs(SpriteColor);
            CircleActivated?.Invoke(this, new CircleEventArgs(SpriteColor));
            _eventBus.TriggerEvent(CircleActivated, args, this);
        }

        private void Awake()
        {
            _eventBus = GameObject.Find("EventBus").GetComponentInChildren<EventBus>();
            _eventBus.RegisterEvent<CircleEventArgs>(CircleActivated);
        }
    }
}