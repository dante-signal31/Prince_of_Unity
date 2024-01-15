using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component controls how gravity affects falling ground.
    ///
    /// It is used mainly to activate gravity for this ground when it should fall.
    /// </summary>
    public class FallingGroundGravityController : GravityController
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to know current character state.")] 
        [SerializeField] private FallingGroundStatus stateMachineStatus;

        private void FixedUpdate()
        {
            switch (stateMachineStatus.CurrentState)
            {
                case FallingGroundStatus.FallingGroundStates.Falling:
                    if (!GravityEnabled) EnableGravity();
                    break;
                default:
                    if (GravityEnabled) DisableGravity();
                    break;
            }
        }

        /// <summary>
        /// Disables gravity for this falling ground.
        /// </summary>
        protected override void DisableGravity()
        {
            base.DisableGravity();
            // Rigid body constraints must be set. Otherwise when this component is used with
            // falling ground character pushes ground before its falling timer is up. Although
            // falling ground gravity is disabled it can still be pushed by characters that have
            // gravity enabled.
            rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}