using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Prince
{
    /// <summary>
    /// <p>Generic component for ray sensors.</p>
    ///
    /// <p>Just place it and give it the layer were you want to detect colliders. It will emit
    /// a colliderDetecter whenever one is hit by ray and a noColliderDetected event when
    /// ray is clear. </p>s
    /// </summary>
    public class RaySensor : MonoBehaviour
    {
        [Header("WIRING:")]
        [Tooltip("Point from ray starts.")]
        [SerializeField] private Transform startPoint;
        [Tooltip("Point ray ends to.")]
        [SerializeField] private Transform endPoint;

        [Header("CONFIGURATION:")]
        [Tooltip("Layers to be detected by this sensor.")]
        [SerializeField] private List<string> layerList = new List<string>();
        [Tooltip("Event to trigger when a collider is detected by this sensor.")]    
        [SerializeField] private UnityEvent<Collider2D> colliderDetected;
        [Tooltip("Event to trigger when no collider is detected by this sensor.")]
        [SerializeField] private UnityEvent noColliderDetected;

        [Header("DEBUG:")] 
        [SerializeField] private Color pointColor = Color.green;
        [Range(0.01f, 1.0f)]
        [SerializeField] private float gizmoRadius;

        /// <summary>
        /// Whether this sensor has detected any collider.
        /// </summary>
        public bool IsColliderDetected => DetectedCollider != null;

        private Collider2D _detectedCollider;

        /// <summary>
        /// Collider currently detected by sensor.
        /// </summary>
        public Collider2D DetectedCollider
        {
            get => _detectedCollider;
            private set
            {
                if (_detectedCollider != value)
                {
                    if (value == null && noColliderDetected != null)
                    {
                        noColliderDetected.Invoke();
                    } 
                    else if (value != null && colliderDetected != null)
                    {
                        colliderDetected.Invoke(value);
                    }
                    _detectedCollider = value;
                }
            }
        }

        private Vector3 _rayDirection;
        private float _rayDistance;
        private int _layerMask;

        private void Awake()
        {
            _rayDirection = GetRayDirection();
            _rayDistance = GetRayDistance();
            _layerMask = GetLayerMask();
        }

        private Vector3 GetRayDirection()
        {
            return (endPoint.position - startPoint.position).normalized;
        }

        private float GetRayDistance()
        {
            return Vector2.Distance(endPoint.position, startPoint.position);
        }

        private int GetLayerMask()
        {
            return layerList.Count > 0? LayerMask.GetMask(layerList.ToArray()): 0;
        }

        private void FixedUpdate()
        {
            RaycastHit2D hit = Physics2D.Raycast(startPoint.position, 
                _rayDirection, 
                _rayDistance, 
                _layerMask);
            DetectedCollider = hit.collider;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            DrawSensor();
        }

        private void DrawSensor()
        {
            Vector3 gizmoSize = new Vector3(gizmoRadius, gizmoRadius, gizmoRadius);
            Gizmos.color = pointColor;
            Gizmos.DrawCube(startPoint.position, gizmoSize);
            Gizmos.DrawSphere(endPoint.position, gizmoRadius);
            Gizmos.color = IsColliderDetected ? Color.green : Color.red;
            Gizmos.DrawLine(startPoint.position, endPoint.position);
        }
#endif
    }
}