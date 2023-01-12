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

        private bool _isTouchingWall;
        /// <summary>
        /// Whether this sensor is touching an architecture brick.
        /// </summary>
        public bool IsTouchingWall { 
            get=>_isTouchingWall;
            private set
            {
                _isTouchingWall = value;
                stateMachine.SetBool("TouchingWall", value);
            }
        }

        private bool _isTouchingBackWall;
        /// <summary>
        /// Whether this sensor is touching an architecture brick with its back rays.
        /// </summary>
        public bool IsTouchingBackWall
        {
            get=>_isTouchingBackWall;
            private set
            {
                _isTouchingBackWall = value;
                stateMachine.SetBool("TouchingBackWall", value);
            }
        }
        
        private bool _isTouchingFrontWall;
        /// <summary>
        /// Whether this sensor is touching an architecture brick with its front rays.
        /// </summary>
        public bool IsTouchingFrontWall
        {
            get=>_isTouchingFrontWall;
            private set
            {
                _isTouchingFrontWall = value;
                stateMachine.SetBool("TouchingFrontWall", value);
            }
        }
        
        // private int _architectureLayer;
        // private CapsuleCollider2D _sensorCollider;
        //
        // private void Awake()
        // {
        //     _architectureLayer = LayerMask.NameToLayer("Architecture");
        //     _sensorCollider = GetComponent<CapsuleCollider2D>();
        // }

        public void OnFrontWallDetected()
        {
            IsTouchingFrontWall = true;
            IsTouchingWall = true;
            this.Log($"(WallSensor - {transform.root.name}) Touching a wall at the front.", showLogs);
        }

        public void OnNoFrontWallDetected()
        {
            IsTouchingFrontWall = false;
            IsTouchingWall = IsTouchingFrontWall || IsTouchingBackWall;
            this.Log($"(WallSensor - {transform.root.name}) No longer touching a wall at the front.", showLogs);
        }

        public void OnBackWallDetected()
        {
            IsTouchingBackWall = true;
            IsTouchingWall = true;
            this.Log($"(WallSensor - {transform.root.name}) Touching a wall at the back.", showLogs);
        }

        public void OnNoBackWallDetected()
        {
            IsTouchingBackWall = false;
            IsTouchingWall = IsTouchingFrontWall || IsTouchingBackWall;
            this.Log($"(WallSensor - {transform.root.name}) No longer touching a wall at the back.", showLogs);
        }

        // private void OnTriggerEnter2D(Collider2D col)
        // {
        //     if (col.gameObject.layer == _architectureLayer)
        //     {
        //         // Sometimes, when falling character touches wall with his back, it activates slide falling animation
        //         // which looks odd. To fix it I check if touched collider is in our back and we are falling. 
        //         if (IamInAnyFallingState() && !ColliderInFrontOfMe(col)) return;
        //         IsTouchingWall = true;
        //         this.Log($"(WallSensor - {transform.root.name}) Touching a wall.", showLogs);
        //     }
        // }
        //
        // private void OnTriggerExit2D(Collider2D other)
        // {
        //     if (other.gameObject.layer == _architectureLayer)
        //     {
        //         IsTouchingWall = false;
        //         this.Log($"(WallSensor - {transform.root.name}) No longer touching a wall.", showLogs);
        //     }
        // }
        //
        // /// <summary>
        // /// Whether touched collider is in front of character. 
        // /// </summary>
        // /// <param name="col">Touched collider.</param>
        // /// <returns>True if touched collider is in front of character. False instead.</returns>
        // private bool ColliderInFrontOfMe(Collider2D col)
        // {
        //     // float colliderPositionX = col.transform.position.x;
        //     Vector2 sensorCenter = GetSensorCenter();
        //     float currentPositionX = sensorCenter.x;
        //     float colliderPositionX = GetColliderCenter(col).x;
        //     // float colliderPositionX = col.ClosestPoint(sensorCenter).x;
        //     return ((colliderPositionX >= currentPositionX) && characterStatus.LookingRightWards) ||
        //            ((colliderPositionX < currentPositionX) && !characterStatus.LookingRightWards);
        // }
        //
        // private Vector2 GetSensorCenter()
        // {
        //     return (Vector2) transform.position + (_sensorCollider.offset * (characterStatus.LookingRightWards? 1: -1));
        // }
        //
        // private Vector2 GetColliderCenter(Collider2D col)
        // {
        //     return (Vector2) transform.position + col.offset;
        // }
        //
        // /// <summary>
        // /// Whether we are in any falling state.
        // /// </summary>
        // /// <returns>True if we are in any falling state. False instead.</returns>
        // private bool IamInAnyFallingState()
        // {
        //     switch (characterStatus.CurrentState)
        //     {
        //         case CharacterStatus.States.Falling:
        //         case CharacterStatus.States.FallStart:
        //             return true;
        //         default:
        //             return false;
        //     }
        // }
    }
}