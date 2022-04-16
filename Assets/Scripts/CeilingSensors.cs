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

        [Header("DEBUG:")]
        [Tooltip("Show this component logs on console window.")]
        [SerializeField] private bool showLogs;
        
        private GameObject _ledge;
        private GameObject _roof;
        
        private int _architectureLayerMask;
        
        
        enum SensorType
        {
            Ledge,
            Roof
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
        /// The game object we are we can climb or hang from.
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
        
        private void Awake()
        {
            _architectureLayerMask = LayerMask.GetMask("Architecture");
        }
        
        /// <summary>
        /// Return ledge detected in front and above of character if any, or null otherwise.
        /// </summary>
        /// <returns>Ledge detected or null otherwise.</returns>
        private GameObject DetectLedge()
        {
            Vector2 rayDirection = ledgeSensorEnd.position - ledgeSensorStart.position;
            float forwardSensorDistance = Vector2.Distance(ledgeSensorStart.position, ledgeSensorEnd.position);
            RaycastHit2D hit = Physics2D.Raycast(ledgeSensorStart.position, 
                rayDirection, 
                forwardSensorDistance, 
                _architectureLayerMask);
            return (hit.collider != null) ? hit.collider.transform.root.gameObject: null;
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
                _architectureLayerMask);
            return (hit.collider != null) ? hit.collider.transform.root.gameObject: null;
        }
        
        private void FixedUpdate()
        {
            Ledge = DetectLedge();
            Roof = DetectRoof();
        }

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
                }
                Gizmos.color = sensorType switch
                {
                    SensorType.Ledge => (LedgeReachable) ? Color.green : Color.red,
                    SensorType.Roof => (RoofOverHead) ? Color.green : Color.red,
                };
                switch (sensorType)
                {
                    case SensorType.Ledge:
                        Gizmos.DrawLine(ledgeSensorStart.position, ledgeSensorEnd.position);
                        break;
                    case SensorType.Roof:
                        Gizmos.DrawLine(roofSensorStart.position, roofSensorEnd.position);
                        break;
                }
            }
            
        #if UNITY_EDITOR
            private void OnDrawGizmosSelected()
            {
                DrawSensor(SensorType.Ledge);
                DrawSensor(SensorType.Roof);
            }
        #endif
        
    }
}