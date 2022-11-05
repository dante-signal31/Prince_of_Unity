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
        [SerializeField] private Transform roofWhileFallingSensorStart;
        [SerializeField] private Transform roofWhileFallingSensorEnd;

        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;
        
        private GameObject _ledge;
        private GameObject _roof;
        private GameObject _roofWhileFalling;
        private GameObject _fallingLedge;
        
        private int _architectureLayerMask;
        private int _groundLayerMask;
        private int _groundArchitectureLayerMask;
        private bool _fallingLedgeDetectionSuspended;


        enum SensorType
        {
            Ledge,
            Roof,
            FallingLedge,
            FallingRoof
        };
        
        /// <summary>
        /// Whether we have a ledge we can climb or hang from.
        /// </summary>
        public bool LedgeReachable => _ledge != null;
        
        /// <summary>
        /// Whether we have a ceiling over our head.
        ///
        /// Don't use this value while Prince is falling. In that case use <see cref="RoofOverHeadWhileFalling"/>
        /// </summary>
        public bool RoofOverHead => _roof != null;
        
        /// <summary>
        /// Whether we have a ceiling over our head (detected by falling sensor).
        ///
        /// While Prince is falling you should use this value instead of <see cref="RoofOverHead"/> one.
        /// </summary>
        public bool RoofOverHeadWhileFalling => _roofWhileFalling != null;
        
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
        /// The game object we have over our head while falling.
        /// </summary>
        public GameObject RoofWhileFalling
        {
            get=> _roofWhileFalling;
            private set
            {
                if (value != _roofWhileFalling)
                {
                    stateMachine.SetBool("RoofOverHeadWhileFalling", (value != null));
                    _roofWhileFalling = value;
                    if (value != null)
                    {
                        this.Log($"(CeilingSensors - {transform.root.name}) Roof over our heads while falling.", showLogs);
                    }
                    else
                    {
                        this.Log($"(CeilingSensors - {transform.root.name}) Open space over our heads while falling.", showLogs);
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
        
        private Vector2 _ledgeSensorRayDirection;
        private Vector2 _roofSensorRayDirection;
        private Vector2 _roofWhileFallingSensorRayDirection;
        private Vector2 _fallingLedgeSensorRayDirection;
        private float _ledgeSensorDistance;
        private float _roofSensorDistance;
        private float _roofWhileFallingSensorDistance;
        private float _fallingLedgeSensorDistance;
        
        private void Awake()
        {
            // _architectureLayerMask = LayerMask.GetMask("Architecture");
            // Layer used to find climbable surfaces.
            _groundLayerMask = LayerMask.GetMask("Ground");
            // Layer used to find jumping and climbing obstacles.
            _groundArchitectureLayerMask = LayerMask.GetMask("Ground", "Architecture");
            SetupRayDirections();
            SetupRayDistances();
        }

        private void SetupRayDirections()
        {
            _ledgeSensorRayDirection = (ledgeSensorEnd.position - ledgeSensorStart.position).normalized;
            _roofSensorRayDirection = (roofSensorEnd.position - roofSensorStart.position).normalized;
            _roofWhileFallingSensorRayDirection = (roofWhileFallingSensorEnd.position - roofWhileFallingSensorStart.position).normalized;
            _fallingLedgeSensorRayDirection = (fallingLedgeSensorEnd.position - fallingLedgeSensorStart.position).normalized;
        }

        private void SetupRayDistances()
        {
            _ledgeSensorDistance = Vector2.Distance(ledgeSensorStart.position, ledgeSensorEnd.position);
            _roofSensorDistance = Vector2.Distance(roofSensorStart.position, roofSensorEnd.position);
            _roofWhileFallingSensorDistance = Vector2.Distance(roofWhileFallingSensorStart.position, roofWhileFallingSensorEnd.position);
            _fallingLedgeSensorDistance = Vector2.Distance(fallingLedgeSensorStart.position, fallingLedgeSensorEnd.position);
        }
        
        /// <summary>
        /// Return ledge detected in front and above of character if any, or null otherwise.
        /// </summary>
        /// <returns>Ledge detected or null otherwise.</returns>
        private GameObject DetectLedge()
        {
            RaycastHit2D hit = Physics2D.Raycast(ledgeSensorStart.position, 
                _ledgeSensorRayDirection, 
                _ledgeSensorDistance, 
                _groundLayerMask);
            return (hit.collider != null)? hit.collider.transform.root.gameObject: null;
        }
        
        /// <summary>
        /// Return roof detected above of character if any, or null otherwise.
        /// </summary>
        /// <returns>Roof detected or null otherwise.</returns>
        private GameObject DetectRoof()
        {
            RaycastHit2D hit = Physics2D.Raycast(roofSensorStart.position, 
                _roofSensorRayDirection, 
                _roofSensorDistance, 
                _groundArchitectureLayerMask);
            return (hit.collider != null) ? hit.collider.transform.root.gameObject: null;
        }
        
        /// <summary>
        /// Return roof detected above of character while falling if any, or null otherwise.
        /// </summary>
        /// <returns>Roof detected or null otherwise.</returns>
        private GameObject DetectRoofWhileFalling()
        {
            RaycastHit2D hit = Physics2D.Raycast(roofWhileFallingSensorStart.position, 
                _roofWhileFallingSensorRayDirection, 
                _roofWhileFallingSensorDistance, 
                _groundArchitectureLayerMask);
            return (hit.collider != null) ? hit.collider.transform.root.gameObject: null;
        }
        
        /// <summary>
        /// Return falling ledge detected in front and above of character if any, or null otherwise.
        /// </summary>
        /// <returns>Falling ledge detected or null otherwise.</returns>
        private GameObject DetectFallingLedge()
        {
            RaycastHit2D hit = Physics2D.Raycast(fallingLedgeSensorStart.position, 
                _fallingLedgeSensorRayDirection, 
                _fallingLedgeSensorDistance, 
                _groundLayerMask);
            return (hit.collider != null)? hit.collider.transform.root.gameObject: null;
        }
        
        private void FixedUpdate()
        {
            Ledge = DetectLedge();
            Roof = DetectRoof();
            FallingLedge = DetectFallingLedge();
            RoofWhileFalling = DetectRoofWhileFalling();
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
                    case SensorType.FallingRoof:
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawCube(roofWhileFallingSensorStart.position, gizmoSize);
                        Gizmos.DrawSphere(roofWhileFallingSensorEnd.position, gizmoRadius);
                        break;
                }
                Gizmos.color = sensorType switch
                {
                    SensorType.Ledge => (LedgeReachable) ? Color.green : Color.red,
                    SensorType.Roof => (RoofOverHead) ? Color.green : Color.red,
                    SensorType.FallingLedge => (FallingLedgeReachable) ? Color.green : Color.red,
                    SensorType.FallingRoof => (RoofOverHeadWhileFalling) ? Color.green : Color.red
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
                    case SensorType.FallingRoof:
                        Gizmos.DrawLine(roofWhileFallingSensorStart.position, roofWhileFallingSensorEnd.position);
                        break;
                }
            }
            
            private void OnDrawGizmosSelected()
            {
                DrawSensor(SensorType.Ledge);
                DrawSensor(SensorType.Roof);
                DrawSensor(SensorType.FallingLedge);
                DrawSensor(SensorType.FallingRoof);
            }
#endif
        
    }
}