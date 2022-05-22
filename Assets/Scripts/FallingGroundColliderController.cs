using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component disables ground collider when it falls and crash.
    /// </summary>
    public class FallingGroundColliderController : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to disable it when crash.")]
        [SerializeField] private Collider2D groundCollider;
        [Tooltip("Needed to know when we are falling.")]
        [SerializeField] private FallingGroundStatus fallingGroundStatus;

        private bool _trackingEnabled = false;
        
        private void FixedUpdate()
        {
            if (fallingGroundStatus.CurrentState == FallingGroundStatus.FallingGroundStates.Falling)
                _trackingEnabled = true;
            if (_trackingEnabled &&
                (fallingGroundStatus.CurrentState == FallingGroundStatus.FallingGroundStates.Crashing ||
                 fallingGroundStatus.CurrentState == FallingGroundStatus.FallingGroundStates.Crashed))
            {
                groundCollider.enabled = false;
                _trackingEnabled = false;
            }
        }
    }
}