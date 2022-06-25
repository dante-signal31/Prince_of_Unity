using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This sensors look for any roof over the character head and any ledge a character can climb or hang from.
    /// </summary>
    public class CeilingSensors : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to update flag about roof over the head.")]
        [SerializeField] private Animator stateMachine;
        [SerializeField] private Transform ledgeSensorStart; 
        [SerializeField] private Transform ledgeSensorEnd; 
        [SerializeField] private Transform roofSensorStart;
        [SerializeField] private Transform roofSensorEnd;
        [SerializeField] private Transform fallingLedgeSensorStart;
        [SerializeField] private Transform fallingLedgeSensorEnd;

        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;
        
        private GameObject _ledge;
        private GameObject _roof;
        private GameObject _fallingLedge;
        
        private int _architectureLayerMask;
        private int _groundLayerMask;
        private int _groundArchitectureLayerMask;
        private bool _fallingLedgeDetectionSuspended;


        enum SensorType
        {
            Ledge,
            Roof,
            FallingLedge
        };
        
        /// <summary>
        /// Whether we have a ledge we can climb or hang from.
        /// </summary>
        public bool LedgeReachable => _ledge != null;
        
        /// <summary>
        /// Whether we have a ceiling over our head.
        /// </summary>
        public bool RoofOverHead => _roof != null;
        
        /// <summary>
        /// Whether we have a ledge we can grab to while we are falling.
        /// </summary>
        public bool FallingLedgeReachable => _fallingLedge != null;
        
        /// <summary>
        /// The game object we can climb or hang from.
        /// </summary>
        public GameObject Ledge
        {
            get=> _ledge;
            private set
            {
                if ((value != null) && 
                    (value.GetComponentInChildren<Climbable>() != null) && 
                    (value != _ledge))
                {
                    stateMachine.SetBool("LedgeReachable", true);
                    _ledge = value;
                    this.Log($"(CeilingSensors - {transform.root.name}) Ledge detected.", showLogs);
                }
                else if ((value == null) && (_ledge != null))
                {
                    stateMachine.SetBool("LedgeReachable", false);
                    _ledge = null;
                    this.Log($"(CeilingSensors - {transform.root.name}) Ledge no longer reachable.", showLogs);
                }
            }
        }
        
        /// <summary>
        /// The game object we have over our head.
        /// </summary>
        public GameObject Roof
        {
            get=> _roof;
            private set
            {
                if (value != _roof)
                {
                    stateMachine.SetBool("RoofOverHead", (value != null));
                    _roof = value;
                    if (value != null)
                    {
                        this.Log($"(CeilingSensors - {transform.root.name}) Roof over our heads.", showLogs);
                    }
                    else
                    {
                        this.Log($"(CeilingSensors - {transform.root.name}) Open space over our heads.", showLogs);
                    }
                }
            }
        }
        
        /// <summary>
        /// The game object we can climb or hang from while falling.
        /// </summary>
        public GameObject FallingLedge
        {
            get=> _fallingLedge;
            private set
            {
                if ((value != null) && 
                    (value.GetComponentInChildren<Climbable>() != null) && 
                    (value != _fallingLedge))
                {
                    stateMachine.SetBool("FallingLedgeReachable", true);
                    _fallingLedge = value;
                    this.Log($"(CeilingSensors - {transform.root.name}) Falling ledge detected.", showLogs);
                }
                else if ((value == null) && (_fallingLedge != null))
                {
                    stateMachine.SetBool("FallingLedgeReachable", false);
                    _fallingLedge = null;
                    this.Log($"(CeilingSensors - {transform.root.name}) Falling ledge no longer reachable.", showLogs);
                }
            }
        }
        
        private void Awake()
        {
            // _architectureLayerMask = LayerMask.GetMask("Architecture");
            // Layer used to find climbable surfaces.
            _groundLayerMask = LayerMask.GetMask("Ground");
            // Layer used to find jumping and climbing obstacles.
            _groundArchitectureLayerMask = LayerMask.GetMask("Ground", "Architecture");
        }
        
        /// <summary>
        /// Return ledge detected in front and above of character if any, or null otherwise.
        /// </summary>
        /// <returns>Ledge detected or null otherwise.</returns>
        private GameObject DetectLedge()
        {
            // TODO: Direction and distance should be calculated just once at awake for all sensors.
            Vector2 rayDirection = ledgeSensorEnd.position - ledgeSensorStart.position;
            float forwardSensorDistance = Vector2.Distance(ledgeSensorStart.position, ledgeSensorEnd.position);
            RaycastHit2D hit = Physics2D.Raycast(ledgeSensorStart.position, 
                rayDirection, 
                forwardSensorDistance, 
                _groundLayerMask);
            return (hit.collider != null)? hit.collider.transform.root.gameObject: null;
        }
        
        /// <summary>
        /// Return roof detected above of character if any, or null otherwise.
        /// </summary>
        /// <returns>Roof detected or null otherwise.</returns>
        private GameObject DetectRoof()
        {
            Vector2 rayDirection = roofSensorEnd.position - roofSensorStart.position;
            float forwardSensorDistance = Vector2.Distance(roofSensorStart.position, roofSensorEnd.position);
            RaycastHit2D hit = Physics2D.Raycast(roofSensorStart.position, 
                rayDirection, 
                forwardSensorDistance, 
                _groundArchitectureLayerMask);
            return (hit.collider != null) ? hit.collider.transform.root.gameObject: null;
        }
        
        /// <summary>
        /// Return falling ledge detected in front and above of character if any, or null otherwise.
        /// </summary>
        /// <returns>Falling ledge detected or null otherwise.</returns>
        private GameObject DetectFallingLedge()
        {
            Vector2 rayDirection = fallingLedgeSensorEnd.position - fallingLedgeSensorStart.position;
            float forwardSensorDistance = Vector2.Distance(fallingLedgeSensorStart.position, fallingLedgeSensorEnd.position);
            RaycastHit2D hit = Physics2D.Raycast(fallingLedgeSensorStart.position, 
                rayDirection, 
                forwardSensorDistance, 
                _groundLayerMask);
            return (hit.collider != null)? hit.collider.transform.root.gameObject: null;
        }
        
        private void FixedUpdate()
        {
            Ledge = DetectLedge();
            Roof = DetectRoof();
            FallingLedge = DetectFallingLedge();
        }

        /// <summary>
        /// Reactivate at once falling ledge detection.
        /// </summary>
        public void ReactivateFallingLedgeDetection()
        {
            _fallingLedgeDetectionSuspended = false;
        }

        /// <summary>
        /// Suspend temporarily falling ledge detection.
        /// </summary>
        /// <param name="duration">Time in seconds to suspend falling ledge detection.</param>
        public void SuspendFallingLedgeDetection(float duration)
        {
            _fallingLedgeDetectionSuspended = true;
            FallingLedge = null;
            Invoke(nameof(ReactivateFallingLedgeDetection), duration);
        }


#if UNITY_EDITOR
        private void DrawSensor(SensorType sensorType)
            {
                float gizmoRadius = 0.05f;
                Vector3 gizmoSize = new Vector3(gizmoRadius, gizmoRadius, gizmoRadius);
                switch (sensorType)
                {
                    case SensorType.Ledge:
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawCube(ledgeSensorStart.position, gizmoSize);
                        Gizmos.DrawSphere(ledgeSensorEnd.position, gizmoRadius);
                        break;
                    case SensorType.Roof:
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawCube(roofSensorStart.position, gizmoSize);
                        Gizmos.DrawSphere(roofSensorEnd.position, gizmoRadius);
                        break;
                    case SensorType.FallingLedge:
                        Gizmos.color = Color.blue;
                        Gizmos.DrawCube(fallingLedgeSensorStart.position, gizmoSize);
                        Gizmos.DrawSphere(fallingLedgeSensorEnd.position, gizmoRadius);
                        break;
                }
                Gizmos.color = sensorType switch
                {
                    SensorType.Ledge => (LedgeReachable) ? Color.green : Color.red,
                    SensorType.Roof => (RoofOverHead) ? Color.green : Color.red,
                    SensorType.FallingLedge => (FallingLedgeReachable) ? Color.green : Color.red,
                };
                switch (sensorType)
                {
                    case SensorType.Ledge:
                        Gizmos.DrawLine(ledgeSensorStart.position, ledgeSensorEnd.position);
                        break;
                    case SensorType.Roof:
                        Gizmos.DrawLine(roofSensorStart.position, roofSensorEnd.position);
                        break;
                    case SensorType.FallingLedge:
                        Gizmos.DrawLine(fallingLedgeSensorStart.position, fallingLedgeSensorEnd.position);
                        break;
                }
            }
            
            private void OnDrawGizmosSelected()
            {
                DrawSensor(SensorType.Ledge);
                DrawSensor(SensorType.Roof);
                DrawSensor(SensorType.FallingLedge);
            }
#endif
        
    }
}