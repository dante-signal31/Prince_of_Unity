using System;
using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component controls how gravity affects falling ground.
    ///
    /// It is used mainly to activate gravity for this ground when it should fall.
    /// </summary>
    public class FallingGroundGravityController: GravityController
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
    }
}