using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Any prefab that has ledges which can be climbed should have this behaviour to interact with
/// its grabbing ledges.
/// </summary>
public class HasGrabbingLedges : MonoBehaviour
{
    [SerializeField] private Transform GrabbingPointRight;

    [SerializeField] private Transform GrabbingPointLeft;

    /// <summary>
    /// Get this prefab grabbing position.
    /// </summary>
    /// <param name="right">True if you want right grabbing position, false if you want left one.</param>
    /// <returns>Grabbing point position.</returns>
    public Vector2 GetGrabbingPoint(bool right)
    {
        Vector3 grabbingPosition = right ? GrabbingPointRight.position : GrabbingPointLeft.position;
        return (Vector2) grabbingPosition;
    }
}
