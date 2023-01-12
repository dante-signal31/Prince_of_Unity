using UnityEngine;

namespace Prince
{
    /// <summary>
    /// This component count how much height a falling ground falls until it crashes.
    ///
    /// It is used to calculate inflicted damage to a hit character.
    /// </summary>
    public class FallingGroundFallenHeightCounter : MonoBehaviour
    {
        [Header("WIRING:")] 
        [Tooltip("Needed to know when fall starts.")]
        [SerializeField] private FallingGroundStatus fallingGroundStatus;

        /// <summary>
        /// Current fallen height since fall started.
        /// </summary>
        public float FallenHeight { get; private set; }
        
        private float _startingHeight;

        /// <summary>
        /// Get current Y coordinate where this component parent game object is placed.
        /// </summary>
        /// <returns>Current Y coordinate this component parent game object is placed.</returns>
        private float GetCurrentHeight()
        {
            return transform.root.position.y;
        }
        
        private void Start()
        {
            _startingHeight = GetCurrentHeight();
        }

        private void FixedUpdate()
        {
            if (fallingGroundStatus.CurrentState == FallingGroundStatus.FallingGroundStates.Falling)
            {
                FallenHeight = _startingHeight - GetCurrentHeight();
            }
        }
    }
}