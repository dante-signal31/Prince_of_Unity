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

        private bool _trackingEnabled = true;
        
        private void FixedUpdate()
        {
            if (_trackingEnabled &&
                fallingGroundStatus.CurrentState == FallingGroundStatus.FallingGroundStates.Falling)
            {
                groundCollider.enabled = false;
                _trackingEnabled = false;
            }
        }
    }
}