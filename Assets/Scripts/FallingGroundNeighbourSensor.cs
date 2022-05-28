using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component detects if another architecture brick is placed at left hand side of this one.
    ///
    /// Is useful for falling grounds to know if they need to notify its neighbour to show its border
    /// when falling starts.
    /// </summary>
    public class FallingGroundNeighbourSensor : MonoBehaviour
    { 
        [Header("WIRING:")] 
        [Tooltip("Needed to know current ground status.")]
        [SerializeField] private FallingGroundStatus fallingGroundStatus;
        [SerializeField] private Transform fallingGroundNeighbourSensorStart;
        [SerializeField] private Transform fallingGroundNeighbourSensorEnd;

        private int _architectureLayerMask;
        private float _raycastDistance;
        private bool _sensorEnabled;

        /// <summary>
        /// Architecture brick next to this one.
        /// </summary>
        public GameObject Neighbour { get; private set; }
        
        /// <summary>
        /// Whether we have detected a neighbour next to us.
        /// </summary>
        public bool NeighbourDetected => Neighbour != null;

        private void Awake()
        {
            _architectureLayerMask = LayerMask.GetMask("Architecture");
            _raycastDistance = Vector2.Distance(fallingGroundNeighbourSensorStart.position, fallingGroundNeighbourSensorEnd.position);
        }

        private void Start()
        {
            // Architecture bricks don't move so we need to run this detection once, at start.
            Neighbour = DetectNeighbour();
        }

        /// <summary>
        /// Return architecture brick next to this one.
        /// </summary>
        /// <returns>Architecture brick detected or null otherwise.</returns>
        private GameObject DetectNeighbour()
        {
            Vector2 rayDirection = fallingGroundNeighbourSensorEnd.position - fallingGroundNeighbourSensorStart.position;
            RaycastHit2D hit = Physics2D.Raycast(fallingGroundNeighbourSensorStart.position, 
                rayDirection, 
                _raycastDistance, 
                _architectureLayerMask);
            
            return (hit.collider != null) 
                ? hit.collider.transform.root.gameObject 
                : null;
        }
        
    #if UNITY_EDITOR
        private void DrawSensor()
        {
            float gizmoRadius = 0.05f;
            Vector3 gizmoSize = new Vector3(gizmoRadius, gizmoRadius, gizmoRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(fallingGroundNeighbourSensorStart.position, gizmoSize);
            Gizmos.DrawSphere(fallingGroundNeighbourSensorEnd.position, gizmoRadius);
            Gizmos.color = (NeighbourDetected) ? Color.green : Color.red;
            Gizmos.DrawLine(fallingGroundNeighbourSensorStart.position, fallingGroundNeighbourSensorEnd.position);
        }
        
        private void OnDrawGizmosSelected()
        {
            DrawSensor();
        }
    #endif
        }
}