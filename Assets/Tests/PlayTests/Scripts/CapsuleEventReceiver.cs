using UnityEngine;

namespace Tests.PlayTests.Scripts
{
    /// <summary>
    /// Capsule event receiver attaches to local UnityEvents using editor.
    /// </summary>
    public class CapsuleEventReceiver : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;
        
        private CircleEventEmitter _circleEmitter;
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
            Debug.Log("I'm receiver3, and I've been activated by circle.");
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
            Debug.Log("I'm receiver3, and I've been activated by box.");
        }
        
        // private void Awake()
        // {
        //     _circleEmitter = GameObject.Find("CircleEmitter").GetComponentInChildren<CircleEventEmitter>();
        // }
        
        // private void OnEnable()
        // {
        //     _circleEmitter.uCircleActivated.AddListener(OnCircleActivated);
        // }
        //
        // private void OnDisable()
        // {
        //     _circleEmitter.uCircleActivated.RemoveListener(OnCircleActivated);
        // }
        
    }
}