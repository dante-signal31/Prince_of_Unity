using UnityEngine;

namespace Tests.PlayTests.Scripts
{
    /// <summary>
    /// Hexagon event receiver directly attaches through code to local events.
    ///
    /// It attaches to two kinds of local events:
    /// * Circle events: C# classic events.
    /// * Box events: UnityEvents (but attached through code and not using editor).
    /// </summary>
    public class HexagonEventReceiver : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;

        private CircleEventEmitter _circleEmitter;
        private BoxEventEmitter _boxEmitter;
        
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
        public virtual void OnCircleActivated(object sender, CircleEventEmitter.CircleEvent args)
        {
            SetColor(args.ActivationValue);
            Debug.Log("I'm receiver4, and I've been activated by circle.");
        }
        
        /// <summary>
        /// Callback for box activated event.
        ///
        /// Set random value equal to the one at emitter.
        /// </summary>
        /// <param name="sender">Event emitter.</param>
        /// <param name="args">Color chosen by emitter.</param>
        public virtual void OnBoxActivated(object sender, BoxEventEmitter.BoxEvent args)
        {
            SetRandomValue(args.Value);
            Debug.Log("I'm receiver4, and I've been activated by box.");
        }
        
        private void Awake()
        {
            _circleEmitter = GameObject.Find("CircleEmitter").GetComponentInChildren<CircleEventEmitter>();
            _boxEmitter = GameObject.Find("BoxEmitter").GetComponentInChildren<BoxEventEmitter>();
        }

        private void OnEnable()
        {
            _circleEmitter.CircleActivated += OnCircleActivated;
            _boxEmitter.uBoxActivated.AddListener(OnBoxActivated);
        }
        
        private void OnDisable()
        {
            _circleEmitter.CircleActivated -= OnCircleActivated;
            _boxEmitter.uBoxActivated.RemoveListener(OnBoxActivated);
        }
    }
}