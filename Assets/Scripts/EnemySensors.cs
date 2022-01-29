using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// Component to manage character sensors to detect enemies.
    /// </summary>
    public class EnemySensors : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Needed to set EnemySeen flags when any enemy is detected.")]
        [SerializeField] private Animator stateMachine;
        [SerializeField] private Transform forwardSensorStart;
        [SerializeField] private Transform forwardSensorEnd;
        [SerializeField] private Transform rearSensorStart;
        [SerializeField] private Transform rearSensorEnd;
        
        private bool _isGuard;
        private int _opponentMask;

        private GameObject _forwardEnemy;
        private GameObject _rearEnemy;

        public GameObject ForwardEnemy
        {
            get=> _forwardEnemy;
            private set
            {
                _forwardEnemy = value;
                if (value != null)
                {
                    stateMachine.SetBool("enemySeenForward", true);
                }
                else
                {
                    stateMachine.SetBool("enemySeenForward", false);
                }
                stateMachine.SetBool("enemySeen", EnemySeen);
            }
        }
        
        public GameObject RearEnemy {
            get=> _rearEnemy;
            private set
            {
                _rearEnemy = value;
                if (value != null)
                {
                    stateMachine.SetBool("enemySeenRear", true);
                }
                else
                {
                    stateMachine.SetBool("enemySeenRear", false);
                }
                stateMachine.SetBool("enemySeen", EnemySeen);
            }
        }

        public bool EnemySeenForward => ForwardEnemy != null;
        public bool EnemySeenRear => RearEnemy != null;
        
        public bool EnemySeen
        {
            get => EnemySeenForward || EnemySeenRear;
        }

        enum SensorType
        {
            Forward,
            Rear
        };
        
        /// <summary>
        /// UpdateAnimator flags that depend on character sensors.
        /// </summary>
        private void UpdateStateMachineFlags()
        {
            stateMachine.SetBool("enemySeen", EnemySeen);
            stateMachine.SetBool("enemySeenForward", EnemySeenForward);
            stateMachine.SetBool("enemySeenRear", EnemySeenRear);
        }
        
        private void Awake()
        {
            // This component is at Sensors subtransform so we must go to its parent to get current tag. 
            // _isGuard = gameObject.transform.parent.transform.parent.CompareTag("Guard");
            _isGuard = transform.root.CompareTag("Guard");
            _opponentMask = _isGuard ? LayerMask.GetMask("Player") : LayerMask.GetMask("Guards");
            UpdateStateMachineFlags();
        }

        /// <summary>
        /// Return opponent detected in front of character if any, or null otherwise.
        /// </summary>
        /// <returns>Opponent detected or null otherwise.</returns>
        private GameObject DetectForward()
        {
            Vector2 rayDirection = forwardSensorEnd.position - forwardSensorStart.position;
            float forwardSensorDistance = Vector2.Distance(forwardSensorStart.position, forwardSensorEnd.position);
            RaycastHit2D hit = Physics2D.Raycast(forwardSensorStart.position, 
                                                rayDirection, 
                                                forwardSensorDistance, 
                                                _opponentMask);
            return DetectOpponent(hit.collider);
        }

        /// <summary>
        /// Return opponent detected if any at character back, or null otherwise.
        /// </summary>
        /// <returns>Opponent detected or null otherwise.</returns>
        private GameObject DetectRear()
        {
            Vector2 rayDirection = rearSensorEnd.position - rearSensorStart.position;
            float rearSensorDistance = Vector2.Distance(rearSensorStart.position, rearSensorEnd.position);
            RaycastHit2D hit = Physics2D.Raycast(rearSensorStart.position, 
                rayDirection, 
                rearSensorDistance, 
                _opponentMask);
            return (hit.collider != null)? DetectOpponent(hit.collider): null;
        }

        /// <summary>
        /// Return other GameObject if other belongs to an opponent.
        ///
        /// Player opponents are Guards and Player is Guards opponent.
        /// </summary>
        /// <param name="otherCollider">Character collider detected.</param>
        /// <returns>Other GameObject if it is an opponent. Null if it is not an opponent.</returns>
        private GameObject DetectOpponent(Collider2D otherCollider)
        {
            if (otherCollider != null)
            {
                // Collider is at a subtransform of a component of Physics subtransform so we must go up to get parent tag.
                string otherTag = otherCollider.transform.root.tag;
                if (((_isGuard) && (otherTag.Equals("Player"))) ||
                    ((!_isGuard) && (otherTag.Equals("Guard"))))
                {
                    return otherCollider.transform.root.gameObject;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        
        private void FixedUpdate()
        {
            ForwardEnemy = DetectForward();
            RearEnemy = DetectRear();
        }

        private void DrawSensor(SensorType sensorType)
        {
            float gizmoRadius = 0.2f;
            Vector3 gizmoSize = new Vector3(gizmoRadius, gizmoRadius, gizmoRadius);
            switch (sensorType)
            {
                case SensorType.Forward:
                    Gizmos.color = Color.blue;
                    Gizmos.DrawCube(forwardSensorStart.position, gizmoSize);
                    Gizmos.DrawSphere(forwardSensorEnd.position, gizmoRadius);
                    break;
                case SensorType.Rear:
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawCube(rearSensorStart.position, gizmoSize);
                    Gizmos.DrawSphere(rearSensorEnd.position, gizmoRadius);
                    break;
            }
            Gizmos.color = sensorType switch
            {
                SensorType.Forward => (EnemySeenForward) ? Color.green : Color.red,
                SensorType.Rear => (EnemySeenRear) ? Color.green : Color.red,
            };
            switch (sensorType)
            {
                case SensorType.Forward:
                    Gizmos.DrawLine(forwardSensorStart.position, forwardSensorEnd.position);
                    break;
                case SensorType.Rear:
                    Gizmos.DrawLine(rearSensorStart.position, rearSensorEnd.position);
                    break;
            }
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            
            DrawSensor(SensorType.Forward);
            DrawSensor(SensorType.Rear);
        }
#endif
    }
}