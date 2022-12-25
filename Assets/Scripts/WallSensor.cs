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
        [Tooltip("Needed to know if we are looking rightwards and our current state.")]
        [SerializeField] private CharacterStatus characterStatus;
        
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
                // Sometimes, when falling character touches wall with his back, it activates slide falling animation
                // which looks odd. To fix it I check if touched collider is in our back and we are falling. 
                if (IamInAnyFallingState() && !ColliderInFrontOfMe(col)) return;
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

        /// <summary>
        /// Whether touched collider is in front of character. 
        /// </summary>
        /// <param name="col">Touched collider.</param>
        /// <returns>True if touched collider is in front of character. False instead.</returns>
        private bool ColliderInFrontOfMe(Collider2D col)
        {
            float colliderPositionX = col.transform.position.x;
            float currentPositionX = transform.position.x;
            return ((colliderPositionX >= currentPositionX) && characterStatus.LookingRightWards) ||
                   ((colliderPositionX < currentPositionX) && !characterStatus.LookingRightWards);
        }

        /// <summary>
        /// Whether we are in any falling state.
        /// </summary>
        /// <returns>True if we are in any falling state. False instead.</returns>
        private bool IamInAnyFallingState()
        {
            switch (characterStatus.CurrentState)
            {
                case CharacterStatus.States.Falling:
                case CharacterStatus.States.FallStart:
                    return true;
                default:
                    return false;
            }
        }
    }
}