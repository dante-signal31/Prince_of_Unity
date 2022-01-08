using System;
using System.Collections;
using System.Collections.Generic;
using Prince;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Component to manage character sensors to detect nearby ground.
/// </summary>
public class GroundSensors : MonoBehaviour
{
    [Tooltip("Needed to set isFalling flag when ground is not detected any longer below feet.")]
    [SerializeField] private CharacterStatus characterStatus;

    [SerializeField] private Transform forwardSensorStart;
    [SerializeField] private Transform forwardSensorEnd;
    [SerializeField] private Transform rearSensorStart;
    [SerializeField] private Transform rearSensorEnd;
    [SerializeField] private Transform centerSensorStart;
    [SerializeField] private Transform centerSensorEnd;

    private int _architectureLayerMask;
    
    private GameObject _forwardGround;
    private GameObject _rearGround;
    private GameObject _centerGround;
    
    public GameObject ForwardGround
    {
        get=> _forwardGround;
        private set => _forwardGround = value;
    }
    
    
    public GameObject RearGround
    {
        get=> _rearGround;
        private set => _rearGround = value;
    }
    
    public GameObject CenterGround
    {
        get=> _centerGround;
        private set
        {
            _centerGround = value;
            if (value == null)
            {
                characterStatus.IsFalling = true;
            }
            else
            {
                characterStatus.IsFalling = false;
            }
        }
    }
    
    enum SensorType
    {
        Forward,
        Rear,
        Center
    };

    private void Awake()
    {
        _architectureLayerMask = LayerMask.GetMask("Architecture");
    }

    /// <summary>
    /// Return ground detected in front of character if any, or null otherwise.
    /// </summary>
    /// <returns>Ground detected or null otherwise.</returns>
    private GameObject DetectForward()
    {
        Vector2 rayDirection = forwardSensorEnd.position - forwardSensorStart.position;
        float forwardSensorDistance = Vector2.Distance(forwardSensorStart.position, forwardSensorEnd.position);
        RaycastHit2D hit = Physics2D.Raycast(forwardSensorStart.position, 
            rayDirection, 
            forwardSensorDistance, 
            _architectureLayerMask);
        return (hit.collider != null) ? hit.collider.transform.parent.gameObject: null;
    }
    
    /// <summary>
    /// Return ground detected in back of character if any, or null otherwise.
    /// </summary>
    /// <returns>Ground detected or null otherwise.</returns>
    private GameObject DetectRear()
    {
        Vector2 rayDirection = rearSensorEnd.position - rearSensorStart.position;
        float rearSensorDistance = Vector2.Distance(rearSensorStart.position, rearSensorEnd.position);
        RaycastHit2D hit = Physics2D.Raycast(rearSensorStart.position, 
            rayDirection, 
            rearSensorDistance, 
            _architectureLayerMask);
        return (hit.collider != null) ? hit.collider.transform.parent.gameObject: null;
    }
    
    /// <summary>
    /// Return ground detected in back of character if any, or null otherwise.
    /// </summary>
    /// <returns>Ground detected or null otherwise.</returns>
    private GameObject DetectCenter()
    {
        Vector2 rayDirection = centerSensorEnd.position - centerSensorStart.position;
        float centerSensorDistance = Vector2.Distance(centerSensorStart.position, centerSensorEnd.position);
        RaycastHit2D hit = Physics2D.Raycast(centerSensorStart.position, 
            rayDirection, 
            centerSensorDistance, 
            _architectureLayerMask);
        return (hit.collider != null) ? hit.collider.transform.parent.gameObject: null;
    }
    
    private void FixedUpdate()
    {
        _forwardGround = DetectForward();
        _rearGround = DetectRear();
        _centerGround = DetectCenter();
    }
    
    private void DrawSensor(SensorType sensorType)
    {
        float gizmoRadius = 0.05f;
        Vector3 gizmoSize = new Vector3(gizmoRadius, gizmoRadius, gizmoRadius);
        switch (sensorType)
        {
            case SensorType.Forward:
                Handles.color = Color.blue;
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
        Handles.color = sensorType switch
        {
            SensorType.Forward => (ForwardGround) ? Color.green : Color.red,
            SensorType.Rear => (RearGround) ? Color.green : Color.red,
            SensorType.Center => (CenterGround) ? Color.green : Color.red,
        };
        switch (sensorType)
        {
            case SensorType.Forward:
                Handles.DrawLine(forwardSensorStart.position, forwardSensorEnd.position);
                break;
            case SensorType.Rear:
                Handles.DrawLine(rearSensorStart.position, rearSensorEnd.position);
                break;
            case SensorType.Center:
                Handles.DrawLine(centerSensorStart.position, centerSensorEnd.position);
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
