using System;
using Prince;
using UnityEngine;

/// <summary>
/// Component to detect where a falling ground is going to land.
/// </summary>
public class FallingGroundCrashSensor : MonoBehaviour
{
    [Header("WIRING:")] 
    [Tooltip("Needed to know current ground status.")]
    [SerializeField] private FallingGroundStatus fallingGroundStatus;
    [SerializeField] private Transform fallingGroundCrashSensorStart;
    [SerializeField] private Transform fallingGroundCrashSensorEnd;

    private int _architectureLayerMask;
    private float _raycastDistance;
    private bool _sensorEnabled;

    /// <summary>
    /// Architecture brick where this falling floor is going to crash over.
    /// </summary>
    public GameObject CrashingOverGround { get; private set; }
    
    /// <summary>
    /// Whether we have detected the ground where we are going to crash over.
    /// </summary>
    public bool GroundBelowDetected => CrashingOverGround != null;
    
    /// <summary>
    /// Distance to ground below if one is detected. If not then equals to positive infinity.
    /// </summary>
    public float DistanceToGroundBelow = Single.PositiveInfinity;

    private void Awake()
    {
        _architectureLayerMask = LayerMask.GetMask("Ground");
        _raycastDistance = Vector2.Distance(fallingGroundCrashSensorStart.position, fallingGroundCrashSensorEnd.position);
    }

    private void FixedUpdate()
    {
        _sensorEnabled = (fallingGroundStatus.CurrentState == FallingGroundStatus.FallingGroundStates.Falling);
        (CrashingOverGround, DistanceToGroundBelow) = (_sensorEnabled) ? DetectGround(): (null, Single.PositiveInfinity);
    }
    
    /// <summary>
    /// Return architecture brick where this falling ground is going to crash if any, or null otherwise.
    /// </summary>
    /// <returns>Architecture brick detected or null otherwise.</returns>
    private (GameObject, float) DetectGround()
    {
        Vector2 rayDirection = fallingGroundCrashSensorEnd.position - fallingGroundCrashSensorStart.position;
        RaycastHit2D hit = Physics2D.Raycast(fallingGroundCrashSensorStart.position, 
            rayDirection, 
            _raycastDistance, 
            _architectureLayerMask);
        
        return (hit.collider != null) 
            ? (hit.collider.transform.root.gameObject, transform.position.y - hit.point.y) 
            : (null, Single.PositiveInfinity);
    }
    
#if UNITY_EDITOR
    private void DrawSensor()
    {
        float gizmoRadius = 0.05f;
        Vector3 gizmoSize = new Vector3(gizmoRadius, gizmoRadius, gizmoRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(fallingGroundCrashSensorStart.position, gizmoSize);
        Gizmos.DrawSphere(fallingGroundCrashSensorEnd.position, gizmoRadius);
        Gizmos.color = (GroundBelowDetected) ? Color.green : Color.red;
        Gizmos.DrawLine(fallingGroundCrashSensorStart.position, fallingGroundCrashSensorEnd.position);
    }
    
    private void OnDrawGizmosSelected()
    {
        DrawSensor();
    }
#endif
}