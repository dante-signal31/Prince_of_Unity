using System;
using Prince;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tests.PlayTests.Scripts
{
    public class CircleEventReceiver : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;

        private EventBus _eventBus;
        private bool _eventRegisteringRetryNeeded;

        public Color SpriteColor => sprite.color;
        public float RandomValue { get; private set; }
        
        /// <summary>
        /// Set circle sprite color.
        /// </summary>
        /// <param name="color">Color to paint circle of.</param>
        protected void SetColor(Color color)
        {
            sprite.color = color;
        }

        /// <summary>
        /// Set current random value.
        /// </summary>
        /// <param name="newValue">New value to set.</param>
        public void SetRandomValue(float newValue)
        {
            RandomValue = newValue;
        }

        /// <summary>
        /// Callback for circle activated event.
        ///
        /// Paint circle of the same color than emitter.
        /// </summary>
        /// <param name="sender">Event emitter.</param>
        /// <param name="color">Color chosen by emitter.</param>
        public virtual void OnCircleActivated(object sender, CircleEventEmitter.CircleEventArgs args)
        {
            SetColor(args.ActivationValue);
            Debug.Log("I'm receiver1, and I've been activated by circle.");
        }

        /// <summary>
        /// Callback for box activated event.
        ///
        /// Set random value equal to the one at emitter.
        /// </summary>
        /// <param name="sender">Event emitter.</param>
        /// <param name="args">Color chosen by emitter.</param>
        public virtual void OnBoxActivated(object sender, BoxEventEmitter.BoxEventArgs args)
        {
            SetRandomValue(args.Value);
            Debug.Log("I'm receiver1, and I've been activated by box.");
        }


        private void Awake()
        {
            _eventBus = GameObject.Find("EventBus").GetComponentInChildren<EventBus>();
            SetRandomValue(Random.value);
        }

        private void OnEnable()
        {
            // This OnEnable should act if game object is disabled, and later enabled again, while in game. But
            // if game is just starting this registering in OnEnable may fail because event may have not been
            // registered yet at event bus. So, in this specific case we retry registering again later at Start()
            // when event should have been already registered.
            try
            {
                RegisterCircleListener();
                RegisterBoxListener();
            }
            catch (EventBus.NotExistingEvent ev)
            {
                _eventRegisteringRetryNeeded = true;
            }
            
        }

        private void OnDisable()
        {
            // OnDisable may be called while destroying game object. In that case is important remove this game objects
            // listener from event bus to remove any reference from event bus to destroyed game object and let
            // garbage collector dispose this game object.
            UnRegisterCircleListener();
            UnRegisterBoxListener();
        }

        private void Start()
        {
            if (_eventRegisteringRetryNeeded)
            {
                RegisterCircleListener();
                RegisterBoxListener();
                _eventRegisteringRetryNeeded = false;
            }
        }


        /// <summary>
        /// Add circle listener to event bus.
        /// </summary>
        private void RegisterCircleListener()
        {
            _eventBus.AddListener<CircleEventEmitter.CircleEventArgs>(OnCircleActivated);
        }

        /// <summary>
        /// Remove circle listener from event bus.
        /// </summary>
        private void UnRegisterCircleListener()
        {
            _eventBus.RemoveListener<CircleEventEmitter.CircleEventArgs>(OnCircleActivated);
        }
        
        /// <summary>
        /// Add box listener to event bus.
        /// </summary>
        private void RegisterBoxListener()
        {
            _eventBus.AddListener<BoxEventEmitter.BoxEventArgs>(OnBoxActivated);
        }

        /// <summary>
        /// Remove box listener from event bus.
        /// </summary>
        private void UnRegisterBoxListener()
        {
            _eventBus.RemoveListener<BoxEventEmitter.BoxEventArgs>(OnBoxActivated);
        }
        
    }
}