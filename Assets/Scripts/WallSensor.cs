using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component detects when Prince touches a wall.
    ///
    /// It is used while falling to signal Animator to pass to SlidingFall state.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class WallSensor : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to signal we are touching a wall.")]
        [SerializeField] private Animator stateMachine;
        
        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;

        /// <summary>
        /// Whether this sensor is touching an architecture brick.
        /// </summary>
        public bool isTouchingWall { get; private set; }
        
        private int _architectureLayer;

        private void Awake()
        {
            _architectureLayer = LayerMask.NameToLayer("Architecture");
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == _architectureLayer)
            {
                stateMachine.SetBool("TouchingWall", true);
                isTouchingWall = true;
                this.Log($"(WallSensor - {transform.root.name}) Touching a wall.", showLogs);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == _architectureLayer)
            {
                stateMachine.SetBool("TouchingWall", false);
                isTouchingWall = false;
                this.Log($"(WallSensor - {transform.root.name}) No longer touching a wall.", showLogs);
            }
        }
    }
}