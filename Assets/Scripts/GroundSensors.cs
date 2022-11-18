using Prince;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Component to manage character sensors to detect nearby ground.
/// </summary>
public class GroundSensors : MonoBehaviour
{
    [Header("WIRING:")]
    [Tooltip("Needed to set isFalling flag when ground is not detected any longer below feet.")]
    [SerializeField] private CharacterStatus characterStatus;
    // [Tooltip("Needed to know if gravity is enabled")] 
    // [SerializeField] private GravityController gravityController;
    [Tooltip("Needed to update state machine with ground detected.")]
    [SerializeField] private Animator stateMachine;
    [SerializeField] private Transform forwardSensorStart;
    [SerializeField] private Transform forwardSensorEnd;
    [SerializeField] private Transform rearSensorStart;
    [SerializeField] private Transform rearSensorEnd;
    [SerializeField] private Transform centerSensorStart;
    [SerializeField] private Transform centerSensorEnd;
    
    [Header("CONFIGURATION:")]
    [Tooltip("How much to advance forward and center sensor when in fighting mode.")]
    [SerializeField] private float forwardFightingModeXOffset;
    [Tooltip("How much to advance forward and center sensor when in fighting mode.")]
    [SerializeField] private float centerFightingModeXOffset;

    [Header("DEBUG:")]
    [Tooltip("Show this component logs on console window.")]
    [SerializeField] private bool showLogs;
    
    private bool _wideSensorDistribution = false;
    
    private int _architectureLayerMask;

    private float _forwardSensorDistance;
    private float _rearSensorDistance;
    private float _centerSensorDistance;
    private Vector2 _forwardRayDirection;
    private Vector2 _rearRayDirection;
    private Vector2 _centerRayDirection;
    
    private GameObject _forwardGround;
    private GameObject _rearGround;
    private GameObject _centerGround;

    public GameObject ForwardGround
    {
        get=> _forwardGround;
        private set
        {
            _forwardGround = value;
            stateMachine.SetBool("GroundAhead", (value != null));
        }
    }
    
    
    public GameObject RearGround
    {
        get=> _rearGround;
        private set
        {
            _rearGround = value;
            stateMachine.SetBool("GroundBehind", (value != null));
        }
    }

    public GameObject CenterGround
    {
        get=> _centerGround;
        private set
        {
                bool _onGround = (value != null);
                _centerGround = value;
                // Gravity enabled check is done in IsFalling property.
                characterStatus.IsFalling = !_onGround;
                // OnGround is not the same as IsFalling. With no gravity you can be
                // off ground but not being falling. With gravity enabled isFalling == !OnGround.
                stateMachine.SetBool("OnGround", _onGround);
                this.Log($"(GroundSensors - {transform.root.name}) On ground: {_onGround}.", showLogs);
        }
    }

    /// <summary>
    /// Is there ground ahead of us?
    /// </summary>
    public bool GroundAhead => ForwardGround != null;
    /// <summary>
    /// Is there ground below our feet?
    /// </summary>
    public bool GroundBelow => CenterGround != null;
    /// <summary>
    /// Is there ground behind us?
    /// </summary>
    public bool GroundBehind => RearGround != null;
    
    /// <summary>
    /// World position where character is touching ground.
    /// </summary>
    public Vector3 GroundTouchPoint { get; private set;}
    
    enum SensorType
    {
        Forward,
        Rear,
        Center
    };

    private void Awake()
    {
        _architectureLayerMask = LayerMask.GetMask("Ground");
        CalculateSensorDistances();
        CalculateRayDirections();
    }

    /// <summary>
    /// Calculate distances between starting an end sensor points.
    /// </summary>
    private void CalculateSensorDistances()
    {
        _forwardSensorDistance = Vector2.Distance(forwardSensorStart.position, forwardSensorEnd.position);
        _centerSensorDistance = Vector2.Distance(centerSensorStart.position, centerSensorEnd.position);
        _rearSensorDistance = Vector2.Distance(rearSensorStart.position, rearSensorEnd.position);
        
    }

    /// <summary>
    /// Calculate ray directions for every sensor.
    /// </summary>
    private void CalculateRayDirections()
    {
        _forwardRayDirection = (forwardSensorEnd.position - forwardSensorStart.position).normalized;
        _centerRayDirection = (centerSensorEnd.position - centerSensorStart.position).normalized;
        _rearRayDirection = (rearSensorEnd.position - rearSensorStart.position).normalized;
    }

    /// <summary>
    /// Return ground detected in front of character if any, or null otherwise.
    /// </summary>
    /// <returns>Ground detected or null otherwise.</returns>
    private GameObject DetectForward()
    {
        RaycastHit2D hit = Physics2D.Raycast(forwardSensorStart.position, 
            _forwardRayDirection, 
            _forwardSensorDistance, 
            _architectureLayerMask);
        return (hit.collider != null) ? hit.collider.transform.root.gameObject: null;
    }
    
    /// <summary>
    /// Return ground detected in back of character if any, or null otherwise.
    /// </summary>
    /// <returns>Ground detected or null otherwise.</returns>
    private GameObject DetectRear()
    {
        RaycastHit2D hit = Physics2D.Raycast(rearSensorStart.position, 
            _rearRayDirection, 
            _rearSensorDistance, 
            _architectureLayerMask);
        return (hit.collider != null) ? hit.collider.transform.root.gameObject: null;
    }
    
    /// <summary>
    /// Return ground detected below of character if any, or null otherwise.
    /// </summary>
    /// <returns>Ground detected or null otherwise.</returns>
    private GameObject DetectCenter()
    {
        RaycastHit2D hit = Physics2D.Raycast(centerSensorStart.position, 
            _centerRayDirection, 
            _centerSensorDistance, 
            _architectureLayerMask);
        if (hit.collider != null)
        {
            GroundTouchPoint = hit.point;
            return hit.collider.transform.root.gameObject;
        }
        else
        {
            GroundTouchPoint = Vector3.zero;
            return null;
        }
        // return (hit.collider != null) ? hit.collider.transform.root.gameObject: null;
    }
    
    private void FixedUpdate()
    {
        ForwardGround = DetectForward();
        CenterGround = DetectCenter();
        RearGround = DetectRear();
        UpdateSensorsPosition();
    }

    /// <summary>
    /// Fighting/running collider is bigger so forward and center sensors need to be advanced when in fighting/running mode.
    /// On the other hand they need to be reverted to its previous position when in normal mode.
    /// </summary>
    private void UpdateSensorsPosition()
    {
        switch (characterStatus.CurrentState)
        {
            // case CharacterStatus.States.RunningStart:
            case CharacterStatus.States.Running:
            case CharacterStatus.States.RunningEnd:
            case CharacterStatus.States.FallStart:
            // case CharacterStatus.States.Falling:
            case CharacterStatus.States.Unsheathe:
            case CharacterStatus.States.RunningJumpImpulse:
            case CharacterStatus.States.RunningJump:
            // case CharacterStatus.States.WalkingJumpStart:
            case CharacterStatus.States.WalkingJump:
            case CharacterStatus.States.WalkingJumpEnd:
            case CharacterStatus.States.AdvanceSword:
            case CharacterStatus.States.Retreat:
            case CharacterStatus.States.AttackWithSword:
            case CharacterStatus.States.BlockSword:
            case CharacterStatus.States.BlockedSword:
            case CharacterStatus.States.IdleSword:
            // case CharacterStatus.States.Landing:
            // case CharacterStatus.States.HardLanding:
            // case CharacterStatus.States.StandFromCrouch:    
                if (!_wideSensorDistribution) SetWideModeSensors();
                _wideSensorDistribution = true;
                break;
            default:
                if (_wideSensorDistribution) SetNormalModeSensors();
                _wideSensorDistribution = false;
                break;
        }
    }

    private void SetWideModeSensors()
    {
        Vector3 currentPosition = forwardSensorStart.localPosition;
        forwardSensorStart.localPosition = new Vector3(currentPosition.x + forwardFightingModeXOffset,
            currentPosition.y,
            currentPosition.z);
        currentPosition = centerSensorStart.localPosition;
        centerSensorStart.localPosition = new Vector3(currentPosition.x + centerFightingModeXOffset,
            currentPosition.y,
            currentPosition.z);
    }
    
    private void SetNormalModeSensors()
    {
        Vector3 currentPosition = forwardSensorStart.localPosition;
        forwardSensorStart.localPosition = new Vector3(currentPosition.x - forwardFightingModeXOffset,
            currentPosition.y,
            currentPosition.z);
        currentPosition = centerSensorStart.localPosition;
        centerSensorStart.localPosition = new Vector3(currentPosition.x - centerFightingModeXOffset,
            currentPosition.y,
            currentPosition.z);
    }
    
    private void DrawSensor(SensorType sensorType)
    {
        float gizmoRadius = 0.05f;
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
            case SensorType.Center:
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(centerSensorStart.position, gizmoSize);
                Gizmos.DrawSphere(centerSensorEnd.position, gizmoRadius);
                break;
        }
        Gizmos.color = sensorType switch
        {
            SensorType.Forward => (ForwardGround) ? Color.green : Color.red,
            SensorType.Rear => (RearGround) ? Color.green : Color.red,
            SensorType.Center => (CenterGround) ? Color.green : Color.red,
        };
        switch (sensorType)
        {
            case SensorType.Forward:
                Gizmos.DrawLine(forwardSensorStart.position, forwardSensorEnd.position);
                break;
            case SensorType.Rear:
                Gizmos.DrawLine(rearSensorStart.position, rearSensorEnd.position);
                break;
            case SensorType.Center:
                Gizmos.DrawLine(centerSensorStart.position, centerSensorEnd.position);
                break;
        }
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        DrawSensor(SensorType.Forward);
        DrawSensor(SensorType.Rear);
        DrawSensor(SensorType.Center);
    }
#endif
    
}
